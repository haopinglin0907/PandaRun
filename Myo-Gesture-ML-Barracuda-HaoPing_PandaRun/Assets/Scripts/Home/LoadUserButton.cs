// © 2021 ETH Zurich, Daniel G. Woolley

using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class LoadUserButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonLabel;

    public Button loadUserButton;

    private GameObject appManager;


    private void Awake()
    {
        // get reference to data script from app manager
        appManager = GameObject.Find("AppManager");
    }

    private void Start()
    {
        SetLabelText();
        loadUserButton.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {

        // use asynchronous loading to avoid frame rate drop with many users
        StartCoroutine(LoadUserActionAsync());
    }

    IEnumerator LoadUserActionAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadUserId");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void SetLabelText()
    {
        buttonLabel.font = GlobalText.FontAssetMedium;
        buttonLabel.text = GlobalText.Dictionary["button_load"].ToUpper();
    }

}
