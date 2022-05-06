// © 2021 ETH Zurich, Daniel G. Woolley

using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;


public class SideSegmentedSelection : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI errorMessage;

    public Image optionImage1;
    public Image optionImage2;

    public TextMeshProUGUI optionLabel1;
    public TextMeshProUGUI optionLabel2;

    public string selectedValue;
    public string otherValue;

    // colours for active and inactive labels and images
    private static Color32 ActiveImage
    {
        get { return GlobalColours.Blue; }
    }

    private static Color32 ActiveLabel
    {
        get { return GlobalColours.BgGrey; }
    }

    private static Color32 InactiveImage
    {
        get { return GlobalColours.LightGrey; }
    }

    private static Color32 InactiveLabel
    {
        get { return GlobalColours.MidGrey; }
    }


    private void InitialiseInputField()
    {
        title.font = GlobalText.FontAssetMedium;
        title.SetText(GlobalText.Dictionary["user_side"]);
        title.color = GlobalColours.MidGrey;

        errorMessage.font = GlobalText.FontAssetMedium;
        errorMessage.SetText(GlobalText.Dictionary["user_side_error"]);
        errorMessage.color = GlobalColours.BgGrey;

        optionLabel1.font = GlobalText.FontAssetMedium;
        optionLabel1.SetText(GlobalText.Dictionary["left"]);
        optionLabel1.color = GlobalColours.MidGrey;

        optionLabel2.font = GlobalText.FontAssetMedium;
        optionLabel2.SetText(GlobalText.Dictionary["right"]);
        optionLabel2.color = GlobalColours.MidGrey;
    }

    private void Awake()
    {
        InitialiseInputField();
    }

    public void OptionAction1()
    {
        optionImage1.color = ActiveImage;
        optionLabel1.color = ActiveLabel;

        optionImage2.color = InactiveImage;
        optionLabel2.color = InactiveLabel;

        selectedValue = "left";
        otherValue = "right";

        ResetSelectionError();
    }

    public void OptionAction2()
    {
        optionImage1.color = InactiveImage;
        optionLabel1.color = InactiveLabel;

        optionImage2.color = ActiveImage;
        optionLabel2.color = ActiveLabel;

        selectedValue = "right";
        otherValue = "left";

        ResetSelectionError();
    }

    public bool ValidateSelection()
    {
        if (!String.IsNullOrEmpty(selectedValue))
        {
            return true;
        }

        SetSelectionError();
        return false;
    }

    public void SetSelectionError()
    {
        title.color = GlobalColours.Red;
        errorMessage.color = GlobalColours.Red;
    }

    private void ResetSelectionError()
    {
        title.color = GlobalColours.MidGrey;
        errorMessage.color = GlobalColours.BgGrey;
    }

}
