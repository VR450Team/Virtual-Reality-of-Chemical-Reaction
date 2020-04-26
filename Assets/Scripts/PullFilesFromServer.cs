using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PullFilesFromServer : MonoBehaviour
{
    private UnityWebRequest uwr;

    public string downloadUrl;
    void Start()
    {
        StartCoroutine(DownloadFile());
    }
    IEnumerator DownloadFile()
    {
        var uwr = new UnityWebRequest("http://www.razib.info/chemvr", UnityWebRequest.kHttpVerbGET);

        //Update with the proper file type or file name
        string path = Path.Combine(Application.persistentDataPath, "filetype");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        //StartCoroutine(ShowDownloadProgress(uwr));
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
        {
            //m_downloadProgress.text = (string.Format("{0:P1}", uwr.downloadProgress));
            Debug.Log("File successfully downloaded and saved to " + path);
        }
    }
}


