using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class DownloadOnClick : MonoBehaviour
{
    public Text FilenameText;
    string url = "https://ttshml1.fhtsecethz.org/GestureModel";
    public void DownloadFile()
    {
        // Get file
        StartCoroutine(GetFile(GlobalUser.Id));
    }
    private IEnumerator GetFile(string subject_id)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        string path = Path.Combine(Application.persistentDataPath, "test.onnx");
        www.downloadHandler = new DownloadHandlerFile(path);

        yield return www.SendWebRequest();
        FilenameText.text = "Downloaded model";
        Debug.LogError("Downloaded model");
    }
}
