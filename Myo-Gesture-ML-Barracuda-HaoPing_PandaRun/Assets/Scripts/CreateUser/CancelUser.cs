// © 2021 ETH Zurich, Daniel G. Woolley

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;


public class CancelUser : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonLabel;

    private void Start()
    {
        SetLabelText();
    }

    public void CancelAction()
    {
        SceneManager.LoadScene("Home");
    }

    private void SetLabelText()
    {
        buttonLabel.font = GlobalText.FontAssetMedium;
        buttonLabel.text = GlobalText.Dictionary["button_cancel"].ToUpper();
    }
}
