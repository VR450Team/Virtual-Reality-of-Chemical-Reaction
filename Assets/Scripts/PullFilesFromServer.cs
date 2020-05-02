using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PullFilesFromServer : MonoBehaviour
{
    private readonly UnityWebRequest uwr;

    public string downloadUrl;
    void Start()
    {
        StartCoroutine(DownloadFile());
    }
    IEnumerator DownloadFile()
    {
        var uwr = new UnityWebRequest("http://people.missouristate.edu/riqbal/data/mosgcone.txt", UnityWebRequest.kHttpVerbGET);

        //Update with the proper file type or file name
        //Stores to the location C:\Users\(YourUserName)\AppData\LocalLow\DefaultCompany\Virtual Reality of Chemical Reactions
        string path = Path.Combine(Application.persistentDataPath, "ReactionFile");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
        {
            Debug.Log("File successfully downloaded and saved to " + path);
        }
    }
}


