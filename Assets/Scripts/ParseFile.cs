using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

namespace xyzparser
{
	public class ParseFile : MonoBehaviour {
		void Start(){
			string file_path1 = "Assets/Resources/test_input.txt";
			double[][][] array1 = get_array_from_xyz_data(file_path1);
			print_3d_array(array1);
		}

		public static double [][][] get_array_from_xyz_data(string file_path){
			string[] file_lines = File.ReadAllLines(file_path);
			string current_coord;
			int frame, current_line, row, column, line_length, line_iterator;
			char current_char, atom;

			// The first line of every frame contains the number of atoms
			int number_of_atoms = int.Parse(file_lines[0]);

			// A frame takes up a line for each atom along with two comment lines
			int number_of_frames = file_lines.Length / (number_of_atoms + 2);

			/* In a normal 3d array in C#, you can only access the elements of the
			* innermost array so if you had a normal3d array and tried to call
			* 3d_array[0], you would get an error since you are not trying to access
			* elements in the innermost array. In order to create a function that allows
			* us to prin a 3d array and possibly other things, I need to avvess the
			* first element of that array, which is a 2d array, and find the length of
			* it to get the number of atoms. A "jagged" array allows me to do this.
			* The only difference seems to be in how you declare, initialize, and 
			* access elements.
			* 
			* The innermost array will have doubles since the coordinates will be 
			* floating point numbers. For the atom type, we will have a 0 represent
			* oxygen, 1 represents hydrogen, and 2 represents carbon. */

			double [][][] the_3d_array = new double[number_of_frames][][];

			// Initialize all internal arrays. Apparently you have to manually do that
			// with a jagged array in C#

			for (frame = 0; frame < number_of_frames; frame++)
			{
				the_3d_array[frame] = new double[number_of_atoms][];
				for (row = 0; row < number_of_atoms; row++)
				{
					// Innermost array needs 4 elements for atom type and x, y, and z coordinates
					the_3d_array[frame][row] = new double[4];
				}
			}

			// Start the current line at 2 to skip the first 2 comment lines and increment it by 2
			// to skip the comment lines between frames
			
			for (frame = 0, current_line = 2; frame < number_of_frames; frame++, current_line += 2){
					for (row = 0, column = 1, current_coord = ""; row < number_of_atoms; row++, 
					current_line++, column = 1)
					{
						// The atom type will be at index 1 of a line
						atom = file_lines[current_line][1];

						/* If that atom is 'H' we'll have the 1st element of the innermost array 
						be 1, if it's 'C', we'll have it be 2, and if it's 'O', we won't do 
						anything and let it be 0, which was it's initialized value */

						if (atom == 'H')
							the_3d_array[frame][row][0] = 1;
						else if (atom == 'C')
							the_3d_array[frame][row][0] = 2;
						
						line_length = file_lines[current_line].Length;

						// The line iterator starts at 7 because that is the first character that
						// is part of a coordinate on every line
						for (line_iterator = 7; line_iterator < line_length; line_iterator++)
						{
							current_char = file_lines[current_line][line_iterator];
							if (current_char != ' ')
								current_coord += current_char;
							else if (current_coord != "")
							{
								the_3d_array[frame][row][column] = Double.Parse(current_coord);
								current_coord = "";
								column++;
							}
						}
						// After the for loop above gets done, current_coord will have the value of
						// the last coordinate.
						the_3d_array[frame][row][column] = Double.Parse(current_coord);
						current_coord = "";
					}
				}
				return the_3d_array;
			}

		static void print_3d_array(double[][][] a_3d_array)
		{
			int frame, row;
			string atom;
			double atom_double, x_coord, y_coord, z_coord;
			int number_of_frames = a_3d_array.Length;
			int number_of_atoms = a_3d_array[0].Length;

			for (frame = 0; frame < number_of_frames; frame++)
			{
				Debug.Log("Now looking at frame " + frame);

				for (row = 0; row < number_of_atoms; row++)
				{
					Debug.Log("Now looking at row " + row);

					atom_double = a_3d_array[frame][row][0];
					if (atom_double == 1)
						atom = "hydrogen";
					else if (atom_double == 2)
						atom = "carbon";
					else
						atom = "oxygen";
					
					x_coord = a_3d_array[frame][row][1];
					y_coord = a_3d_array[frame][row][2];
					z_coord = a_3d_array[frame][row][3];

					Debug.Log("The atom is" + atom + ", the x coord is "+ x_coord + ", the y coord is " + y_coord + ", "
						+ "and the z coord is" + z_coord);
				}

				Debug.Log("\n");

			}
		}
	}
}