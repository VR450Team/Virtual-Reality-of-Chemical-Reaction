using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is attached to every bond object and prefab
public class BondScript : MonoBehaviour
{
    Vector3 positionWhenNotInReaction = new Vector3(-20, -20, -20);
    Vector3 scaleWhenNotInReaction = new Vector3(0.1f, 0, 0.1f);

    // This variable gets initialized in MainSceneScript in the instantiateBonds function.
    // 1st Vector3 is for position in a frame, 2nd Vector3 is for localScale in a frame, 
    // and Quaternion is for rotation in a frame.
    public Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> bondsDict;

    // Start is called before the first frame update
    void Start()
    {
        // transform.position and localScale refer to the position and scale of the bond object this script is attached to
        transform.position = positionWhenNotInReaction;
        transform.localScale = scaleWhenNotInReaction;
    }

    // Update is called once per frame
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
