// © 2021 ETH Zurich, Daniel G. Woolley

// Attached to Load object in Load scene
// Makes one time application settings required on first load

using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;


public class Load : MonoBehaviour
{

    private void Start()
    {
        // set target frame rate
        Application.targetFrameRate = 60;

        // disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // transition to main scene 
        SceneManager.LoadScene("Home");
    }
}
