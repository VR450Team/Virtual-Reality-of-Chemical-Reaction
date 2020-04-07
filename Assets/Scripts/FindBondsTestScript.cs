using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBondsTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		string[] filePaths = new string[] {"Assets/Resources/testInput1.txt", 
			"Assets/Resources/testInput2.txt", "Assets/Resources/xyzexample1.xyz",
			"Assets/Resources/testInput3.txt"};

		string filePath = filePaths[3];
		char[] atomTypes = getAtomTypes(filePath);
		float[][][] the3dArray = getArrayFromXYZData(filePath);

		findAtomBonds(the3dArray, atomTypes);
    }

	void findAtomBonds(float[][][] a3dArray, char[] atomTypes)
	{
		int numberOfAtoms = a3dArray.Length;
		int numberOfFrames = a3dArray[0].Length;
		int atom1Index, atom2Index, frameIndex, count = 0;
		char atom1Type, atom2Type;
		float[] atom1Coords, atom2Coords;
		//float atom1XCoord, atom1YCoord, atom1ZCoord, atom2XCoord, atom2YCoord, atom2ZCoord;

		for (atom1Index = 0; atom1Index < numberOfAtoms - 1; atom1Index++)
		{
			atom1Type = atomTypes[atom1Index];
			for (atom2Index = atom1Index + 1; atom2Index < numberOfAtoms; atom2Index++)
			{
				atom2Type = atomTypes[atom2Index];

				//Debug.Log("Checking if atom " + atom1Index + " and atom " + atom2Index + " are bonded at all frames");
				//Debug.Log("Atom " + atom1Index + " is a " + atom1Type + " and atom " + atom2Index + " is a " + atom2Type);

				for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
				{
					//Debug.Log("Checking frame " + frameIndex);
						
					atom1Coords = a3dArray[atom1Index][frameIndex];
					atom2Coords = a3dArray[atom2Index][frameIndex];

					/*atom1XCoord = atom1Coords[0];
					atom1YCoord = atom1Coords[1];
					atom1ZCoord = atom1Coords[2];
					atom2XCoord = atom2Coords[0];
					atom2YCoord = atom2Coords[1];
					atom2ZCoord = atom2Coords[2];*/

					//Debug.Log("Atom " + atom1Index + "'s coords are " + atom1XCoord + ", " + atom1YCoord + ", and " + atom1ZCoord);
					//Debug.Log("Atom " + atom2Index + "'s coords are " + atom2XCoord + ", " + atom2YCoord + ", and " + atom2ZCoord);

					if (isBonded(atom1Coords, atom1Type, atom2Coords, atom2Type))
					{
						count++;
						//Debug.Log("Atoms are bonded");
					}
					//else
						//Debug.Log("Atoms are not bonded");
				}
			}
		}
		Debug.Log("Found " + count + " bonds");
	}

	bool isBonded(float[] coord_arr_1, char atom_type_1, float[] coord_arr_2, char atom_type_2){
		//Function checks if 2 given atoms are bonded and returns true if so
		//An atom is bonded if the distance between two atoms is less than the combined values of
		//there Van der Waal's radii, as approved by our share holder (values given below)
		float h_van_r = 120;	//120 picometers
		float c_van_r = 170; 	//170 picometers
		float o_van_r = 152;	//152 picometers
		float dist = 0.0f;
		bool is_bonded = false;
		float van_dist = 0;

		//float dist = Vector3.Distance(coord_arr_1.position, coord_arr_1.position);		

		//float comp_dist;
		//dist = square_root((x2 - x1)^2 + (y2 - y1)^2 + (z2 - z1)^2)
		dist = ((coord_arr_2[0] - coord_arr_1[0]) * (coord_arr_2[0] - coord_arr_1[0]))
			+ ((coord_arr_2[1] - coord_arr_1[1]) * (coord_arr_2[1] - coord_arr_1[1]))
			+ ((coord_arr_2[2] - coord_arr_1[2]) * (coord_arr_2[2] - coord_arr_1[2]));
		dist = Mathf.Sqrt(dist);

		if(atom_type_1 == 'H'){
			van_dist += h_van_r;
		}
		else if(atom_type_1 == 'C'){
			van_dist += c_van_r;
		}
		else if(atom_type_1 == 'O'){
			van_dist += o_van_r;
		}
		else if(atom_type_2 == 'H'){
			van_dist += h_van_r;
		}
		else if(atom_type_2 == 'C'){
			van_dist += c_van_r;
		}
		else if(atom_type_2== 'O'){
			van_dist += o_van_r;
		}

		if(van_dist >= dist){
			//If distance less than Van der Waal radii atoms are bonded
			is_bonded = true;
		}
		return(is_bonded);
	}

	char[] getAtomTypes(string filePath) {
		string[] fileLines = File.ReadAllLines(filePath);
		int numberOfAtoms = int.Parse(fileLines[0]);
		char[] atoms = new char[numberOfAtoms];
		int insertionIndex, currentLineIndex, lastLineIndex = numberOfAtoms + 1;

		for (currentLineIndex = 2, insertionIndex = 0; currentLineIndex <= lastLineIndex;
			currentLineIndex++, insertionIndex++)
			atoms[insertionIndex] = fileLines[currentLineIndex][1];

		return atoms;
	}

	float[][][] getArrayFromXYZData(string filePath) {
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
		for (atomIndex = 0; atomIndex < numberOfAtoms; atomIndex++) {
			// Innermost array needs 3 elements for x, y, and z coordinates
			the3dArray[atomIndex] = new float[numberOfFrames][];
			for (frameIndex = 0; frameIndex < numberOfFrames; frameIndex++)
				the3dArray[atomIndex][frameIndex] = new float[3];
		}

		// Start the current line at 2 to skip the first 2 comment lines and increment it by 2
		// to skip the comment lines between frames
		for (frameIndex = 0, currentLine = 2; frameIndex < numberOfFrames; frameIndex++, currentLine += 2) {
			for (atomIndex = 0, coordIndex = 0, currentCoord = ""; atomIndex < numberOfAtoms; atomIndex++,
				currentLine++, coordIndex = 0) {
				lineLength = fileLines[currentLine].Length;

				// The line index starts at 7 because that is the first character that
				// is part of a coordinate on every line
				for (lineIndex = 7; lineIndex < lineLength; lineIndex++) {
					currentChar = fileLines[currentLine][lineIndex];
					if (currentChar != ' ')
						currentCoord += currentChar;
					else if (currentCoord != "") {
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
}
