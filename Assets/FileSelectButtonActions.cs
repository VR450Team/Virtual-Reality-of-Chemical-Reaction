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
        Global.filePath = filePaths[0];
	}

    public void selectReaction2()
	{
        Global.filePath = filePaths[1];
	}

    public void goToMainScene()
	{
        if (Global.filePath != "nothing")
            SceneManager.LoadScene("MainScene");
	}
}
