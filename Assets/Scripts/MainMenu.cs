using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public void PlayGame(){
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
        SceneManager.LoadScene("FileSelect");
    }
}

