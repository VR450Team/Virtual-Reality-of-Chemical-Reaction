using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileSelectButtonActions : MonoBehaviour
{
    string[] filePaths = {"Assets/Resources/officialReaction1.xyz",
        "Assets/Resources/officialReaction2.xyz"};
    
    public void selectReaction1()
	{
        MainSceneScript.filePath = filePaths[0];
	}

    public void selectReaction2()
	{
        MainSceneScript.filePath = filePaths[1];
	}

    public void goToMainScene()
	{
        if (MainSceneScript.filePath != "nothing")
            SceneManager.LoadScene("MainScene");
	}
}
