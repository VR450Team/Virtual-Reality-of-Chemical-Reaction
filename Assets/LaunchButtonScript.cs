using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchButtonScript : MonoBehaviour
{
    public void Start()
	{
        MainSceneScript.filePath = "nothing";
	}

    public void goToMainScene()
    {
        // filePath gets changed by the user clicking a button to select a reaction.
        if (MainSceneScript.filePath != "nothing")
            SceneManager.LoadScene("MainScene");
    }
}
