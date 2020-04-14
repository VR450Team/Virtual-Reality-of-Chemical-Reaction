using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour{
    public Text buttonText;
    public Text displayText;
    public void DisplayMessage() {
        displayText = GameObject.Find("Canvas/DisplayMessage").GetComponent<Text>();

        displayText.text = "File Selected";
    }

    /*
    public void DisplayMessage1() {
        //buttonText = GameObject.Find("Canvas/ScrollView/Viewport/Content/Rection1/Text").GetComponent<TextMesh>();
        //displayText = GameObject.Find("Canvas/DisplayMessage").GetComponent<Text>();

        //displayText.text = "Selected: " + buttonText.text;
        
    }
    public void DisplayMessage2() {
        buttonText = GameObject.Find("Canvas/Viewport/Content/Rection2/Text").GetComponent<Text>();
        displayText = GameObject.Find("Canvas/DisplayText").GetComponent<Text>();

        displayText.text = "Selected: " + buttonText.text;
    }
    */
}
