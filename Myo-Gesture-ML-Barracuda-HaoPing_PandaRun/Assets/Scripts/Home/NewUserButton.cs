// © 2021 ETH Zurich, Daniel G. Woolley

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class NewUserButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonLabel;

    public Button newUserButton;

    private GameObject appManager;


    private void Awake()
    {
        // get reference to data script from app manager
        appManager = GameObject.Find("AppManager");
    }


    private void Start()
    {
        SetLabelText();
        newUserButton.onClick.AddListener(TaskOnClick);
    }


    private void TaskOnClick()
    {
        SceneManager.LoadScene("CreateUser");
    }


    private void SetLabelText()
    {
        buttonLabel.font = GlobalText.FontAssetMedium;
        buttonLabel.text = GlobalText.Dictionary["button_new"].ToUpper();
    }

}
