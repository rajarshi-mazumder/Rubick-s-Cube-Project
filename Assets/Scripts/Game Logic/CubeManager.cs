using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CubeManager : MonoBehaviour
{
    #region Dependent Variables
    [SerializeField]
    GameObject cubeMap;  

    [SerializeField]
    CustomColors customColorSet;

    CustomColors currentColorSet;
    Utiltities utilties;
    Automate automate;
    #endregion

    public List<List<Vector3>> undoPositions = new List<List<Vector3>>();
    public List<List<Vector3>> undoRotations = new List<List<Vector3>>();
    protected List<GameObject> little_cubes_list = new List<GameObject>();
    List<Vector3> rots = new List<Vector3>();


    public enum States { Default, Unsolved, Solved }; // States enum to keep track of cube state
    public States cur_state { get; set; }

    public int maxUndoCount; // keeps track of max number of accumulated undos at a given time
    void Start()
    {
        utilties = new Utiltities();
        automate = this.GetComponent<Automate>();
        GetAllLittleCubes();
        InitializeDefaultColorSet();

    }

   
    private void LateUpdate()
    {
        if (automate.isShuffleEnded && GameManager.hasGameStarted) // check state of the cube after every interaction
        {
            
            this.GetComponent<Timer>().StartTimer();
            bool temp_state= CheckIfsolved();

        }
    }
    public CustomColors InitializeDefaultColorSet() // Returns default color values
    {
        CustomColors default_color_Set = ScriptableObject.CreateInstance<CustomColors>();
        default_color_Set.front = PlayerPrefsExtra.GetColor("front_def_color");
        default_color_Set.back = PlayerPrefsExtra.GetColor("back_def_color");
        default_color_Set.top = PlayerPrefsExtra.GetColor("top_def_color");
        default_color_Set.down = PlayerPrefsExtra.GetColor("down_def_color");
        default_color_Set.left = PlayerPrefsExtra.GetColor("left_def_color");
        default_color_Set.right = PlayerPrefsExtra.GetColor("right_def_color");
        return default_color_Set;
   
    }
    public void SetDefaultState()
    {

        currentColorSet = InitializeDefaultColorSet(); // Gets CustomColors object with default colors

        foreach (GameObject g in little_cubes_list) // Resets every little cube to its default position, rotation and color
        {
            g.transform.position = PlayerPrefsExtra.GetVector3(g.name + "DefPos");
            var savedAngles = PlayerPrefsExtra.GetVector3(g.name + "DefRot");
            Vector3 tmp = new Vector3(MathCalculations.UnwrapAngle(savedAngles.x),
                                    MathCalculations.UnwrapAngle(savedAngles.y),
                                    MathCalculations.UnwrapAngle(savedAngles.z));
            g.transform.eulerAngles = tmp;

            foreach (Transform child in g.transform) // Sets default color to each of the faces of every little cube
            {
                if (child.gameObject.GetComponent<SetColor>())
                    child.GetComponent<SetColor>().SetFaceColor(currentColorSet); 
            }
        }
        
        PlayerPrefs.SetFloat("time_elapsed", 0); // resets saved timer
        this.GetComponent<Timer>().ResetTimer(); //resets time counter variable
        SetCubeColor(currentColorSet);
        SetState(States.Default);

    }
    public void SaveMove() // called everytime the player completes a move
    {
        this.gameObject.GetComponent<SaveScript>().SaveMove(little_cubes_list);
    }

    // Undo works by storing the player's moves after every move in the undoPositions and undoRotations list
    // Whenever an undo is called, the last values from these 2 lists is popped, unless there is only 1 item in each list
    public void UndoMove()
    {
        int undoCount = undoPositions.Count;
        int index;

        if (undoCount >= 1)
        {
            if (maxUndoCount <= undoCount)
                maxUndoCount = undoCount;

            if (undoCount == maxUndoCount)
                index = undoCount - 2;
            else index = undoCount - 1;

            RevertByOneMove(index);

            if (index != 0) // when the only move left to undo is the current move, then we do not pop it off as it will create indexOutOfRange Exception
            {
                try
                {
                    undoPositions.RemoveAt(index);
                    undoRotations.RemoveAt(index);
                }
                catch (System.Exception)
                {   
                    // In case the undo list ever becomes null and it still tries to read it
                    Debug.Log("No undos to remove");
                }
            }
        }
        else try
            {
                RevertByOneMove(0);
                undoPositions.RemoveAt(0);
                undoRotations.RemoveAt(0);
               
            }
            catch (System.Exception)
            {
                print("Undo Exception");
            }

    }


    // this function sets the position and rotation of all little cubes to the saved position and rotation
    // at the specified index in the undoPositions and undoRotations lists
    void RevertByOneMove(int index) 
    {   

        for (int j = 0; j < little_cubes_list.Count; j++)
        try{
            Vector3 tempPos = new Vector3(undoPositions[index][j].x, undoPositions[index][j].y, undoPositions[index][j].z);
            little_cubes_list[j].transform.position = tempPos;


            Vector3 tempRot = new Vector3(MathCalculations.UnwrapAngle(undoRotations[index][j].x),
                                    MathCalculations.UnwrapAngle(undoRotations[index][j].y),
                                    MathCalculations.UnwrapAngle(undoRotations[index][j].z));
            little_cubes_list[j].transform.eulerAngles = tempRot;


        }
         catch(System.Exception) //if the undoPositions and/ or undoRotations lists are empty and it still tries to read it
            {
                Debug.Log("Nothing to undo");
            }
        
    }
    public void SaveDefaultState() // needs to be called only once to save the playerPref values
    {
        
        utilties.SaveDefaultState(little_cubes_list); // saves the default positions and rotations of all the little cubes
        PlayerPrefs.SetInt("def_saved", 0);
    }

    public void GetAllLittleCubes() // gets a list of all the little cubes contained in the parent cube
    {
        little_cubes_list.Clear();
        rots.Clear();

        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Rays") // ignore the child called rays
            {
                little_cubes_list.Add(child.gameObject);
            }

        }
    }
    public void SaveProgress() // saves the current position, rotation, and color of all the little cubes
    {
        this.GetComponent<SaveScript>().SaveProgress(little_cubes_list, rots);
        this.GetComponent<SaveScript>().SaveColors(currentColorSet);

    }


    public void LoadSavedProgress() // sets the postion, rotation and color of all the little cubes. Also saves the time elapsed
    {
        this.GetComponent<SaveScript>().LoadSavedProgress(little_cubes_list);
        this.GetComponent<Timer>().SetTimer(PlayerPrefs.GetFloat("time_elapsed"));

    }
    public void SetState(States state) // setter function to set the state of the cube from this or other classes
    {
        cur_state = state;
        PlayerPrefs.SetString("state", cur_state.ToString());
    }

    public CustomColors SetSavedColor() // sets the color of each of the little cubes to the saved color
    {
        customColorSet = ScriptableObject.CreateInstance<CustomColors>();
        customColorSet.front = PlayerPrefsExtra.GetColor("front_color");
        customColorSet.back = PlayerPrefsExtra.GetColor("back_color");
        customColorSet.top = PlayerPrefsExtra.GetColor("top_color");
        customColorSet.down = PlayerPrefsExtra.GetColor("down_color");
        customColorSet.left = PlayerPrefsExtra.GetColor("left_color");
        customColorSet.right = PlayerPrefsExtra.GetColor("right_color");

        foreach (GameObject g in little_cubes_list)
        {
            int i = 0;
            foreach (Transform child in g.transform)
            {
                if (child.gameObject.GetComponent<SetColor>())
                    child.GetComponent<SetColor>().SetFaceColor(customColorSet);
                i++;

            }

        }
        print("Set saved color called");
        return customColorSet;
    }

    // checks if cube has been solved by checking the cubemap
    // this is done by iterating over the cubemap sides, and storing the color of first face in the respective side
    // as soon as the color of any of the other faces in that side is different from the stored color, the check is stopped 
    public bool CheckIfsolved() 
    {

        foreach (Transform face in cubeMap.transform)
        {
            Color cube_col = Color.black; // temporary variable that stores the color of the first face in each side, which is then 
                                          // compared to the color of the rest of the faces                                                    

            foreach (Transform face_cube in face)
            {
                var temp_col = face_cube.GetComponent<Image>().color;
                if (cube_col == Color.black)
                {
                    cube_col = temp_col;
                }

                else if (temp_col != cube_col)
                {
                    SetState(States.Unsolved);
                    return false;
                }
            }
        }
        if (cur_state == States.Default) // do not return true or solved if the cube is in default state and has not been state.
            return false;                 // this can occur if the check happens before the cube is shuffled

        SetState(States.Solved);
        this.GetComponent<Timer>().StopTimer();
        return true;
    }
    public void SetCubeColor(CustomColors colorSet) // sets the color of the entire cube to the CustomColors object passed in the argument
    {
        foreach(GameObject g in little_cubes_list)
        {
            int i = 0;
            foreach(Transform child in g.transform)
            {
                if(child.gameObject.GetComponent<SetColor>())
                    child.GetComponent<SetColor>().SetFaceColor(colorSet);
                i++;

            }
            
        }
        currentColorSet = colorSet;
        
    }
}
