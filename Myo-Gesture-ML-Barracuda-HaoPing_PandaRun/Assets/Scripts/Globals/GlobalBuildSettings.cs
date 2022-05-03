// © 2021 ETH Zurich, Daniel G. Woolley

public static class GlobalBuildSettings
{
    public static Environment Environment = Environment.fhtDev;
    public static Support Support = Support.ot;
    public static DisplayLanguage DisplayLanguage = DisplayLanguage.english;

    public static string Version = "FHT-OT DEV 12.04.2022 (EN)";
}

public enum Environment { cmuDev, cmuProduction, fhtDev, fhtProduction };
public enum Support { ot, minimal };  // currently for different versions of the fht app
public enum DisplayLanguage { english, chinese };  // applied globally within app