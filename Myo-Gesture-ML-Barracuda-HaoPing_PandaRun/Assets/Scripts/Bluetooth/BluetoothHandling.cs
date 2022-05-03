// © 2021 ETH Zurich, Daniel G. Woolley

// Attached to AppManager object in Load scene - runs for life of application
// Interface to Unity BLE asset - includes all bluetooth handling

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;


public class BluetoothHandling : MonoBehaviour
{
    private string targetDevice = "zx";  // zx are the first 2 characters of all device names

    private String[] discoveryUUID = { "d5060001-a904-deb9-4748-2c7f4a124842" };

    public class Characteristic
    {
        public string ServiceUUID;
        public string CharacteristicUUID;
    }

    public static List<Characteristic> Characteristics = new List<Characteristic>
    {
        // Control service
        new Characteristic { ServiceUUID = "d5060001-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060101-a904-deb9-4748-2c7f4a124842" }, //0 - serial number - read only characteristic
        new Characteristic { ServiceUUID = "d5060001-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060201-a904-deb9-4748-2c7f4a124842" }, //1 - firmware - read only characteristic
        new Characteristic { ServiceUUID = "d5060001-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060401-a904-deb9-4748-2c7f4a124842" }, //2 - command - write only characteristic

        // IMU data service
        new Characteristic { ServiceUUID = "d5060002-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060402-a904-deb9-4748-2c7f4a124842" }, //3 - IMU data - notify only characteristic
        new Characteristic { ServiceUUID = "d5060002-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060502-a904-deb9-4748-2c7f4a124842" }, //4 - Motion event - indicate only characteristic

        // Classifier service
        new Characteristic { ServiceUUID = "d5060003-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060103-a904-deb9-4748-2c7f4a124842" }, //5 - Classifier data - indicate only characteristic

        // EMG service
        new Characteristic { ServiceUUID = "d5060005-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060105-a904-deb9-4748-2c7f4a124842" }, //6 - EMG data 1 - notify only characteristic
        new Characteristic { ServiceUUID = "d5060005-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060205-a904-deb9-4748-2c7f4a124842" }, //7 - EMG data 2 - notify only characteristic
        new Characteristic { ServiceUUID = "d5060005-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060305-a904-deb9-4748-2c7f4a124842" }, //8 - EMG data 3 - notify only characteristic
        new Characteristic { ServiceUUID = "d5060005-a904-deb9-4748-2c7f4a124842", CharacteristicUUID = "d5060405-a904-deb9-4748-2c7f4a124842" }, //9 - EMG data 4 - notify only characteristic

        // Standard services
        new Characteristic { ServiceUUID = "0000180f-0000-1000-8000-00805f9b34fb", CharacteristicUUID = "00002a19-0000-1000-8000-00805f9b34fb" }, //10 - Battery level - read/notify characteristic
        new Characteristic { ServiceUUID = "0000180f-0000-1000-8000-00805f9b34fb", CharacteristicUUID = "00002a00-0000-1000-8000-00805f9b34fb" }, //11 - Name - Read/write characterisitic
    };

    public Characteristic Command = Characteristics[2];
    public static List<Characteristic> CharacteristicsEMG = Characteristics.GetRange(6, 4);
    public Characteristic IMU = Characteristics[3];
    public Characteristic Battery = Characteristics[10];

    public enum BluetoothState
    {
        None,
        Initialise,
        Scan,
        Connect,
        ConnectTimeout,
        SubscribeBattery,
        SubscribeBatteryTimeout,
        SubscribeEMG,
        SubscribeEMGTimeout,
        SubscribeIMU,
        SubscribeIMUTimeout,
        ConfigureSleep,
        ConfigureMode,
        ReadBattery,
        ReadBatteryTimeout,
        Write,
        Disconnect,
        Deinitialise
    }

    public BluetoothState bluetoothState;

    public enum ConnectionStatus
    {
        None,
        EnablingBluetooth,
        RequestEnable,
        Scanning,
        Connecting,
        ConnectFail,
        Subscribing,
        SubscribeFail,
        Subscribed,
        Configured,
        Disconnecting,
        Disconnected
    }

    public ConnectionStatus connectionStatus;

    // device name for display on home screen
    public string deviceName;

    // interface states
    public bool initialised = false;  // do not reset

    // local parameters
    private float timeout;

    private string deviceAddress;

    private byte[] command;

    private int subCountEMG;
    private int subFailCountEMG;
    private int subFailCountIMU;
    private int subFailCountBattery;

    // dictionary used to store available devices
    public Dictionary<int, Tuple<string, string>> availableDevices = new Dictionary<int, Tuple<string, string>>();

    // list to store data from all raw emg characteristics in array - cleared on consumption in data.cs
    public List<string> emgData = new List<string>();

    // list to store data from all imu characteristic - cleared on consumption in data.cs
    public List<string> imuData = new List<string>();

    // list of timestamps
    public List<double> rawTimestamps = new List<double>();

    // battery level
    public byte[] batteryLevel;

    // configuration commands
    byte[] noSleep = new byte[] { 0x09, 0x01, 0x01 };
    byte[] setMode = new byte[] { 0x01, 0x03, 0x02, 0x01, 0x00 };


    private void InitialiseParameters()
    {
        timeout = 0f;

        deviceAddress = null;

        subCountEMG = 0;
        subFailCountEMG = 0;
        subFailCountBattery = 0;

        availableDevices.Clear();
    }

    // called from Start() in Connect.cs
    public void DeviceScan()
    {
        // start scanning by initialising parameters
        InitialiseParameters();

        // scan if bluetooth initialised, otherwise initialise first
        if (initialised)
        {
            SetState(BluetoothState.Scan, 0.5f);
            connectionStatus = ConnectionStatus.Scanning;
        }
        else
        {
            // if entered from bluetooth enable request, deinitialise before initialisation
            if (connectionStatus == ConnectionStatus.RequestEnable)
            {
                BluetoothLEHardwareInterface.DeInitialize(() =>
                {
                    initialised = false;
                    SetState(BluetoothState.Initialise, 0.5f);
                });
            }
            else
            {
                SetState(BluetoothState.Initialise, 0.05f);
            }
        }
    }

    // Called from BluetoothState.Scan when device is available
    public void DeviceConnect(string selectedAddress)
    {
        // set connection status
        connectionStatus = ConnectionStatus.Connecting;

        // stop scanning if necessary
        if (bluetoothState == BluetoothState.Scan) BluetoothLEHardwareInterface.StopScan();

        // set device address
        deviceAddress = selectedAddress;

        // set connect state
        SetState(BluetoothState.Connect, 0.01f);
    }

    // called from ...
    public void DeviceDisconnect()
    {
        connectionStatus = ConnectionStatus.Disconnecting;
        SetState(BluetoothState.Disconnect, 0.01f);
    }

    // called from ...
    public void DeviceWrite(byte[] _command)
    {
        command = _command;
        SetState(BluetoothState.Write, 0.01f);
    }

    // called from ...
    public void DevicePause(bool isPaused)
    {
        BluetoothLEHardwareInterface.PauseMessages(isPaused);
    }

    // called from AutoConnect.cs
    public void SetHighConnectionPriority()
    {
        BluetoothLEHardwareInterface.BluetoothConnectionPriority(BluetoothLEHardwareInterface.ConnectionPriority.High);
    }

    // called from BatteryLevel.cs
    public void ReadBattery()
    {
        SetState(BluetoothState.ReadBattery, 2f);
    }

    private void Start()
    {
        connectionStatus = ConnectionStatus.Disconnected;
        bluetoothState = BluetoothState.None;
    }

    void Update()
    {
        if (timeout > 0f)
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0f)
            {
                timeout = 0f;

                switch (bluetoothState)
                {
                    case BluetoothState.None:
                        break;

                    case BluetoothState.Initialise:
                        BluetoothLEHardwareInterface.Initialize(true, false, () =>
                        {
                            initialised = true;
                            connectionStatus = ConnectionStatus.Scanning;
                            SetState(BluetoothState.Scan, 0.5f);
                        }, (error) =>
                        {
#if UNITY_ANDROID
                            connectionStatus = ConnectionStatus.EnablingBluetooth;
                            BluetoothLEHardwareInterface.BluetoothEnable(true);
                            BluetoothLEHardwareInterface.DeInitialize(() =>
                            {
                                initialised = false;
                                SetState(BluetoothState.Initialise, 0.5f);
                            });
#endif

#if UNITY_IOS
                            connectionStatus = ConnectionStatus.RequestEnable;
                            DeviceScan();
#endif

                        });
                        break;

                    case BluetoothState.Scan:
                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(discoveryUUID, null,
                        (_address, _deviceName, _rssi, _packet) =>
                        {
                            availableDevices[_rssi] = new Tuple<string, string>(_deviceName, _address);
                            int selectedDeviceKey = availableDevices.Keys.Max();
                            string selectedDeviceName = availableDevices[selectedDeviceKey].Item1;
                            string selectedDeviceAddress = availableDevices[selectedDeviceKey].Item2;

                            if (selectedDeviceName.Contains(targetDevice))
                            {
                                deviceName = selectedDeviceName;
                                DeviceConnect(selectedDeviceAddress);
                            }
                            else
                            {
                                SetState(BluetoothState.Scan, 0.5f);
                            }
                        }, true);
                        break;

                    case BluetoothState.Connect:
                        SetState(BluetoothState.ConnectTimeout, 5.0f);
                        BluetoothLEHardwareInterface.ConnectToPeripheral(deviceAddress, (_address) =>
                        {
                            connectionStatus = ConnectionStatus.Subscribing;
                            SetState(BluetoothState.SubscribeBattery, 1.5f);
                        }, null, null, null);
                        break;

                    case BluetoothState.ConnectTimeout:
                        connectionStatus = ConnectionStatus.ConnectFail;
                        SetState(BluetoothState.None, 0);
                        break;

                    case BluetoothState.SubscribeBattery:
                        SetState(BluetoothState.SubscribeBatteryTimeout, 1.0f);
                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(deviceAddress, Battery.ServiceUUID, Battery.CharacteristicUUID,
                            (_deviceAddress, _characteristic) => {
                                SetState(BluetoothState.SubscribeEMG, 1.5f);
                            },
                            (_deviceAddress, _characteristic, _bytes) => {
                                batteryLevel = _bytes;
                            }
                        );
                        break;

                    case BluetoothState.SubscribeBatteryTimeout:
                        subFailCountBattery++;
                        if (subFailCountBattery < 8)
                        {
                            SetState(BluetoothState.SubscribeBattery, 0.1f);
                        }
                        else
                        {
                            SetState(BluetoothState.SubscribeEMG, 1.5f);
                        }
                        break;

                    case BluetoothState.SubscribeEMG:
                        SetState(BluetoothState.SubscribeEMGTimeout, 1.0f);
                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(deviceAddress, CharacteristicsEMG[subCountEMG].ServiceUUID, CharacteristicsEMG[subCountEMG].CharacteristicUUID,
                            (_deviceAddress, _characteristic) => {
                                subCountEMG++;
                                if (subCountEMG < CharacteristicsEMG.Count)
                                {
                                    SetState(BluetoothState.SubscribeEMG, 0.1f);
                                }
                                else
                                {                                  
                                    SetState(BluetoothState.SubscribeIMU, 1.5f);
                                }
                            },
                            (_deviceAddress, _characteristic, _bytes) => {
                                // raw emg data consumed in DataHandling.cs
                                emgData.Add(BitConverter.ToString(_bytes, 0));
                                rawTimestamps.Add(Time.realtimeSinceStartup);
                            }
                        );
                        break;

                    case BluetoothState.SubscribeEMGTimeout:
                        subFailCountEMG++;
                        if (subFailCountEMG < 16)
                        {
                            SetState(BluetoothState.SubscribeEMG, 0.1f);
                        }
                        else
                        {
                            connectionStatus = ConnectionStatus.SubscribeFail;
                            SetState(BluetoothState.None, 0);
                        }
                        break;

                    case BluetoothState.SubscribeIMU:
                        SetState(BluetoothState.SubscribeIMUTimeout, 1.0f);
                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(deviceAddress, IMU.ServiceUUID, IMU.CharacteristicUUID,
                            (_deviceAddress, _characteristic) => {
                                connectionStatus = ConnectionStatus.Subscribed;
                                SetState(BluetoothState.ConfigureSleep, 0.1f);
                            },
                            (_deviceAddress, _characteristic, _bytes) => {
                                // raw imu data consumed in DataHandling.cs
                                imuData.Add(BitConverter.ToString(_bytes, 0));
                            }
                        );
                        break;

                    case BluetoothState.SubscribeIMUTimeout:
                        subFailCountIMU++;
                        if (subFailCountIMU < 8)
                        {
                            SetState(BluetoothState.SubscribeIMU, 0.1f);
                        }
                        else
                        {
                            connectionStatus = ConnectionStatus.SubscribeFail;
                            SetState(BluetoothState.None, 0);
                        }
                        break;

                    case BluetoothState.ConfigureSleep:
                        BluetoothLEHardwareInterface.WriteCharacteristic(deviceAddress, Command.ServiceUUID, Command.CharacteristicUUID, noSleep, noSleep.Length, true, (_message) =>
                        {
                            SetState(BluetoothState.ConfigureMode, 0.1f);
                        });
                        break;

                    case BluetoothState.ConfigureMode:
                        BluetoothLEHardwareInterface.WriteCharacteristic(deviceAddress, Command.ServiceUUID, Command.CharacteristicUUID, setMode, setMode.Length, true, (_message) =>
                        {
                            connectionStatus = ConnectionStatus.Configured;
                            SetState(BluetoothState.None, 0);
                        });
                        break;

                    case BluetoothState.ReadBattery:
                        SetState(BluetoothState.ReadBatteryTimeout, 2.0f);
                        BluetoothLEHardwareInterface.ReadCharacteristic(deviceAddress, Battery.ServiceUUID, Battery.CharacteristicUUID, (_characteristic, _bytes) =>
                        {
                            batteryLevel = _bytes;
                            SetState(BluetoothState.None, 0);
                        });
                        break;

                    case BluetoothState.ReadBatteryTimeout:
                        SetState(BluetoothState.None, 0);
                        break;

                    case BluetoothState.Disconnect:
                        BluetoothLEHardwareInterface.DisconnectPeripheral(deviceAddress, (address) =>
                        {
                            connectionStatus = ConnectionStatus.Disconnected;
                            SetState(BluetoothState.None, 0);
                        });
                        break;

                    case BluetoothState.Write:
                        BluetoothLEHardwareInterface.WriteCharacteristic(deviceAddress, Command.ServiceUUID, Command.CharacteristicUUID, command, command.Length, true, (_message) =>
                        {
                            SetState(BluetoothState.None, 0);
                        });
                        break;

                    case BluetoothState.Deinitialise:
                        BluetoothLEHardwareInterface.DeInitialize(() =>
                        {
                            initialised = false;
                            SetState(BluetoothState.None, 0);
                        });
                        break;

                }
            }
        }
    }

    private void SetState(BluetoothState newState, float _timeout)
    {
        bluetoothState = newState;
        timeout = _timeout;
    }

}