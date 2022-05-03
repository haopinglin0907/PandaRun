// © 2021 ETH Zurich, Daniel G. Woolley

using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryLevel : MonoBehaviour
{
    BluetoothHandling bt;
    GameObject appManager;

    public TextMeshProUGUI battery;
    public Image batteryIcon;

    // battery sprite array
    public Sprite[] batterySprites = new Sprite[11];

    private bool batteryQueried = false;

    private byte[] currentBatteryLevel;

    void Start()
    {
        // get reference to BluetoothInterface script from app manager
        appManager = GameObject.Find("AppManager");
        bt = appManager.GetComponent<BluetoothHandling>();

        // set font
        battery.font = GlobalText.FontAssetLight;

        // set text colour
        battery.color = GlobalColours.Black;

        UpdateBatteryLevel();

        InvokeRepeating("UpdateBatteryLevel", 0f, 5f);
    }

    private void UpdateBatteryLevel()
    {
        // get battery status whenever entering homescreen
        if (bt.connectionStatus == BluetoothHandling.ConnectionStatus.Configured &&
            bt.bluetoothState != BluetoothHandling.BluetoothState.Write &&
            bt.bluetoothState != BluetoothHandling.BluetoothState.ReadBattery)
        {
            bt.ReadBattery();
        }

        // update battery level text and icon if battery level changes
        if (!bt.batteryLevel.Equals(currentBatteryLevel) && bt.batteryLevel.Length > 0)
        {
            string batteryLevelStr = bt.batteryLevel[0].ToString();
            battery.text = batteryLevelStr + "%";

            int batteryLevelInt = int.Parse(batteryLevelStr);
            int spriteIndex = (int)Math.Round(batteryLevelInt / 10.0);
            batteryIcon.sprite = batterySprites[spriteIndex];

            currentBatteryLevel = bt.batteryLevel.ToArray();
        }
    }

}
