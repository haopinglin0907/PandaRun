// © 2021ETH Zurich, Daniel G. Woolley

using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class Language : MonoBehaviour
{
    public Dictionary<string, string> dictionary = new Dictionary<string, string>();

    // chinese font
    public TMP_FontAsset chineseFont;

    //english fonts
    public TMP_FontAsset englishFontThin;
    public TMP_FontAsset englishFontLight;
    public TMP_FontAsset englishFontRegular;
    public TMP_FontAsset englishFontMedium;

    private void Start()
    {
        if (GlobalBuildSettings.DisplayLanguage == DisplayLanguage.english)
        {
            SetEnglish();
        }

        if (GlobalBuildSettings.DisplayLanguage == DisplayLanguage.chinese)
        {
            SetChinese();
        }
    }

    public void SetEnglish()
    {
        GlobalText.Dictionary = English.dictionary;
        GlobalText.FontAssetThin = englishFontThin;
        GlobalText.FontAssetLight = englishFontLight;
        GlobalText.FontAssetRegular = englishFontRegular;
        GlobalText.FontAssetMedium = englishFontMedium;
    }

    public void SetChinese()
    {
        GlobalText.Dictionary = Chinese.dictionary;
        GlobalText.FontAssetThin = chineseFont;
        GlobalText.FontAssetLight = chineseFont;
        GlobalText.FontAssetRegular = chineseFont;
        GlobalText.FontAssetMedium = chineseFont;
    }
}
