using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FileSelectButtonActions : MonoBehaviour
{
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        MainSceneScript.filePath = "nothing";

        GameObject button;
        Vector3 buttonPosition = new Vector3(0, 0, 0);
        Vector3 buttonSize = new Vector3(2, 2, 0);
        foreach (string filePath in System.IO.Directory.GetFiles("Assets/Files/"))
        {
            // Some files will be .meta files so ignore those
            if (filePath.Substring(filePath.Length - 4) == ".xyz")
            {
                //Debug.Log(filePath);
                button = Instantiate(buttonPrefab, this.transform) as GameObject;
                button.transform.localPosition = buttonPosition;
                button.transform.localScale = buttonSize;

                // The first 13 characters are "Assets/Files/" and we don't want that in the button's text.
                // The second parameter is the length of the substring, and we need to set it to filePath.Length
                // - 17 so we the ".xyz" at the end isn't included.
                button.GetComponentInChildren<Text>().text = filePath.Substring(13, filePath.Length - 17);
                button.GetComponent<Button>().onClick.AddListener(delegate { setFilePath(filePath); });
                buttonPosition.y -= 50;
            }
        }

    }

    public void setFilePath(string filePath)
	{
        MainSceneScript.filePath = filePath;
	}
}
