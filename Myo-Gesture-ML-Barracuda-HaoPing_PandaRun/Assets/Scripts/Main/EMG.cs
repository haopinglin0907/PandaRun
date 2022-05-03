using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class EMG : MonoBehaviour
{

    private GameObject appManager;
    private DataHandling dataHandling;

    // threshold to detect maximum
    private int maxThreshold = 1;

    // emg value for scaling display
    private float emgScale = 128f;

    // reference to active bar container
    public GameObject activeBarContainer;
    RectTransform activeBarContainerRT;

    // active bar container and bar objects
    public GameObject activeBar;

    // active bar container and component lists
    List<RectTransform> activeBarRTs = new List<RectTransform>();
    List<Image> activeBarImages = new List<Image>();


    private void Awake()
    {
        // get reference to data script from app manager
        appManager = GameObject.Find("AppManager");
        dataHandling = appManager.GetComponent<DataHandling>();
    }


    private void Start()
    {
        // reflects layout size - used for updating scaling factor 
        activeBarContainerRT = activeBarContainer.GetComponent<RectTransform>();

        for (int i = 0; i < GlobalData.Channels; i++)
        {
            // instantiate active bar, get rt and add to list, add bar to image list, add bar to bar list
            GameObject activeBarClone = Instantiate(activeBar, activeBarContainer.transform);
            activeBarRTs.Add(activeBarClone.GetComponent<RectTransform>());
            activeBarImages.Add(activeBarClone.GetComponent<Image>());
        }
    }


    private void Update()
    {
        if (dataHandling.kalmanFilterEMG.Count == GlobalData.Channels)
        {
            for (int i = 0; i < GlobalData.Channels; i++)
            {
                // calculate scaling factor based on bar container height - max emg value = 128
                float scalingFactor = activeBarContainerRT.rect.height / emgScale;

                // calculate active bar heights
                float activeBarHeight = (float)dataHandling.kalmanFilterEMG[i] * scalingFactor;

                // set active bar heights
                activeBarImages[i].rectTransform.sizeDelta = new Vector2(activeBarRTs[i].rect.width, activeBarHeight);
            }
        }
    }


    public void ZoomIn()
    {
        if (emgScale == 64f)
            emgScale = 32f;
        else if (emgScale == 128f)
            emgScale = 64f;
    }


    public void ZoomOut()
    {
        if (emgScale == 64f)
            emgScale = 128f;
        else if (emgScale == 32f)
            emgScale = 64f;
    }

}
