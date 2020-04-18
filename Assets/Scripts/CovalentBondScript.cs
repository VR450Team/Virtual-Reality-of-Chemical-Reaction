using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovalentBondScript : MonoBehaviour
{
    float distance;
    Vector3 atom1Coords, atom2Coords, scaleVector, positionWhenNotInReaction, scaleWhenNotInReaction;
    Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> bondsDict;

    // Start is called before the first frame update
    void Start()
    {
        positionWhenNotInReaction = new Vector3(-20, -20, -20);
        scaleWhenNotInReaction = new Vector3(0.1f, 0, 0.1f);
        scaleVector = scaleWhenNotInReaction;

        transform.position = positionWhenNotInReaction;
        transform.localScale = scaleVector;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainSceneScript.playing)
        {
            if (bondsDict.ContainsKey(MainSceneScript.frame))
            {
                transform.position = bondsDict[MainSceneScript.frame].Item1;
                transform.localScale = bondsDict[MainSceneScript.frame].Item2;
                transform.rotation = bondsDict[MainSceneScript.frame].Item3;
            }
            // Move covalent bond out of reaction if need be
            else if (bondsDict.ContainsKey(MainSceneScript.frame - 1))
            {
                transform.position = positionWhenNotInReaction;
                transform.localScale = scaleWhenNotInReaction;
            }
        }
    }

    public void setBondsDict(Dictionary<int, Tuple<Vector3, Vector3, Quaternion>> bondsDict)
    {
        this.bondsDict = bondsDict;
    }
}
