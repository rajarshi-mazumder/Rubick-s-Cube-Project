using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public static bool autoRotating = false;
    public static bool started = false;
    
    public void PickUp(List<GameObject> cubeSide) // called when any side of the parent cube is clicked on by the player
    {
        foreach (GameObject face in cubeSide)
        {
            //Attach parent of each face to the parent of the middle cube( 4th index cube for 3x3)
            //Unless it is already a middle cube(4th index for 3x3)
            if (face != cubeSide[4])
            {
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;

            }
        }
        
    }
    public void PutDown(List<GameObject> littleCubes, Transform pivot) // called after the rotation of the side is completed
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
        if (!CubeState.autoRotating && CubeState.started)
        {
            this.GetComponent<CubeManager>().SaveMove();
        }
    }
    


}

