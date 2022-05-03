// © 2021 ETH Zurich, Daniel G. Woolley

using UnityEngine;

public class AutoConnect : MonoBehaviour
{
    BluetoothHandling bt;
    GameObject appManager;

    void Start()
    {
        // get reference to BluetoothInterface script from app manager
        appManager = GameObject.Find("AppManager");
        bt = appManager.GetComponent<BluetoothHandling>();

        // initiate automatic device discovery and autoconnect
        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.Disconnected)
        {
            // set connection priority high - only works on android
            bt.SetHighConnectionPriority();

            // start scan
            bt.DeviceScan();
        }
    }

}