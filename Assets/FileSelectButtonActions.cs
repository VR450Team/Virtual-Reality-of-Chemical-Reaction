using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileSelectButtonActions : MonoBehaviour
{
    string[] filePaths = {"Assets/Resources/officialReaction1.xyz",
        "Assets/Resources/officialReaction2.xyz"};
    
    public void startReaction1()
	{
        Global.filePath = filePaths[0];
        SceneManager.LoadScene("MainScene");
	}

    public void startReaction2()
	{
        Global.filePath = filePaths[1];
        SceneManager.LoadScene("MainScene");
	}
}
