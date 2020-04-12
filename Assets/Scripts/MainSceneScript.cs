// This script is attached to an empty object in MainScene. It runs when you hit the play button in that scene.
using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
	// This variable is available to all atoms and covalent bonds by using Global.frame
	public static int frame;
	public static bool playing;
}

public class MainSceneScript : MonoBehaviour
{
	public GameObject hydrogenPrefab, carbonPrefab, oxygenPrefab, fluorinePrefab, brominePrefab, covalentBondPrefab;

	int numberOfFrames;
	string debugString; // I plan on using this to use string interpolation in a Debug.Log

	// Start is called before the first frame update
	void Start()
	{
		// vSyncCount makes Unity synchronize with your monitor's refresh rate, setting this to 0
		// turns that off and this needs to be done if you want to view the reaction at lower frame rate
		//QualitySettings.vSyncCount = 0;
		//Application.targetFrameRate = 90;

		// Initialize global variables
		Global.frame = 0;
		Global.playing = true;
		
		string[] filePaths = {"Assets/Resources/testInput1.txt",
			"Assets/Resources/testInput2.txt", "Assets/Resources/testInput3.txt", 
			"Assets/Resources/testInput4.txt", "Assets/Resources/officialReaction1.xyz",
			"Assets/Resources/officialReaction2.xyz"};
		string filePath = filePaths[4];

		Tuple<int, string[], Vector3[][]> data = getDataFromXYZFile(filePath);
		numberOfFrames = data.Item1;
		string[] atomTypes = data.Item2;
		Vector3[][] coords3dArray = data.Item3;
		List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> bondsDictList = getBonds(atomTypes, coords3dArray);

		instantiateAtoms(atomTypes, coords3dArray);
		instantiateBonds(bondsDictList);
	}

	void Update()
	{
		if (Global.playing)
			Global.frame++;

		if (Global.frame == numberOfFrames)
			Global.playing = false;
	}

	void instantiateAtoms(string[] atomTypes, Vector3[][] coords3dArray)
	{
		GameObject atom;
		Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>()
		{
			{"H", hydrogenPrefab },
			{"C", carbonPrefab },
			{"O", oxygenPrefab },
			{"F", fluorinePrefab },
			{"Br", brominePrefab }
		};

		for (int i = 0; i < atomTypes.Length; i++)
		{
			atom = Instantiate(prefabsDict[atomTypes[i]]) as GameObject;

			// Give the atom it's element of coords3dArray
			atom.GetComponent<AtomScript>().setCoords2dArray(coords3dArray[i]);
		}
	}

	void instantiateBonds(List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> bondsDictList)
	{
		GameObject covalentBond;
		foreach (Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> dict in bondsDictList)
		{
			covalentBond = Instantiate(covalentBondPrefab) as GameObject;
			covalentBond.GetComponent<CovalentBondScript>().setBondsDict(dict);
		}
	}

	Tuple<int, string[], Vector3[][]> getDataFromXYZFile(string filePath)
	{
		string[] fileLines = File.ReadAllLines(filePath);

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
				debugString = $"Error: {atomString} on line {currentLineIndex + 1} of the input file is not a valid atom type";
				Debug.Log(debugString);
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

		// Return numberOfFrames, atomTypes, and coords3dArray
		return new Tuple<int, string[], Vector3[][]>(numberOfFrames, atomTypes, coords3dArray);
	}

	List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> getBonds(string[] atomTypes, Vector3[][] coords3dArray)
	{
		int numberOfAtoms = coords3dArray.Length;
		int numberOfFrames = coords3dArray[0].Length;
		int atom1Index, atom2Index, frameIndex;
		string atom1Type, atom2Type;
		Vector3 atom1Coords, atom2Coords, position;
		Vector3 scale = new Vector3(0.1f, 0, 0.1f);
		Quaternion rotation;
		List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> bondsDictList = new List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>>();
		Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> bondFrameData = new Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>();
		float bondingDistance, distance;

		Dictionary<string, float> VDWValues = new Dictionary<string, float>()
		{
			{"H", 0.6f },
			{"C", 0.85f },
			{"O", 0.76f },
			{"F", 0.735f },
			{"Br", 0.925f }
		};

		for (atom1Index = 0; atom1Index < numberOfAtoms - 1; atom1Index++)
		{
			atom1Type = atomTypes[atom1Index];
			for (atom2Index = atom1Index + 1; atom2Index < numberOfAtoms; atom2Index++)
			{
				atom2Type = atomTypes[atom2Index];

				bondingDistance = VDWValues[atom1Type] + VDWValues[atom2Type];

				for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
				{
					atom1Coords = coords3dArray[atom1Index][frameIndex];
					atom2Coords = coords3dArray[atom2Index][frameIndex];
					distance = Vector3.Distance(atom1Coords, atom2Coords);

					if (distance < bondingDistance)
					{
						position = (atom1Coords - atom2Coords) / 2.0f + atom2Coords;
						scale.y = distance * 0.6f;
						rotation = Quaternion.FromToRotation(Vector3.up, atom1Coords - atom2Coords);

						bondFrameData.Add(frameIndex, new Tuple<Vector3, Vector3, Quaternion>(position, scale, rotation));
					}
				}

				if (bondFrameData.Count > 0)
				{
					bondsDictList.Add(bondFrameData);
					bondFrameData = new Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>();
				}
			}
		}
		return bondsDictList;
	}

	bool isBonded(Vector3 coord_arr_1, char atom_type_1, Vector3 coord_arr_2, char atom_type_2)
	{
		//Function checks if 2 given atoms are bonded and returns true if so
		//An atom is bonded if the distance between two atoms is less than the combined values of
		//there Van der Waal's radii, as approved by our share holder (values given below)
		float h_van_r = 1.2f;   //120 picometers. I think this is 1.2 unity units
		float c_van_r = 1.7f;   //170 picometers
		float o_van_r = 1.52f;  //152 picometers
		float dist = 0.0f;
		bool is_bonded = false;
		float van_dist = 0;

		//float dist = Vector3.Distance(coord_arr_1.position, coord_arr_1.position);		

		//float comp_dist;
		//dist = square_root((x2 - x1)^2 + (y2 - y1)^2 + (z2 - z1)^2)
		/*dist = ((coord_arr_2[0] - coord_arr_1[0]) * (coord_arr_2[0] - coord_arr_1[0]))
						+ ((coord_arr_2[1] - coord_arr_1[1]) * (coord_arr_2[1] - coord_arr_1[1]))
						+ ((coord_arr_2[2] - coord_arr_1[2]) * (coord_arr_2[2] - coord_arr_1[2]));
		dist = Mathf.Sqrt(dist);*/
		dist = Vector3.Distance(coord_arr_1, coord_arr_2);

		if (atom_type_1 == 'H')
			van_dist += h_van_r;
		else if (atom_type_1 == 'C')
			van_dist += c_van_r;
		else if (atom_type_1 == 'O')
			van_dist += o_van_r;

		if (atom_type_2 == 'H')
			van_dist += h_van_r;
		else if (atom_type_2 == 'C')
			van_dist += c_van_r;
		else if (atom_type_2 == 'O')
			van_dist += o_van_r;

		if (van_dist >= dist)
		{
			//If distance less than Van der Waal radii atoms are bonded
			is_bonded = true;
		}
		return (is_bonded);
	}

	void testBondsDictList(List<Dictionary<int, float[][]>> bondsDictList)
	{
		Dictionary<int, float[][]> currentDict;
		float[] atom1Coords, atom2Coords;

		for (int dictIndex = 0; dictIndex < bondsDictList.Count; dictIndex++)
		{
			currentDict = bondsDictList[dictIndex];
			Debug.Log("Now looking at dictionary " + dictIndex);

			foreach (KeyValuePair<int, float[][]> entry in currentDict)
			{
				Debug.Log("Key (frame): " + entry.Key);
				Debug.Log("Values:");
				atom1Coords = entry.Value[0];
				atom2Coords = entry.Value[1];
				Debug.Log("The 1st atoms coords are");
				foreach (float coord in atom1Coords)
					Debug.Log(coord);
				Debug.Log("The 2nd atoms coords are");
				foreach (float coord in atom2Coords)
					Debug.Log(coord);
			}
		}
	}

	void print3dArray(Vector3[][] a3dArray)
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