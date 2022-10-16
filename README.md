Rubick’s Cube Game
Game Controls-
Right click hold and drag rotates the whole cube. Left Click hold drag rotates a given side.
Working Principle-
The rubick’s cube is made up of 27 smaller cubes( referred to as little cubes in the code). Each of these 27 cubes is an instance of a prefab.

Each of the six sides has 9 cube faces, and there are 9 raycasts per side being done to each face, to get the color of the face at any given time. These colors are then mapped to a cubemap, which is a canvas gameobject containing 27 simple sprite images, that correspond to the color of the faces. The cubemap helps to keep a track of the state of the cube.

The raycasts on each side are fired and cubemap is updated every time the user makes a move. When all the faces of the cubemap are of the same color, the cube is considered to be solved.

Classes Description-

Rubick’s Cube functionality is controlled by the following classes-

Read Cube- This contains a reference point on each side, from which 9 rays are built and cast to each face of each side of the cube.

Cube State- It contains 2 functions ( PickUp() and PutDown() ). The pickUp function works such that when a user clicks on a side, all the little cubes in that side are parented to the middle cube( cube at 4th index since its only a 3x3 cube).
The putDown() function deparents the cubes after the rotation of the side is over. Essentially, this function also calls the saveMove() function of the CubeManager class to save the move for future undo purposes.

Select Face- Detects which side has been clicked on and then calls the PickUp() method of the CubeState class.

Automate- Contains the logic for automation functions.
It contains a predefined list of all possible moves(18) that can be made on the cube, referenced by a string.
The DoMove(string move) method executes a given move. The Shuffle() method creates a random list of moves and stores in allMoves and the Update() method calls the DoMove() method for each move in allMoves.

PivotRotation- It is contained by the middle cube on each side.
This class contains logic for rotating a side by determining the correct axis of rotation. It also contains the logic for autoRotation, which basically autorotates the cube side to the nearest target rotation( so the user does not have to drag the side to the full rotation everytime)

Game Logic is controlled by the following classes-

GameManager- Connects the UI to the cube. It contains all the methods that are triggered by button events, which in turn call the respective functionality on the CubeManager script contained by the Rubick’s cube.

CubeManager- Contains the methods for cube logic such as state management and undo, and other methods that are required to make those logic happen. This script is a midway connector b/w the GameManager script on the UI, and the other scripts that mange cube logic( such as ReadCube, ReadState etc). It also connects the cube to Utility scripts such as SaveScript, CustomColor script etc.

SaveScript- This class contains all the save logic, containing classes for saving position and rotation after every move for undo purposes, saving progress by storing the positions, rotations, states, time elapsed and colors.

Timer- Contains logic to keep track and display time

CreateColor- Contains color picker logic, and then sets the color of the cube by accessing the SetCubeColor() method of CubeManager class.

Utility Classes and Scriptable Objects-

CustomColors- A scriptable object that creates a color_set template that gets used to provide color to the faces of the cube and save selected colors.

Utilities- Contains general methods for storing the default state, setting materials on a given object etc.

MathCalculations- Contains methods to convert Quaternion Angles to Euler and vice versa.

Plugins-

PlayerPrefsExtra- provides a wider range of datatypes that can be stored, compared to Unity’s default PlayerPrefs

Cinemachine- Used for the final orbital animation of the camera that plays after the user solves the cube
