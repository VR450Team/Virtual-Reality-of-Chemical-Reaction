using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ParseFileTestScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void start_simulation()
	{
		string filePath1 = "Assets/Resources/test_input.txt";
		char[] atomsArray = getAtomsArray(filePath1);
		printAtomsArray(atomsArray);
		//double[][][] array1 = getArrayFromXyzData(filePath1);
		//Debug.Log(array1[0].Length);
		//print3dArray(array1);
	}

	char[] getAtomsArray(string filePath)
	{
		string[] fileLines = File.ReadAllLines(filePath);
		int numberOfAtoms = int.Parse(fileLines[0]);
		char[] atomsArray = new char[numberOfAtoms];
		int insertionIndex, currentLineIndex, lastLineIndex = numberOfAtoms + 1;

		for (currentLineIndex = 2, insertionIndex = 0; currentLineIndex <= lastLineIndex;
			currentLineIndex++, insertionIndex++)
			atomsArray[insertionIndex] = fileLines[currentLineIndex][1];

		return atomsArray;
	}

	void printAtomsArray(char[] atomsArray)
	{
		int length = atomsArray.Length, i;
		for (i = 0; i < length; i++)
			Debug.Log(atomsArray[i]);
	}

	double[][][] getArrayFromXyzData(string filePath)
	{
		string[] fileLines = File.ReadAllLines(filePath);
		string currentCoord;

		// Array iterators
		int atomIter, frameIter, coordIter;

		int currentLine, lineLength, lineIter;
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

		double[][][] the3dArray = new double[numberOfAtoms][][];

		// Initialize all internal arrays
		for (atomIter = 0; atomIter < numberOfAtoms; atomIter++)
		{
			// Innermost array needs 3 elements for x, y, and z coordinates
			the3dArray[atomIter] = new double[numberOfFrames][];
			for (frameIter = 0; frameIter < numberOfFrames; frameIter++)
				the3dArray[atomIter][frameIter] = new double[3];
		}

		// Start the current line at 2 to skip the first 2 comment lines and increment it by 2
		// to skip the comment lines between frames
		for (frameIter = 0, currentLine = 2; frameIter < numberOfFrames; frameIter++, currentLine += 2)
		{
			for (atomIter = 0, coordIter = 0, currentCoord = ""; atomIter < numberOfAtoms; atomIter++,
			currentLine++, coordIter = 0)
			{
				lineLength = fileLines[currentLine].Length;

				// The line iterator starts at 7 because that is the first character that
				// is part of a coordinate on every line
				for (lineIter = 7; lineIter < lineLength; lineIter++)
				{
					currentChar = fileLines[currentLine][lineIter];
					if (currentChar != ' ')
						currentCoord += currentChar;
					else if (currentCoord != "")
					{
						the3dArray[atomIter][frameIter][coordIter] = Convert.ToDouble(currentCoord);
						currentCoord = "";
						coordIter++;
					}
				}
				// After the for loop above gets done, current_coord will have the value of
				// the last coordinate.
				the3dArray[atomIter][frameIter][coordIter] = Convert.ToDouble(currentCoord);
				currentCoord = "";
			}
		}
		return the3dArray;
	}

	void print3dArray(double[][][] a3dArray)
	{
		// Array iterators
		int atomIter, frameIter;

		double xCoord, yCoord, zCoord;
		int numberOfAtoms = a3dArray.Length;
		int numberOfFrames = a3dArray[0].Length;

		for (atomIter = 0; atomIter < numberOfAtoms; atomIter++)
		{
			Debug.Log("Now looking at atom " + atomIter);

			for (frameIter = 0; frameIter < numberOfFrames; frameIter++)
			{
				Debug.Log("Now looking at frame " + frameIter);

				xCoord = a3dArray[atomIter][frameIter][0];
				yCoord = a3dArray[atomIter][frameIter][1];
				zCoord = a3dArray[atomIter][frameIter][2];

				Debug.Log("The x coord is " + xCoord + ", the y coord is " + yCoord + " and the z coord is " + zCoord);
			}

			Debug.Log(' ');

		}
	}
}
