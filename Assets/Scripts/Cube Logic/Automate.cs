using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    public GameManager gameManager;
    public static List<string> moveList = new List<string>() { };
    private readonly List<string> allMoves = new List<string>() // predefined named list of all possible moves
        { "U", "D", "L", "R", "F", "B",
          "U2", "D2", "L2", "R2", "F2", "B2",
          "U'", "D'", "L'", "R'", "F'", "B'"
        };

    #region logic related variables

    public bool isShuffleStarted = false;
    public bool isShuffleEnded = true;
    public int totalShuffles = 0;
    public int shuffleCounter = 0;

    #endregion

    #region References to other components on the cube object

    private CubeState cubeState;
    private ReadCube readCube;
    private CubeManager cubeManager;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
        cubeManager = FindObjectOfType<CubeManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started) // this block is executed to auto shuffle the cube
        {
            if (isShuffleStarted == false)
            {
                isShuffleStarted = true;
                isShuffleEnded = false;
                
            }

            //Do the move at the first index;
            DoMove(moveList[0]);

            // remove the move at the first index
            moveList.Remove(moveList[0]);
        }

        if (isShuffleStarted && moveList.Count <= 0) // this block checks if shuffle has ended. Wait for shuffle to end and then store the position and rotation
        {                                            // of all the little cubes is saved using SaveMove(). This becomes the first element in the undoPosition
                                                     // and undoRotation lists of CubeManager script
            isShuffleStarted = false;
            isShuffleEnded = true;
            if (!GameManager.hasGameStarted) 
            {
                StartCoroutine(DelayAfterShuffleEnded());
            }
        }
        
    }

    public void Shuffle(int maxShuffle)
    {
        List<string> moves = new List<string>();
        int shuffleLength = Random.Range(1,maxShuffle);
        totalShuffles = shuffleLength;
        for (int i = 0; i < shuffleLength; i++)
        {
            int randomMove = Random.Range(0, allMoves.Count);
            moves.Add(allMoves[randomMove]);
        }
        moveList = moves;
        cubeManager.cur_state = CubeManager.States.Unsolved;
        
        
    }


    void DoMove(string move)
    {
        readCube.ReadState();
        CubeState.autoRotating = true;
        if (move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
        shuffleCounter++;
    }


    void RotateSide(List<GameObject> side, float angle)
    {
        // automatically rotate the side by the angle
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }
    IEnumerator DelayAfterShuffleEnded() // wait for 1s after shuffle is over. Then saveMove and start the game
    {                                   // this is done to ensure that the move is saved ONLY after the shuffle is over
        yield return new WaitForSeconds(1f);
        cubeManager.SaveMove();
        GameManager.hasGameStarted = true;
    }
}
