using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomScript : MonoBehaviour
{
    public Vector3[] coords2dArray;

    // Update is called once per frame
    void Update()
    {
        if (MainSceneScript.playing)
            transform.position = coords2dArray[MainSceneScript.frame];
    }
}
