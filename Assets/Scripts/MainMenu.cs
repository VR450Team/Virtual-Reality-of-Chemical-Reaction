using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public bool isStart;
    public bool isQuit;
    public void PlayGame(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(2);
    }

    public void QuitGame(){
		Debug.Log("Has quit game");
        Application.Quit();
    }
    
    /*void OnMouseUp() {
        if(isStart){
            SceneManager.LoadScene(2);
        }
        if(isQuit){
            SceneManager.LoadScene(0);
        }

    }*/
    public void FileSelect(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        SceneManager.LoadScene(3);
    }
}

