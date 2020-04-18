using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestScrollViewScript : MonoBehaviour
{
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        List<string> filePaths = new List<string>();
        foreach (string filePath in System.IO.Directory.GetFiles("Assets/Files/"))
        {
            if (filePath.Substring(filePath.Length - 4) == ".xyz")
                filePaths.Add(filePath);
        }

        // A button has a height of 30 by default so make button container be the height
        // of (number of files * 40) to have spacing of 10 between buttons.
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(1, filePaths.Count * 40);
        Vector3 buttonPosition = new Vector3(0, -20, 0);

        GameObject button;
        foreach (string filePath in filePaths)
		{
            button = Instantiate(buttonPrefab, this.transform) as GameObject;
            button.transform.localPosition = buttonPosition;
            buttonPosition.y -= 40;
		}

        /*GameObject button;
		Vector3 buttonPosition = new Vector3(0, 0, 0);
        Vector2 buttonContainerDimensions = new Vector2(1, 0);
        for (int i = 0; i < 5; i++)
        {
            buttonContainerDimensions.y += 60;
            this.GetComponent<RectTransform>().sizeDelta = buttonContainerDimensions;
            button = Instantiate(buttonPrefab, this.transform) as GameObject;
            button.transform.localPosition = buttonPosition;
            buttonPosition.y -= 60;
        }*/

    }

    void goToMainScene(string filePath)
    {
        MainSceneScript.filePath = filePath;
        SceneManager.LoadScene("MainScene");
    }
}
