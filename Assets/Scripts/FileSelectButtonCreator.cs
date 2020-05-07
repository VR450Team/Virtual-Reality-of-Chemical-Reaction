using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script is attached to an empty object in the file select scene. This script runs when that scene is accessed.
public class FileSelectButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject scrollViewContent;
    public Text displayText;

    // Start is called before the first frame update
    void Start()
    {
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
            button.GetComponent<Button>().onClick.AddListener(delegate { setFilePathAndDisplayMessage(filePath, reactionName); });
        }
    }

    void setFilePathAndDisplayMessage(string filePath, string reactionName)
	{
        MainSceneScript.filePath = filePath;
        displayText.text = reactionName + " Selected";
    }
}
