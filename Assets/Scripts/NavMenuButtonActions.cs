﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavMenuButtonActions : MonoBehaviour
{
	public Text buttonText;
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

	public void rotateLeft()
	{
		Camera.main.transform.RotateAround(Global.reactionCenterPoint, leftRotation, 20);
	}

	public void rotateRight()
	{
		Camera.main.transform.RotateAround(Global.reactionCenterPoint, rightRotation, 20);
	}

	public void rotateUp()
	{
		Camera.main.transform.RotateAround(Global.reactionCenterPoint, upRotation, 20);
	}

	public void rotateDown()
	{
		Camera.main.transform.RotateAround(Global.reactionCenterPoint, downRotation, 20);
	}

	public void zoomIn()
	{

	}

	public void zoomOut()
	{

	}
}