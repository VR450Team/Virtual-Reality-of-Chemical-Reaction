using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to every atom object and prefab
public class AtomScript : MonoBehaviour
{
    // coords2dArray is an array of Vector3s, similar to a 2d array. It is mentioned in part 2.1 of the SDD.
    // This gets initialized from MainSceneScript in the instantiateAtoms function.
    public Vector3[] coords2dArray;

    // Update is called during every frame update. This function for AtomScript is mentioned in section 
    // 3.2.3.5.1.9c of the SDD.
    // FR.5 : During the simulation, the atoms will change position.
    void Update()
    {
        // Check if the reaction should be playing
        if (MainSceneScript.playing)
		{
            // transform.position refers to the position of the atom this script is attached to
            transform.position = coords2dArray[MainSceneScript.frame];
        }
    }
}
