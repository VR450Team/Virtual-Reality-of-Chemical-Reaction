using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to the Script object in MainScene
public class MainSceneScript : MonoBehaviour
{
	// These objects are modified by going to the object hierarchy in MainScene, click on the Script object, 
	// look at the Unity inspector, look at the Main Scene Script (Script) section, and drag and drop objects 
	// from the Project section at the bottom to the spots on the inspector.
	public GameObject hydrogenPrefab, carbonPrefab, oxygenPrefab, fluorinePrefab, brominePrefab, bondPrefab;

	int numberOfFrames;

	// The following variables can be accessed from any script using MainSceneScript.variableName.
	// These are mentioned in section 2.2 of the SDD.
	public static int frame;
	public static string filePath;
	public static bool playing;
	public static Vector3 reactionCenterPoint;

	// Start is called when the user presses the launch button on the file select scene, which causes 
	// the scene to be switched to MainScene
	void Start()
	{
		// The data tuple is mentioned in section 2.3 of the SDD
		Tuple<int, string[], Vector3[][]> data = getDataFromXYZFile(filePath);
		numberOfFrames = data.Item1;
		string[] atomTypes = data.Item2;
		Vector3[][] coords3dArray = data.Item3;

		reactionCenterPoint = getReactionCenterPoint(coords3dArray);

		// The bondsDictList list is mentioned in section 2.3 of the SDD.
		List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> bondsDictList = getBonds(atomTypes, coords3dArray);

		instantiateAtoms(atomTypes, coords3dArray);
		instantiateBonds(bondsDictList);

		// frame represents the current frame of the reaction
		frame = 0;
		// playing represents whether the reaction should be playing or paused
		playing = true;
	}

	// Update is called during every frame update
	void Update()
	{
		if (playing)
			frame++;

		// Stop the reaction when the end has been reached
		if (frame == numberOfFrames)
			playing = false;
	}

	Vector3 getReactionCenterPoint(Vector3[][] coords3dArray)
	{
		// Returns the average point of all coordinates in a reaction.
		// Mentioned in 
		int numberOfAtoms = coords3dArray.Length;
		int numberOfFrames = coords3dArray[0].Length;
		int numberOfEachCoord = numberOfAtoms * numberOfFrames;

		float averageXCoord = 0;
		float averageYCoord = 0;
		float averageZCoord = 0;

		int atomIndex, frameIndex;
		for (atomIndex = 0; atomIndex < numberOfAtoms; atomIndex++)
		{
			for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
			{
				averageXCoord += coords3dArray[atomIndex][frameIndex][0];
				averageYCoord += coords3dArray[atomIndex][frameIndex][1];
				averageZCoord += coords3dArray[atomIndex][frameIndex][2];
			}
		}

		averageXCoord /= numberOfEachCoord;
		averageYCoord /= numberOfEachCoord;
		averageZCoord /= numberOfEachCoord;

		return new Vector3(averageXCoord, averageYCoord, averageZCoord);
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

			// There's a script attached to every atom object that dictates it's behavior.
			// There's a variable in each atom script of a two-dimensional array of coordinates.
			// Access that variable and set it to the proper two-dimensional subarray of coordinates 
			// of the three-dimensional array of coordinates.
			atom.GetComponent<AtomScript>().coords2dArray = coords3dArray[i];
		}
	}

	void instantiateBonds(List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> bondsDictList)
	{
		GameObject bond;
		foreach (Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> dict in bondsDictList)
		{
			bond = Instantiate(bondPrefab) as GameObject;

			// There's a script attached to every bond object that dictates it's behavior. 
			// There's a variable in each bond script of a dictionary. Access that variable and 
			// set it to the proper dictionary in bondsDictList.
			bond.GetComponent<BondScript>().bondsDict = dict;
		}
	}

	Tuple<int, string[], Vector3[][]> getDataFromXYZFile(string filePath)
	{
		// Get an array containing all lines of a reaction data file
		string[] fileLines = File.ReadAllLines(filePath);

		// The first line of every frame contains the number of atoms
		int numberOfAtoms = int.Parse(fileLines[0]);

		// A frame takes up a line for each atom along with two comment lines
		int numberOfFrames = fileLines.Length / (numberOfAtoms + 2);

		// First, get atom types
		string[] atomTypes = new string[numberOfAtoms];
		char firstAtomLetter, secondAtomLetter;
		string atomString = "";
		int currentLineIndex, insertionIndex, lastLineIndex;

		HashSet<string> validAtoms = new HashSet<string>() { "H", "C", "O", "F", "Br" };

		// Start currentLineIndex at 2 to skip the first two comment lines. lastLineIndex refers to the
		// last line of the first frame, which contains the last atom type. insertionIndex refers to 
		// which index of the atomTypes array to insert an atom type.
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
				// Error handling features
				Debug.Log("Error: " + atomString + " on line " + (currentLineIndex + 1).ToString() + " of the input file is not" +
					" a valid atom type");
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
		float bondingDistance, distance;
		Vector3 atom1Coords, atom2Coords, position;

		// The 1st and 3rd values of these, or the x and z values, determine thickness and will always be 0.1 
		// to be the right thickness. They 2nd or y value determines length and will change to change length.
		Vector3 scale = new Vector3(0.1f, 0, 0.1f);
		Quaternion rotation;

		// bondReactionData is used to gather the bonds between 2 atoms for an entire reaction. The int key is for the frame
		// in which the 2 atoms are bonded. The values are for the data for the bond object including position, scale, and rotation.
		Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> bondReactionData = new Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>();

		// bondsDictList is a list that will contain dictionaries of bond data. This list gets returned at the end.
		List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>> bondsDictList = new List<Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>>();

		// To find the bonding distance of 2 atoms, add their values of the dictionary below.
		// VDW stands for van der waals. These values are based on VDW radii.
		Dictionary<string, float> VDWValues = new Dictionary<string, float>()
		{
			{"H", 0.6f },
			{"C", 0.85f },
			{"O", 0.76f },
			{"F", 0.735f },
			{"Br", 0.925f }
		};

		// Compare every atom with every other atom. Look at their distances away from each other in every frame. If they are 
		// bonded in any frames, create a dictionary and have the keys be the frames they are bonded in and have the values be 
		// the data that the bond objects will need to appear that it is connecting the 2 atoms. This data includes the position,
		// scale, and rotation. 
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
						// Set position to the position halfway between the 2 atoms
						position = (atom1Coords - atom2Coords) / 2.0f + atom2Coords;
						// scale is a Vector3 and the y attribute is the second value of it. This affects the length of the bond object.
						scale.y = distance * 0.6f;
						// Set the rotation so that one end of the bond is at one atom and the other end is at the other atom
						rotation = Quaternion.FromToRotation(Vector3.up, atom1Coords - atom2Coords);

						bondReactionData.Add(frameIndex, new Tuple<Vector3, Vector3, Quaternion>(position, scale, rotation));
					}
				}

				// This is true when bonds were found between 2 atoms.
				if (bondReactionData.Count > 0)
				{
					bondsDictList.Add(bondReactionData);

					// Create a new dictionary since for a list of dictionaries, the dictionaries are stored by reference and not value.
					// Clearing the dictionary would result in bondsDictList containg empty dictionaries at the end.
					bondReactionData = new Dictionary<int, Tuple<Vector3, Vector3, Quaternion>>();
				}
			}
		}
		return bondsDictList;
	}
}