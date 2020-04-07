using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneScript : MonoBehaviour
{
	// This script is attached to an empty object in TestScene. It runs when you hit the play button in that scene.
    public GameObject TestAtomPrefab;
	public GameObject HydrogenPrefab;
	public GameObject CarbonPrefab;
	public GameObject OxygenPrefab;

    // Start is called before the first frame update
    void Start()
    {
		// vSyncCount makes Unity synchronize with your monitor's refresh rate, setting this to 0
		//  turns that off and this needs to be done if you want to view the reaction at lower frame rate
		//QualitySettings.vSyncCount = 0;
		//Application.targetFrameRate = 2;

		string[] filePaths = new string[] {"Assets/Resources/testInput1.txt", 
			"Assets/Resources/testInput2.txt", "Assets/Resources/xyzexample1.xyz"};
		
		string filePath = filePaths[2];
		char[] atomTypes = getAtomTypes(filePath);
		int numberOfAtoms = atomTypes.Length;
		float[][][] the3dArray = getArrayFromXYZData(filePath);
		//print3dArray(the3dArray);

		GameObject atom;

		for (int i = 0; i < numberOfAtoms; i++)
		{
			if (atomTypes[i] == 'H')
				atom = Instantiate(HydrogenPrefab) as GameObject;
			else if (atomTypes[i] == 'C')
				atom = Instantiate(CarbonPrefab) as GameObject;
			else if (atomTypes[i] == 'O')
				atom = Instantiate(OxygenPrefab) as GameObject;
			else
				return;
			//atom = Instantiate(TestAtomPrefab) as GameObject;
			
			//atom.GetComponent<AtomScript>().setCoords2dArray(the3dArray[i]);
		}
	}

	char[] getAtomTypes(string filePath)
	{
		string[] fileLines = File.ReadAllLines(filePath);
		int numberOfAtoms = int.Parse(fileLines[0]);
		char[] atoms = new char[numberOfAtoms];
		int insertionIndex, currentLineIndex, lastLineIndex = numberOfAtoms + 1;

		for (currentLineIndex = 2, insertionIndex = 0; currentLineIndex <= lastLineIndex;
			currentLineIndex++, insertionIndex++)
			atoms[insertionIndex] = fileLines[currentLineIndex][1];

		return atoms;
	}

	float[][][] getArrayFromXYZData(string filePath)
	{
		string[] fileLines = File.ReadAllLines(filePath);
		string currentCoord;
		int atomIndex, frameIndex, coordIndex, currentLine, lineLength, lineIndex;
		char currentChar;

		// The first line of every frame contains the number of atoms
		int numberOfAtoms = int.Parse(fileLines[0]);

		// A frame takes up a line for each atom along with two comment lines
		int numberOfFrames = fileLines.Length / (numberOfAtoms + 2);

		/* In a normal 3d array in C#, you can only access the elements of the
		* innermost array so if you had a normal3d array and tried to call
		* 3dArray[0], you would get an error since you are not trying to access
		* elements in the innermost array. In order to create a function that allows
		* us to prin a 3d array and possibly other things, I need to access the
		* first element of that array, which is a 2d array, and find the length of
		* it to get the number of atoms. A "jagged" array allows me to do this.
		* The only difference seems to be in how you declare, initialize, and 
		* access elements.
		*/

		float[][][] the3dArray = new float[numberOfAtoms][][];

		// Initialize all internal arrays
		for (atomIndex = 0; atomIndex < numberOfAtoms; atomIndex++)
		{
			// Innermost array needs 3 elements for x, y, and z coordinates
			the3dArray[atomIndex] = new float[numberOfFrames][];
			for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
				the3dArray[atomIndex][frameIndex] = new float[3];
		}

		// Start the current line at 2 to skip the first 2 comment lines and increment it by 2
		// to skip the comment lines between frames
		for (frameIndex = 0, currentLine = 2; frameIndex < numberOfFrames; frameIndex++, currentLine += 2)
		{
			for (atomIndex = 0, coordIndex = 0, currentCoord = ""; atomIndex < numberOfAtoms; atomIndex++,
			currentLine++, coordIndex = 0)
			{
				lineLength = fileLines[currentLine].Length;

				// The line index starts at 7 because that is the first character that
				// is part of a coordinate on every line
				for (lineIndex = 7; lineIndex < lineLength; lineIndex++)
				{
					currentChar = fileLines[currentLine][lineIndex];
					if (currentChar != ' ')
						currentCoord += currentChar;
					else if (currentCoord != "")
					{
						the3dArray[atomIndex][frameIndex][coordIndex] = float.Parse(currentCoord, CultureInfo.InvariantCulture.NumberFormat);
						currentCoord = "";
						coordIndex++;
					}
				}
				// After the for loop above gets done, current_coord will have the value of
				// the last coordinate.
				the3dArray[atomIndex][frameIndex][coordIndex] = float.Parse(currentCoord, CultureInfo.InvariantCulture.NumberFormat);
				currentCoord = "";
			}
		}
		return the3dArray;
	}

	void print3dArray(float[][][] a3dArray)
	{
		// Array iterators
		int atomIndex, frameIndex;

		double xCoord, yCoord, zCoord;
		int numberOfAtoms = a3dArray.Length;
		int numberOfFrames = a3dArray[0].Length;

		for (atomIndex = 0; atomIndex < numberOfAtoms; atomIndex++)
		{
			Debug.Log("Now looking at atom " + atomIndex);

			for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
			{
				Debug.Log("Now looking at frame " + frameIndex);

				xCoord = a3dArray[atomIndex][frameIndex][0];
				yCoord = a3dArray[atomIndex][frameIndex][1];
				zCoord = a3dArray[atomIndex][frameIndex][2];

				Debug.Log("The x coord is " + xCoord + ", the y coord is " + yCoord + " and the z coord is " + zCoord);
			}

			Debug.Log(' ');

		}
	}

}
