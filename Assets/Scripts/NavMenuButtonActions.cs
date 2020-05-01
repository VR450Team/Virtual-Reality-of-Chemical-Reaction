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
	float distanceFromCenterPoint;
	Vector3 leftRotation = new Vector3(0, 1, 0);
	Vector3 rightRotation = new Vector3(0, -1, 0);
	Vector3 upRotation = new Vector3(1, 0, 0);
	Vector3 downRotation = new Vector3(-1, 0, 0);

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

	public void goToReactionSelectScreen()
	{
		MainSceneScript.playing = false;
		SceneManager.LoadScene("FileSelect");
	}

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

	public void pauseOrPlayReaction()
	{
		if (MainSceneScript.playing)
		{
			//buttonText.text = "Play";
			MainSceneScript.playing = false;
			pauseRect1.enabled = false;
			pauseRect2.enabled = false;
			playImage.enabled = true;
			//pauseRect1.image.setActive(false);
			//pauseRect2.image.setActive(false);
			//Image img = pauseAndPlayButton.GetComponentInChildren<PlayImage>();
			//pauseAndPlayButton.GetComponentInChildren<PauseImage>().GetComponentInChildren<Rectangle1>().setActive(false);
			//pauseAndPlayButton.GetComponentInChildren<PauseImage>().GetComponentInChildren<Rectangle2>().setActive(false);
			//pauseAndPlayButton.GetComponentInChildren<PlayImage>().setActive(true);
		} else
		{
			//buttonText.text = "Pause";
			MainSceneScript.playing = true;
			playImage.enabled = false;
			pauseRect1.enabled = true;
			pauseRect2.enabled = true;
			//pauseAndPlayButton.GetComponentInChildren<PlayImage>().setActive(false);
			//pauseAndPlayButton.GetComponentInChildren<PauseImage>().GetComponentInChildren<Rectangle1>().setActive(true);
			//pauseAndPlayButton.GetComponentInChildren<PauseImage>().GetComponentInChildren<Rectangle2>().setActive(true);
		}
	}

	public void exitApplication()
	{
		Debug.Log("Application quit. If you're running this in the editor then nothing will happen.");
		Application.Quit();
	}

	public void rotateLeft()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, leftRotation, 20);
	}

	public void rotateRight()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, rightRotation, 20);
	}

	public void rotateUp()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, upRotation, 20);
	}

	public void rotateDown()
	{
		Camera.main.transform.RotateAround(MainSceneScript.reactionCenterPoint, downRotation, 20);
	}

	public void zoomIn()
	{
        Camera.main.fieldOfView -= 4;
    }

	public void zoomOut()
	{
        Camera.main.fieldOfView += 4;
    }
}
