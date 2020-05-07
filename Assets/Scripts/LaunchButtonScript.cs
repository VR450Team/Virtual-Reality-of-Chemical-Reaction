using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script is attached to the Launch button script object in the file select scene
public class LaunchButtonScript : MonoBehaviour
{
    // This function runs when the user goes to the file select scene
    public void Start()
	{
        MainSceneScript.filePath = "nothing";
	}

    // This function is attached to a button click listener for the launch button.
    // This function is mentioned in section 3.2.3.5.1.8c of the SDD.
    public void goToMainScene()
    {
        // filePath gets changed by the user clicking a button to select a reaction.
        if (MainSceneScript.filePath != "nothing")
            SceneManager.LoadScene("MainScene");
    }
}
