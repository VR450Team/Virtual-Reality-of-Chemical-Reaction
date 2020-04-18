using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchButtonScript : MonoBehaviour
{
    public void goToMainScene()
    {
        if (MainSceneScript.filePath != "nothing")
            SceneManager.LoadScene("MainScene");
    }
}
