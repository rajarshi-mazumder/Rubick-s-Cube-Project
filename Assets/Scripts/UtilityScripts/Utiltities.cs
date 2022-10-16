using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utiltities
{   
    
    public void SaveDefaultState(List<GameObject> little_cubes_list) // Called only once
    {
        foreach (GameObject g in little_cubes_list)
        {
            Vector3 initEulerAngles = g.transform.eulerAngles;
            PlayerPrefsExtra.SetVector3(g.name + "DefRot", new Vector3(MathCalculations.WrapAngle(initEulerAngles.x),
               MathCalculations.WrapAngle(initEulerAngles.y), MathCalculations.WrapAngle(initEulerAngles.z)));
            PlayerPrefsExtra.SetVector3(g.name + "DefPos", g.transform.position);
        }
    }

    public static void SetMaterialColor(MeshRenderer meshrenderer, Color color)
    {
        meshrenderer.material.color = color;
    }
    
}
