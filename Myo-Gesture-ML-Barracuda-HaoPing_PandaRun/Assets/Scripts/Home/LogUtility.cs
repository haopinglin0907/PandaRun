// © 2021 ETH Zurich, Daniel G. Woolley

using System;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

public static class LogUtility
{
    private static string iv = "MbQeThWmZq4t7w!z";
    private static string key = "hWmZq3t6w9z$C&F)J@NcRfUjXn2r5u7x";

    public static void CreateRootDirectories()
    {
        // set user folder
        string userFolder = GlobalUser.FileCode;

        // folder path
        string folderpath = GlobalData.RootPath + userFolder + "/";

        // create folders
        Directory.CreateDirectory(folderpath);
        Directory.CreateDirectory(folderpath + "SummaryData/");
        Directory.CreateDirectory(folderpath + "TouchData/");
        Directory.CreateDirectory(folderpath + "Synced/");
    }

    public static string CreateFilePath(string filename)
    {
        // set user folder
        string userFolder =  GlobalUser.FileCode;

        // file path
        string filepath = GlobalData.RootPath + userFolder + "/" + filename + ".json";

        return filepath;
    }

    public static void WriteData(string fullFilepath, string data)
    {
        if (GlobalBuildSettings.Environment == Environment.cmuProduction || GlobalBuildSettings.Environment == Environment.fhtProduction)
        {
            string encryptedData = Encrypt(data);
            Thread thread = new Thread(() => ThreadWrite(fullFilepath, encryptedData));
            thread.Start();
        }
        else if (GlobalBuildSettings.Environment == Environment.cmuDev || GlobalBuildSettings.Environment == Environment.fhtDev)
        {
            Thread thread = new Thread(() => ThreadWrite(fullFilepath, data));
            thread.Start();
        }     
    }

    private static void ThreadWrite(string fullFilepath, string data)
    {
        File.WriteAllText(fullFilepath, data);
    }

    public static string ReadData(string fullFilepath)
    {
        string json = File.ReadAllText(fullFilepath);

        if (GlobalBuildSettings.Environment == Environment.cmuProduction || GlobalBuildSettings.Environment == Environment.fhtProduction)
        {
            return Decrypted(json);
        }
        else if (GlobalBuildSettings.Environment == Environment.cmuDev || GlobalBuildSettings.Environment == Environment.fhtDev)
        {
            return json;
        }

        return json;
    }

    private static string Encrypt(string decrypted)
    {
        byte[] textbytes = UTF8Encoding.UTF8.GetBytes(decrypted);
        AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
        endec.BlockSize = 128;
        endec.KeySize = 256;
        endec.IV = UTF8Encoding.UTF8.GetBytes(iv);
        endec.Key = UTF8Encoding.UTF8.GetBytes(key);
        endec.Padding = PaddingMode.PKCS7;
        endec.Mode = CipherMode.CBC;
        ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);
        byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
        icrypt.Dispose();
        return Convert.ToBase64String(enc);
    }

    private static string Decrypted(string encrypted)
    {
        byte[] textbytes = Convert.FromBase64String(encrypted);
        AesCryptoServiceProvider endec = new AesCryptoServiceProvider();
        endec.BlockSize = 128;
        endec.KeySize = 256;
        endec.IV = UTF8Encoding.UTF8.GetBytes(iv);
        endec.Key = UTF8Encoding.UTF8.GetBytes(key);
        endec.Padding = PaddingMode.PKCS7;
        endec.Mode = CipherMode.CBC;
        ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
        byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
        icrypt.Dispose();
        return UTF8Encoding.UTF8.GetString(enc);
    }
}
