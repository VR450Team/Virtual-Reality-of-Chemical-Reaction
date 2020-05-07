using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMain : MonoBehaviour
{
    // This function is mentioned in sections 3.2.3.5.1.6c
    public void RMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
