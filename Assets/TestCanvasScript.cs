using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestCanvasScript : MonoBehaviour
{
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject button;
        Vector3 buttonPosition = new Vector3(-150, 0, 0);
        foreach(string filePath in System.IO.Directory.GetFiles("Assets/Resources/"))
		{
            if (filePath.Substring(filePath.Length - 4) == ".xyz")
            {
                Debug.Log(filePath);
                button = Instantiate(buttonPrefab, this.transform) as GameObject;
                button.transform.localPosition = buttonPosition;

                // The first 17 characters are "Assets/Resources/" and we don't want that in the button's text.
                // The second parameter is the length of the substring, and we need to set it to filePath.Length
                // - 21 so we the ".xyz" at the end isn't included.
                button.GetComponentInChildren<Text>().text = filePath.Substring(17, filePath.Length - 21);
                button.GetComponent<Button>().onClick.AddListener(delegate { goToMainScene(filePath); });
                buttonPosition.x += 150;
            }
		}
        
    }

    void goToMainScene(string filePath)
	{
        MainSceneScript.filePath = filePath;
        SceneManager.LoadScene("MainScene");
	}
}
