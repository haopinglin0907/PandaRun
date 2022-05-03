// © 2021 ETH Zurich, Daniel G. Woolley

using System.Collections.Generic;

using TMPro;


public static class GlobalText
{
    private static Dictionary<string, string> dictionary;

    private static TMP_FontAsset fontAssetThin;
    private static TMP_FontAsset fontAssetLight;
    private static TMP_FontAsset fontAssetRegular;
    private static TMP_FontAsset fontAssetMedium;

    public static Dictionary<string, string> Dictionary
    {
        get { return dictionary; }
        set { dictionary = value; }
    }

    public static TMP_FontAsset FontAssetThin
    {
        get { return fontAssetThin; }
        set { fontAssetThin = value; }
    }

    public static TMP_FontAsset FontAssetLight
    {
        get { return fontAssetLight; }
        set { fontAssetLight = value; }
    }

    public static TMP_FontAsset FontAssetRegular
    {
        get { return fontAssetRegular; }
        set { fontAssetRegular = value; }
    }

    public static TMP_FontAsset FontAssetMedium
    {
        get { return fontAssetMedium; }
        set { fontAssetMedium = value; }
    }
}
