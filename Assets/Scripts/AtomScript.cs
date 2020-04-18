using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomScript : MonoBehaviour
{

    public Vector3[] coords2dArray;
    float xCoord, yCoord, zCoord;

    // Update is called once per frame
    void Update()
    {
        if (MainSceneScript.playing)
            transform.position = coords2dArray[MainSceneScript.frame];
    }

    public void setCoords2dArray(Vector3[] coords2dArray)
    {
        // this.coords2dArray refers to the variable that I declared at the top of the class,
        // coords2dArray refers to the parameter I passed in.
        this.coords2dArray = coords2dArray;
    }
}
