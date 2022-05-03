// © 2021 ETH Zurich, Daniel G. Woolley

// Attached to AppManager object in Load scene
// All scripts attached to the AppManager object continue to run for the life of the application

using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    public GameObject appManager;

    void Awake()
    {
        DontDestroyOnLoad(appManager);
    }
}