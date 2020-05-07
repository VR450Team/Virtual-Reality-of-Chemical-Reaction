using System.Collections;
using System.Collections.Generic;
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

        List<string> filePaths = new List<string>();
        foreach (string filePath in System.IO.Directory.GetFiles("Assets/Files/"))
        {
            // Look for files that end in ".txt".
            // Some files will be .meta files so ignore those.
            if (filePath.Substring(filePath.Length - 4) == ".txt")
                filePaths.Add(filePath);
        }

        // The following code changes the size of the scroll view content area.
        // A button has a height of 30 by default so make the scroll view content area be a height of 
        // (number of files * 40) to have spacing of 10 between buttons. 
        scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(1, filePaths.Count * 40);

        // Make the y coord of the 1st button be -20 because if it was 0, only the bottom half will show.
        Vector3 buttonPosition = new Vector3(0, -20, 0);

        GameObject button;
        foreach (string filePath in filePaths)
        {
            button = Instantiate(buttonPrefab, scrollViewContent.transform) as GameObject;
            button.transform.localPosition = buttonPosition;

            // Set the position for the next button
            buttonPosition.y -= 40;

            // The first 13 characters are "Assets/Files/" and we don't want that in the button's text.
            // The second parameter is the length of the substring, and we need to set it to filePath.Length
            // - 17 so we the ".txt" at the end isn't included.
            string reactionName = filePath.Substring(13, filePath.Length - 17);
            button.GetComponentInChildren<Text>().text = reactionName;

            // Add button click listener
            button.GetComponent<Button>().onClick.AddListener(delegate { setFilePathAndDisplayMessage(filePath, reactionName); });
        }
    }

    // This function is mentioned in sections 3.2.3.5.1.6a and 3.2.3.5.1.7c of the SDD
    void setFilePathAndDisplayMessage(string filePath, string reactionName)
	{
        MainSceneScript.filePath = filePath;
        displayText.text = reactionName + " Selected";
    }
}
