using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is attached to the MainMenu object on the canvas in the MainMenu scene. These functions are in 
// button click listeners and get called when one of the buttons is clicked. These functions are mentioned in sections
// 3.2.3.5.1.3c, 3.2.3.5.1.2c, 
//FR.1 : The system will display a start menu when the application is launched.
//FR.2 : The start menu will display buttons for the user to click.
public class MainMenu : MonoBehaviour
{
    // This function is mentioned in section 3.2.3.5.1.3a of the SDD
    // FR.2.3 : The start menu will display a button to exit the application.
    public void quitProgram()
    {
        Application.Quit();
    }

    // This function is mentioned in section 3.2.3.5.1.1a of the SDD
    // FR.2.1 : The start menu will display a button for navigating to the file select scene.
    public void goToFileSelect()
    {
        SceneManager.LoadScene("FileSelect");
    }

    // This function is mentioned in section 3.2.3.5.1.2a of the SDD
    // FR.2.2 : The start menu will display a button for downloading files from a web server.
    public void goToDownloadScene()
    {
        SceneManager.LoadScene("Download");
    }

    //FR.2.4 : The start menu will display a help button.
    public void goToHelpMenu() 
    {
        SceneManager.LoadScene("Help");
    }
}

