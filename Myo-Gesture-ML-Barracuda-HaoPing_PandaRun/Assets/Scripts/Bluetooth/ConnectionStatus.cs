// © 2021 ETH Zurich, Daniel G. Woolley

using UnityEngine;
using UnityEngine.UI;

using TMPro;


public class ConnectionStatus : MonoBehaviour
{
    BluetoothHandling bt;
    GameObject appManager;

    public TextMeshProUGUI status;
    public Image statusDot;
    public GameObject StartButton;

    void Start()
    {
        // get reference to BluetoothInterface script from app manager
        appManager = GameObject.Find("AppManager");
        bt = appManager.GetComponent<BluetoothHandling>();

        // set font
        status.font = GlobalText.FontAssetLight;

        // set text colour
        status.color = GlobalColours.Black;

        StartButton.SetActive(false);
    }

    void Update()
    {
        // update status text
        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.RequestEnable)
        {
            statusDot.color = GlobalColours.Red;
            status.text = GlobalText.Dictionary["home_status_enable"];
        }

        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.EnablingBluetooth)
        {
            statusDot.color = GlobalColours.MidGrey;
            status.text = GlobalText.Dictionary["home_status_enabling"];
        }

        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.Scanning)
        {
            statusDot.color = GlobalColours.MidGrey;
            status.text = GlobalText.Dictionary["home_status_searching"];
        }

        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.Connecting ||
            bt.connectionStatus == BluetoothHandling.ConnectionStatus.Subscribing)
        {
            statusDot.color = GlobalColours.Orange;
            status.text = GlobalText.Dictionary["home_status_connecting"];
        }

        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.Configured)
        {
            statusDot.color = GlobalColours.Green;
            status.text = string.Format("{0} {1} {2}", GlobalText.Dictionary["home_status_myo"], bt.deviceName.Substring(2), GlobalText.Dictionary["home_status_connected"]);
            StartButton.SetActive(true);
        }

        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.ConnectFail ||
            bt.connectionStatus == BluetoothHandling.ConnectionStatus.SubscribeFail)
        {
            statusDot.color = GlobalColours.Red;
            status.text = GlobalText.Dictionary["home_status_restart"];
        }
    }

}
