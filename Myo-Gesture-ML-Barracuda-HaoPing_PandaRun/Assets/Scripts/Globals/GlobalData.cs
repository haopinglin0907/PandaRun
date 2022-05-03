// © 2021 ETH Zurich, Daniel G. Woolley

// Data shared across scenes
using System.Collections.Generic;


public static class GlobalData
{
    private static string rootPath;

    private static int packets = 2;
    private static int channels = 8;

    private static int channel;


    public static string RootPath
    {
        get { return rootPath; }
        set { rootPath = value; }
    }

    public static int Packets
    {
        get { return packets; }
    }

    public static int Channels
    {
        get { return channels; }
    }

    public static int Channel
    {
        get { return channel; }
        set { channel = value; }
    }

}
