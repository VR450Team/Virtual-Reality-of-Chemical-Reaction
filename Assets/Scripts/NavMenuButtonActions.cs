using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavMenuButtonActions : MonoBehaviour
{
	public Text buttonText;
	
	public void Update()
	{
		if (Input.GetKeyDown("f"))
			goToReactionSelectScreen();

		if (Input.GetKeyDown("r"))
			restartReaction();

		if (Input.GetKeyDown("space"))
			pauseOrPlayReaction();

		if (Input.GetKeyDown("e"))
			exitApplication();

	}

	public void goToReactionSelectScreen()
	{
		Global.playing = false;
		SceneManager.LoadScene("FileSelect");
	}

	public void restartReaction()
	{
		Global.frame = 0;
	}

	public void pauseOrPlayReaction()
	{
		if (Global.playing)
		{
			buttonText.text = "Play";
			Global.playing = false;
		} else
		{
			buttonText.text = "Pause";
			Global.playing = true;
		}
	}

	public void exitApplication()
	{
		Debug.Log("Application quit. If you're running this in the editor then nothing will happen, " +
				"That's what the documentation says.");
		Application.Quit();
	}
}
