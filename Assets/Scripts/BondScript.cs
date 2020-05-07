using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to every bond object and prefab
public class BondScript : MonoBehaviour
{
    // -20 is what we made the x, y, and z coordinate for a bond be when it is not in a reaction
    Vector3 positionWhenNotInReaction = new Vector3(-20, -20, -20);
    // The 1st and 3rd values are the x and z values and they affect the thickness of the bond object 
    // and they will always be 0.1. They 2nd value is the y value and it affects the length of a bond
    // object and it will change to be an appropriate length to make it appear as if the bond is connecting
    // 2 atoms.
    Vector3 scaleWhenNotInReaction = new Vector3(0.1f, 0, 0.1f);

    // The bondsDict dictionary is mentioned in section 2.1 of the SDD.
    // It gets initialized in MainSceneScript in the instantiateBonds function.
    // 1st Vector3 is for position in a frame, 2nd Vector3 is for localScale in a frame, 
    // and Quaternion is for rotation in a frame.
    public Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> bondsDict;

    // Start is called right after the bond objects are instantiated at the beginning of a reaction
    void Start()
    {
        // transform.position and localScale refer to the position and scale of the bond object this script is attached to
        transform.position = positionWhenNotInReaction;
        transform.localScale = scaleWhenNotInReaction;
    }

    // Update is called during every frame update. This function for BondScript is mentioned in section 
    // 3.2.3.5.1.9c of the SDD.
    void Update()
    {
        // Check if the reaction should be playing
        if (MainSceneScript.playing)
        {
            // Check if there is an entry for the current frame
            if (bondsDict.ContainsKey(MainSceneScript.frame))
            {
                // Set the position, scale, and rotation to their values in the Dictionary for the current frame
                transform.position = bondsDict[MainSceneScript.frame].Item1;
                transform.localScale = bondsDict[MainSceneScript.frame].Item2;
                transform.rotation = bondsDict[MainSceneScript.frame].Item3;
            }

            // Move covalent bond out of reaction if need be. The only time 
            // transform.localScale.y is 0 is when it is moved outside the reaction.
            else if (transform.localScale.y != 0)
            {
                transform.position = positionWhenNotInReaction;
                transform.localScale = scaleWhenNotInReaction;
            }
        }
    }
}
