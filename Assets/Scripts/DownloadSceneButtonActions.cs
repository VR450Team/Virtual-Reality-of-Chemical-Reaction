using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Globalization;
using UnityEngine.SceneManagement;

// This script is attached to the Script object in the Download scene
public class DownloadSceneButtonActions : MonoBehaviour
{
	// This Text object is modified by going to the object hierarchy in the Download scene, click on the Canvas object, 
	// click on the Buttons Action Script object, look at the Unity inspector, look at the Download Scene Button Actions
	// Script (Script) section, and drag and drop objects from the Project section at the bottom to the spots on the inspector.
	public Text inputFieldText;
    public Text displayText;

	// The following code, along with the commented out code in the downloadFile function provides a way for the program to 
	// check if the user tries to request a file from the server that they already have a copy of. 
	/*HashSet<string> filesInFilesFolder;
	public void Start()
	{
		filesInFilesFolder = new HashSet<string>();
		foreach (string filePath in System.IO.Directory.GetFiles("Assets/Files/"))
		{
			if (filePath.Substring(filePath.Length - 4) == ".txt")
				filesInFilesFolder.Add(filePath.Substring(13, filePath.Length - 13));
		}
		//foreach (string file in filesInFilesFolder)
			//Debug.Log(file);
	}*/

	// This function mentioned in sections 3.2.3.5.1.5a and 3.2.3.5.1.5c of the SDD
	public void goToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void startDownload()
	{
		string fileName = inputFieldText.text;

		/*if (filesInFilesFolder.Contains(fileName))
		{
			Debug.Log("You already have the file you requested");
		}
		else
		{
			StartCoroutine(downloadFile(fileName));
		}*/

		StartCoroutine(downloadFile(fileName));
	}

	// The algorithm in downloadFile is mentioned in sections 3.2.3.5.1.4a and 3.2.3.5.1.4c of the SDD
	IEnumerator downloadFile(string fileName)
	{
		// Dr. Iqbal's file path for his web server
		string filePathBeginning = "http://people.missouristate.edu/riqbal/data/";
		string url = filePathBeginning + fileName;

		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				Debug.Log("Error: " + webRequest.error);
				displayText.text = fileName + " failed to pull";
			}
			else
			{
				string data = webRequest.downloadHandler.text;
				Debug.Log("Received data and checking it");
				Debug.Log("Received " + data);
				try
				{
					if (fileIsGood(data))
					{
						File.WriteAllText("Assets/Files/" + fileName, data);
						Debug.Log("Just added file to files folder");
                        displayText.text = fileName + " pulled from server";
					}
					else
					{
						Debug.Log("File not valid because of atom types");
						displayText.text = fileName + " failed to pull";
						// Error handling features
					}
				}
				catch
				{
					Debug.Log("File not valid");
					displayText.text = fileName + " failed to pull";
					// Error handling features
				}
			}
		}

		/*
		//var uwr = new UnityWebRequest("http://people.missouristate.edu/riqbal/data/mosgcone.txt", UnityWebRequest.kHttpVerbGET);

		//Update with the proper file type or file name
		//Stores to the location C:\Users\(YourUserName)\AppData\LocalLow\DefaultCompany\Virtual Reality of Chemical Reactions
		/*string path = Path.Combine(Application.persistentDataPath, "ReactionFile");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
        {
            Debug.Log("File successfully downloaded and saved to " + path);
        }*/
	}

	bool fileIsGood(string fileContents)
	{
		// Should return true if there are no errors while reading the file.
		// This function is almost the same as the file reading function from MainSceneScript.

		string[] fileLines = fileContents.Split('\n');

		// The first line of every frame contains the number of atoms
		int numberOfAtoms = int.Parse(fileLines[0]);

		// A frame takes up a line for each atom along with two comment lines
		int numberOfFrames = fileLines.Length / (numberOfAtoms + 2);

		// 1st, get atom types
		string[] atomTypes = new string[numberOfAtoms];
		char firstAtomLetter, secondAtomLetter;
		string atomString = "";
		int currentLineIndex, insertionIndex, lastLineIndex;

		HashSet<string> validAtoms = new HashSet<string>() { "H", "C", "O", "F", "Br" };

		for (currentLineIndex = 2, insertionIndex = 0, lastLineIndex = numberOfAtoms + 1;
			currentLineIndex <= lastLineIndex; currentLineIndex++, insertionIndex++)
		{
			firstAtomLetter = fileLines[currentLineIndex][1];
			secondAtomLetter = fileLines[currentLineIndex][2];

			atomString += firstAtomLetter;
			if (secondAtomLetter != ' ')
				atomString += secondAtomLetter;

			if (!validAtoms.Contains(atomString))
			{
				// Error
				Debug.Log("Error: " + atomString + " on line " + (currentLineIndex + 1).ToString() + " of the input file is not a " +
					"valid atom type");
				return false;
			}

			atomTypes[insertionIndex] = atomString;
			atomString = "";
		}


		// Next, get the 3d array

		string currentCoord = "";
		int atomIndex, frameIndex, coordIndex, lineLength, lineCharIndex;
		char currentChar;

		// Use a jagged 3d array so we can access the 2d array elements inside of it.
		// In a normal 3d array in C#, you can only access the elements of the innermost array.
		Vector3[][] coords3dArray = new Vector3[numberOfAtoms][];

		// Initialize all internal arrays
		for (atomIndex = 0; atomIndex < numberOfAtoms; atomIndex++)
		{
			coords3dArray[atomIndex] = new Vector3[numberOfFrames];
			for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
				coords3dArray[atomIndex][frameIndex] = new Vector3();
		}

		// Start the current line at 2 to skip the first 2 comment lines and increment it by 2
		// to skip the comment lines between frames
		for (frameIndex = 0, currentLineIndex = 2; frameIndex < numberOfFrames; frameIndex++, currentLineIndex += 2)
		{
			for (atomIndex = 0, coordIndex = 0; atomIndex < numberOfAtoms; atomIndex++, currentLineIndex++)
			{
				lineLength = fileLines[currentLineIndex].Length;

				// The line char index starts at 7 because that is the first character that
				// is part of a coordinate on every line
				for (lineCharIndex = 7; lineCharIndex < lineLength; lineCharIndex++)
				{
					currentChar = fileLines[currentLineIndex][lineCharIndex];
					if (currentChar != ' ')
						currentCoord += currentChar;
					else if (currentCoord != "")
					{
						coords3dArray[atomIndex][frameIndex][coordIndex] = float.Parse(currentCoord, CultureInfo.InvariantCulture.NumberFormat);
						currentCoord = "";
						coordIndex++;
					}
				}
				// After the for loop above gets done, currentCoord will have the value of the last coordinate.
				coords3dArray[atomIndex][frameIndex][coordIndex] = float.Parse(currentCoord, CultureInfo.InvariantCulture.NumberFormat);
				currentCoord = "";
				coordIndex = 0;
			}
		}
		return true;
	}
}

