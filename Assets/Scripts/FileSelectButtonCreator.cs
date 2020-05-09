using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script is attached to the Button Creator Script object in the file select scene
public class FileSelectButtonCreator : MonoBehaviour
{
    // These objects are modified by going to the object hierarchy in the file select scene, click on the Canvas object to show it's
    // children, click on the Button Creator Script object, look at the Unity inspector, look at the File Select Button Creator (Script) section,
    // and drag and drop objects from the Project section at the bottom to the spots on the inspector.
    public GameObject buttonPrefab;
    public GameObject scrollViewContent;
    public Text displayText;

    // This function is called when the user goes to the file select scene
    void Start()
    {
        // This algorithm is mentioned in sections 3.2.3.5.1.1a and 3.2.3.5.1.2c of the SDD

        // properFiles will be filled with names of .xyz and .txt files
        List<string> properFiles = new List<string>();
        string fileExtension;
        string[] filesInCurrentDirectory = Directory.GetFiles(".");

        foreach (string file in filesInCurrentDirectory)
        {
            // Look for files that end in ".txt" or ".xyz".
            // fileExtension is last 4 characters of the file
            fileExtension = file.Substring(file.Length - 4);
            if (fileExtension == ".txt" || fileExtension == ".xyz")
			{
                // file starts with a ".\" so remove that
                properFiles.Add(file.Substring(2, file.Length - 2));
            }
        }

        // The following code changes the size of the scroll view content area.
        // A button has a height of 30 by default so make the scroll view content area be a height of 
        // (number of files * 40) to have spacing of 10 between buttons. 
        scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(1, properFiles.Count * 40);

        // Make the y coord of the 1st button be -20 because if it was 0, only the bottom half will show.
        Vector3 buttonPosition = new Vector3(0, -20, 0);

        GameObject button;
        foreach (string file in properFiles)
        {
            button = Instantiate(buttonPrefab, scrollViewContent.transform) as GameObject;
            button.transform.localPosition = buttonPosition;

            // Set the position for the next button
            buttonPosition.y -= 40;

            // reactionName is the file name without the extension
            string reactionName = file.Substring(0, file.Length - 4);
            button.GetComponentInChildren<Text>().text = reactionName;

            // Add button click listener
            button.GetComponent<Button>().onClick.AddListener(delegate { setFilePathAndDisplayMessage(file, reactionName); });
        }
    }

    // This function is mentioned in sections 3.2.3.5.1.6a and 3.2.3.5.1.7c of the SDD
    void setFilePathAndDisplayMessage(string file, string reactionName)
	{
        // Make it so that when the user clicks on a button, the filePath static variable of MainSceneScript gets changed
        // to the proper file. Because of this, when the user goes to MainScene, a function will read that file.
        MainSceneScript.filePath = file;
        
        // Let the user know which file was selected
        displayText.text = reactionName + " Selected";
    }
}
