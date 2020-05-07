using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to every atom object and prefab
public class AtomScript : MonoBehaviour
{
    // This gets initialized from MainSceneScript in the instantiateAtoms function
    public Vector3[] coords2dArray;

    // Update is called once per frame
    void Update()
    {
        if (MainSceneScript.playing)
		{
            // transform.position refers to the position of the atom this script is attached to
            transform.position = coords2dArray[MainSceneScript.frame];
        }
    }
}
