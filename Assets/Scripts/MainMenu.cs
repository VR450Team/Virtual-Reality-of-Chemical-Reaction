using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is attached to the MainMenu object on the canvas in the MainMenu scene. These functions are in 
// button click listeners and get called when one of the buttons is clicked.
public class MainMenu : MonoBehaviour
{
    public void quitProgram()
    {
        Application.Quit();
    }
    
    public void goToFileSelect()
    {
        SceneManager.LoadScene("FileSelect");
    }

    public void goToDownloadScene()
    {
        SceneManager.LoadScene("Download");
    }

    public void goToHelpMenu() 
    {
        SceneManager.LoadScene("Help");
    }
}

