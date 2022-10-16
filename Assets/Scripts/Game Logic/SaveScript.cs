using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveScript: MonoBehaviour
{
    CubeManager cubeStateHandler;
    public GameObject cubeMap;
    

    void Start()
    {
        cubeStateHandler = this.gameObject.GetComponent<CubeManager>();
        cubeMap = GameObject.FindGameObjectWithTag("CubeMap");
    }
    
    public void SaveMove(List<GameObject> little_cubes_list) // saves the current position and rotation of all the little cubes in undoPosition
    {                                                       // and undoRotation lists, so that it can be used to undo the moves

        List<Vector3> littleCubesTempPos = new List<Vector3>();
        List<Vector3> littleCubesTempRot = new List<Vector3>();
        int i = 0;
        foreach (GameObject g in little_cubes_list)
        {
            littleCubesTempPos.Add(g.transform.position);

            Vector3 tempAngles= g.transform.eulerAngles;
            littleCubesTempRot.Add(new Vector3(MathCalculations.WrapAngle(tempAngles.x),
               MathCalculations.WrapAngle(tempAngles.y), MathCalculations.WrapAngle(tempAngles.z)));
            i++;

        }
        cubeStateHandler.undoPositions.Add(littleCubesTempPos);
        cubeStateHandler.undoRotations.Add(littleCubesTempRot);
        
    }
    public void SaveProgress(List<GameObject> little_cubes_list, List<Vector3> rots)
    {
        int i = 0;
        foreach (GameObject g in little_cubes_list) // saves the position and rotation of all the little cubes
        {
            Vector3 tempAngles = g.transform.eulerAngles;
            rots.Add(new Vector3(MathCalculations.WrapAngle(tempAngles.x),
                MathCalculations.WrapAngle(tempAngles.y), MathCalculations.WrapAngle(tempAngles.z)));
            PlayerPrefsExtra.SetVector3(g.name + "Rot", rots[i]);
            PlayerPrefsExtra.SetVector3(g.name + "Pos", g.transform.position);
            i++;
        }

        PlayerPrefs.SetFloat("time_elapsed", this.GetComponent<Timer>().timeElapsed); // saves the time elapsed

        if (cubeStateHandler.CheckIfsolved()) //saves the current state
        {
            cubeStateHandler.cur_state = CubeManager.States.Solved;
        }
        else cubeStateHandler.SetState(CubeManager.States.Unsolved);
        
    }
    public void LoadSavedProgress(List<GameObject> little_cubes_list) // sets the position and rotation of all the little cubes to the saved position and rotation
    {
        int i = 0;
        foreach (GameObject g in little_cubes_list)
        {
            g.transform.position = PlayerPrefsExtra.GetVector3(g.name + "Pos");

            Vector3 tempAngles = PlayerPrefsExtra.GetVector3(g.name + "Rot");
            Vector3 tmp = new Vector3(MathCalculations.UnwrapAngle(tempAngles.x),
                MathCalculations.UnwrapAngle(tempAngles.y), MathCalculations.UnwrapAngle(tempAngles.z));
            g.transform.eulerAngles = tmp;
            i++;
        }
        cubeStateHandler.CheckIfsolved();
    }

    public void SaveDefaultColors() // saves the default colors. needs to be called only once
    {
        PlayerPrefsExtra.SetColor("front_def_color", cubeMap.transform.Find("Front").GetChild(0).GetComponent<Image>().color);
        PlayerPrefsExtra.SetColor("back_def_color", cubeMap.transform.Find("Back").GetChild(0).GetComponent<Image>().color);
        PlayerPrefsExtra.SetColor("top_def_color", cubeMap.transform.Find("Up").GetChild(0).GetComponent<Image>().color);
        PlayerPrefsExtra.SetColor("down_def_color", cubeMap.transform.Find("Down").GetChild(0).GetComponent<Image>().color);
        PlayerPrefsExtra.SetColor("left_def_color", cubeMap.transform.Find("Left").GetChild(0).GetComponent<Image>().color);
        PlayerPrefsExtra.SetColor("right_def_color", cubeMap.transform.Find("Right").GetChild(0).GetComponent<Image>().color);
    }
    public void SaveColors(CustomColors colorSet) // saves the current colors
    {
        try
        {
            PlayerPrefsExtra.SetColor("front_color", colorSet.front);
            PlayerPrefsExtra.SetColor("back_color", colorSet.back);
            PlayerPrefsExtra.SetColor("top_color", colorSet.top);
            PlayerPrefsExtra.SetColor("down_color", colorSet.down);
            PlayerPrefsExtra.SetColor("left_color", colorSet.left);
            PlayerPrefsExtra.SetColor("right_color", colorSet.right);
        }
        catch(System.Exception)
        {
            //Invalid color set
        }
    }
   
}
