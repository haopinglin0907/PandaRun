// © 2021 ETH Zurich, Daniel G. Woolley

// Attached to AppManager object in Load scene - runs for life of application
// Consumes data collected by BlutoothHandling, converts from hex to int, and applies filtering

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;


public class DataHandling : MonoBehaviour
{
    GameObject appManager;
    BluetoothHandling bt;

    // n samples for window filtering
    private int windowSize = 50;

    public class KalmanFilter
    {
        private double A, H, Q, R, P, x;

        public KalmanFilter(double A, double H, double Q, double R, double initial_P, double initial_x)
        {
            this.A = A;  // system constant
            this.H = H;  // gain constant
            this.Q = Q;  // process noise covariance
            this.R = R;  // measurement noise covariance
            this.P = initial_P;  // estimation error covariance
            this.x = initial_x;  // value of interest
        }

        public double Output(double input)
        {
            // time update - prediction
            x = A * x;
            P = A * P * A + Q;

            // measurement update - correction
            double K = P * H / (H * P * H + R);  // kalman gain
            x = x + K * (input - H * x);
            P = (1 - K * H) * P;

            return x;
        }
    }

    // stores kalman filter for each channel
    private Dictionary<int, KalmanFilter> kalmanFilters = new Dictionary<int, KalmanFilter>();

    public class WindowFilter
    {
        private int windowSize;
        private List<int> windowBuffer;

        public WindowFilter(int windowSize)
        {
            this.windowSize = windowSize;
            this.windowBuffer = new List<int>();
        }

        public double Output(List<int> signedEMG)
        {
            List<int> unsignedEMG = new List<int>();

            // convert signed emg data to unsigned
            for (int i = 0; i < signedEMG.Count; i++)
            {
                unsignedEMG.Add(Math.Abs(signedEMG[i]));
            }

            // add new data to buffer
            windowBuffer.AddRange(unsignedEMG);

            // remove old data that exceeds window size
            if (windowBuffer.Count > windowSize) windowBuffer.RemoveRange(0, windowBuffer.Count - windowSize);

            // average buffer and add to list
            double windowAverageEMG = windowBuffer.Average();

            return windowAverageEMG;
        }
    }

    // stores window filters for each channel
    private Dictionary<int, WindowFilter> windowFilters = new Dictionary<int, WindowFilter>();

    // raw EMG data to be accessed in other scripts
    public Dictionary<int, List<int>> newEMG = new Dictionary<int, List<int>>();

    // filtered EMG to be accessed in other scripts
    public List<double> windowFilterEMG = new List<double>();
    public List<double> kalmanFilterEMG = new List<double>();

    // IMU data
    public Dictionary<int, List<float>> newIMU = new Dictionary<int, List<float>>();

    // timestamps to be accessed in other scripts
    public float filterTimestamp;

    private void InitialiseFilters()
    {
        for (int i = 0; i < GlobalData.Channels; i++)
        {
            windowFilters[i] = new WindowFilter(windowSize);
            kalmanFilters[i] = new KalmanFilter(1, 1, 0.01, 1, 0, 0);
        }
    }

    private void Awake()
    {
        // get reference to BluetoothInterface script from app manager
        appManager = GameObject.Find("AppManager");
        bt = appManager.GetComponent<BluetoothHandling>();
    }

    private void Start()
    {
        InitialiseFilters();
    }

    private void Update()
    {
        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.Configured)
        {
            // get new EMG data  
            newEMG.Clear();
            newEMG = GetNewEMG();

            // calculate window average - returns one sample for each channel - only if new data is available
            if (newEMG.Count > 0)
            {
                windowFilterEMG.Clear();
                windowFilterEMG = WindowFilterEMG(newEMG);
            }

            // kalman filter averaging - use old data for update if new data is not available
            kalmanFilterEMG.Clear();
            kalmanFilterEMG = KalmanFilterEMG(windowFilterEMG);

            // get new IMU data
            newIMU.Clear();
            newIMU = GetNewIMU();

            // timestamp for filtered data
            filterTimestamp = Time.realtimeSinceStartup;
        }
    }


    private Dictionary<int, List<int>> GetNewEMG()
    {
        // copy bluetooth data notification list
        List<string> hexPackage = new List<string>(bt.emgData);

        // clear bluetooth data notifiction list
        bt.emgData.Clear();

        // copy timestamps
        List<double> rawTimestamps = new List<double>(bt.rawTimestamps);

        // clear timestamps
        bt.rawTimestamps.Clear();

        Dictionary<int, List<int>> _newEMG = new Dictionary<int, List<int>>();

        // each sample is a list of 8 channels of emg data from 1 timepoint
        for (int i = 0; i < GlobalData.Channels; i++)
        {
            // sample list in channel order
            List<int> emgChannelPacket = new List<int>();

            foreach (string hexPacket in hexPackage)
            {
                // split hex string into array
                string[] hexArray = hexPacket.Split('-');

                for (int j = 0; j < GlobalData.Packets; j++)
                {
                    // convert hex to unsigned int (signed data is transmitted as unsigned 8 bit integer)
                    int unsignedEMG = Convert.ToByte(hexArray[i + (j * GlobalData.Channels)], 16);
                    // convert to signed int
                    int signedEMG = TwosComplement(unsignedEMG);
                    // convert to absolute value
                    //int absEMG = Math.Abs(signedEMG);
                    // add to channel list 
                    //emgChannelPacket.Add(absEMG);
                    // add signed data to channel list
                    emgChannelPacket.Add(signedEMG);
                }
            }

            // add sample to main storage list
            if (emgChannelPacket.Count > 0) _newEMG.Add(i, emgChannelPacket);
        }

        return _newEMG;
    }


    private int TwosComplement(int val)
    {
        int convertedVal = (val & 0x80) == 0 ? val : (byte)(~(val - 0x01)) * -1;
        return convertedVal;
    }


    private List<double> WindowFilterEMG(Dictionary<int, List<int>> emg)
    {
        List<double> _windowFilterEMG = new List<double>();

        // for each channel
        for (int i = 0; i < GlobalData.Channels; i++)
        {
            _windowFilterEMG.Add(windowFilters[i].Output(emg[i]));
        }

        return _windowFilterEMG;
    }

    private List<double> KalmanFilterEMG(List<double> emg)
    {
        List<double> _kalmanFilterEMG = new List<double>();

        for (int i = 0; i < GlobalData.Channels; i++)
        {
            _kalmanFilterEMG.Add(kalmanFilters[i].Output(emg[i]));
        }

        return _kalmanFilterEMG;
    }

    private Dictionary<int, List<float>> GetNewIMU()
    {
        // copy bluetooth data notification list
        List<string> hexPackage = new List<string>(bt.imuData);

        // clear bluetooth data notifiction list
        bt.imuData.Clear();

        Dictionary<int, List<float>> _newIMU = new Dictionary<int, List<float>>();

        if (hexPackage.Count > 0)
        {
            for (int i = 0; i < hexPackage.Count; i++)
            {
                List<float> imuPacket = new List<float>();

                // get hex packet
                string hexPacket = hexPackage[i];

                // split hex string into array
                string[] hexArray = hexPacket.Split('-');

                for (int j = 0; j < hexArray.Count(); j += 2)
                {
                    // convert hex to signed int (imu data is transmitted as signed 16 bit integer)
                    int val = Convert.ToInt16(hexArray[j + 1] + hexArray[j], 16);

                    // scale and add to list 
                    if (j < 7) // first four shorts are orientation -> scaling factor 16384.0f 
                    {
                        imuPacket.Add(val / 16384.0f);
                    }
                    else if (j > 7 && j < 13) // next three shorts are acceleration -> scaling factor 2048.0f 
                    {
                        imuPacket.Add(val / 2048.0f);
                    }
                    else if (j > 13)  // last three shorts are gyro -> scaling factor 16.0f 
                    {
                        imuPacket.Add(val / 16.0f);
                    }
                }

                _newIMU.Add(i, imuPacket);

            }
        }

        return _newIMU;
    }

}
