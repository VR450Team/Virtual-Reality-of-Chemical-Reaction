using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavMenuButtonActions : MonoBehaviour
{
	public Text buttonText;
	public Button pauseAndPlayButton;
	public Image playImage, pauseRect1, pauseRect2;

	Vector3 horizontalRotation = new Vector3(0, 1, 0);
	Vector3 verticalRotation = new Vector3(1, 0, 0);

	// Update function is called every frame update. All that needs to be done is check if the user
	// has pressed any of the keys and call the corresponding function.
	public void Update()
	{
		if (Input.GetKeyDown("f"))
			goToReactionSelectScreen();

		if (Input.GetKeyDown("r"))
			restartReaction();

		if (Input.GetKeyDown("space"))
			pauseOrPlayReaction();

		if (Input.GetKeyDown("e"))
			goToMainMenu();

		if (Input.GetKeyDown(KeyCode.LeftArrow))
			rotateLeft();

		if (Input.GetKeyDown(KeyCode.RightArrow))
			rotateRight();

		if (Input.GetKeyDown(KeyCode.UpArrow))
			rotateUp();

		if (Input.GetKeyDown(KeyCode.DownArrow))
			rotateDown();

		if (Input.GetKeyDown("i"))
			zoomIn();

		if (Input.GetKeyDown("o"))
			zoomOut();
	}

	// This function is mentioned in section 3.2.3.5.1.10a of the SDD
	public void goToReactionSelectScreen()
	{
		MainSceneScript.playing = false;
		SceneManager.LoadScene("FileSelect");
	}

	// This function is mentioned in section 3.2.3.5.1.11a of the SDD
	public void goToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	// This function is mentioned in sections 3.2.3.5.1.9a and 3.2.3.5.1.13c of the SDD
	public void restartReaction()
	{
		if (!MainSceneScript.playing)
		{
			MainSceneScript.playing = true;
			playImage.enabled = false;
			pauseRect1.enabled = true;
			pauseRect2.enabled = true;
		}
		MainSceneScript.frame = 0;
	}

	// This function is mentioned in sections 3.2.3.5.1.8a and 3.2.3.5.1.10c of the SDD
	public void pauseOrPlayReaction()
	{
		if (MainSceneScript.playing)
		{
			MainSceneScript.playing = false;
			pauseRect1.enabled = false;
			pauseRect2.enabled = false;
			playImage.enabled = true;
		} else
		{
			if (MainSceneScript.frame < MainSceneScript.numberOfFrames)
			{
				MainSceneScript.playing = true;
				playImage.enabled = false;
				pauseRect1.enabled = true;
				pauseRect2.enabled = true;
			}
		}
	}

	// All of the following functions relate to section 3.2.3.5.1.6b of the SDD, which goes over updates of the main camera

	// The rotation functions are mentioned in sections 3.2.3.5.1.12a and 3.2.3.5.1.11c of the SDD
	public void rotateLeft()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, horizontalRotation, 20);
	}

	public void rotateRight()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, horizontalRotation, -20);
	}

	public void rotateUp()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, verticalRotation, 20);
	}

	public void rotateDown()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, verticalRotation, -20);
	}

	// The zoom in and zoom out functions are mentioned in sections 3.2.3.5.1.13a and 3.2.3.5.1.12c of the SDD
	public void zoomIn()
	{
        Camera.main.fieldOfView -= 4;
    }

	public void zoomOut()
	{
        Camera.main.fieldOfView += 4;
    }
}
