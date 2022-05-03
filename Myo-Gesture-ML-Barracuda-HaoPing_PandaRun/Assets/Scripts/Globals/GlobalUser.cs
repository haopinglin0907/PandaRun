// © 2021 ETH Zurich, Daniel G. Woolley

// User data shared across scenes

public static class GlobalUser
{
    private static bool loaded;
    private static string id, displayId, trainingSide, healthySide, dateCreated, fileCode;

    private static bool recalibrate = true;


    public static bool Loaded
    {
        get { return loaded; }
        set { loaded = value; }
    }

    public static string Id
    {
        get { return id; }
        set { id = value; }
    }

    public static string DisplayId
    {
        get { return displayId; }
        set { displayId = value; }
    }

    public static string TrainingSide
    {
        get { return trainingSide; }
        set { trainingSide = value; }
    }

    public static string HealthySide
    {
        get { return healthySide; }
        set { healthySide = value; }
    }

    public static string DateCreated
    {
        get { return dateCreated; }
        set { dateCreated = value; }
    }

    public static string FileCode
    {
        get { return fileCode; }
        set { fileCode = value; }
    }

    public static bool Recalibrate
    {
        get { return recalibrate; }
        set { recalibrate = value; }
    }

}