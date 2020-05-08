using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is attached to the MainMenu object on the canvas in the MainMenu scene. These functions are in 
// button click listeners and get called when one of the buttons is clicked.
public class MainMenu : MonoBehaviour
{
    // This function is mentioned in section 3.2.3.5.1.3a of the SDD
    public void quitProgram()
    {
        Application.Quit();
    }

    // This function is mentioned in section 3.2.3.5.1.1a of the SDD
    public void goToFileSelect()
    {
        SceneManager.LoadScene("FileSelect");
    }

    // This function is mentioned in section 3.2.3.5.1.2a of the SDD
    public void goToDownloadScene()
    {
        SceneManager.LoadScene("Download");
    }

    public void goToHelpMenu() 
    {
        SceneManager.LoadScene("Help");
    }
}

