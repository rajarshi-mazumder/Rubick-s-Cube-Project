using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    #region Canvas Elements
    [SerializeField]
    GameObject homeScreenPanel;

    [SerializeField]
    GameObject startNewGameUI;

    [SerializeField]
    GameObject restartGameUI;

    [SerializeField]
    GameObject colorPickerContainer;

    [SerializeField]
    GameObject addCustomColorBtn;

    [SerializeField]
    GameObject backToTitleScreenBtn;

    [SerializeField]
    GameObject sideMenuPanel;

    [SerializeField]
    GameObject timerText;

    [SerializeField]
    GameObject toggleTimerBtn;

    [SerializeField]
    GameObject quitGameDialogBox;

    [SerializeField]
    GameObject congratulationsContainer;

    [SerializeField]
    Text timeTaken ;

    [SerializeField]
    GameObject undoBtn;

    [SerializeField]
    GameObject camAnimationContainer;

    [SerializeField]
    GameObject LoadGameBtn;

    [SerializeField]
    Slider MaxShufflesSlider;

    [SerializeField]
    Text maxShuffleText;
    #endregion

    [SerializeField]
    GameObject mainCamera;

    CubeManager cubeManager;
    GameObject cubeParent;

    public static bool hasGameStarted = false; // player cannot interact with the game when this is false
    bool delayCheck = false; // to introduce delay after starting or resuming game before doing state checks

    void Start()
    {   
        delayCheck = false;
        congratulationsContainer.SetActive(false);
        sideMenuPanel.SetActive(false);
        startNewGameUI.SetActive(false);
        colorPickerContainer.SetActive(false);
        backToTitleScreenBtn.SetActive(false);
        camAnimationContainer.SetActive(false);

        MaxShufflesSlider.value = 30;

        if (!PlayerPrefs.HasKey("def_saved"))
            LoadGameBtn.SetActive(false);
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;

        cubeParent = GameObject.FindGameObjectWithTag("CubeParent");
        cubeManager = cubeParent.GetComponent<CubeManager>();    
    }

    
    void LateUpdate()
    {   
        // wait and then check if the cube is solved
        if (delayCheck && cubeManager.cur_state == CubeManager.States.Solved) 
            FinishGame();
        maxShuffleText.text = "Maximum no of Shuffles: " + MaxShufflesSlider.value;
    }

    public void StartNewGame() // gets called when Start New Game option is selected in the home screen
    {   
        homeScreenPanel.SetActive(false);

        cubeManager.SaveDefaultState();

        cubeManager.SetDefaultState();
        cubeParent.GetComponent<ReadCube>().SetInitializemap();
        cubeParent.GetComponent<SaveScript>().SaveDefaultColors();
        startNewGameUI.SetActive(true);      
    }
    public void DisplayRestartGameDialog()
    {
        restartGameUI.SetActive(true);
        hasGameStarted = false;
        cubeParent.GetComponent<Timer>().StopTimer();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartNewGame();
    }
    public void Play()
    {
        startNewGameUI.SetActive(false);
        backToTitleScreenBtn.SetActive(true);
        cubeParent.GetComponent<Automate>().Shuffle(((int)MaxShufflesSlider.value));
        SetDelay(2f); // Introduce a delay before checking cube state
    }
    public void LoadSavedGame()
    {
        homeScreenPanel.SetActive(false);
        cubeManager.SaveDefaultState();
        cubeManager.LoadSavedProgress();
        cubeParent.GetComponent<ReadCube>().SetInitializemap();
        cubeManager.SetSavedColor();
        cubeManager.SaveMove();
        backToTitleScreenBtn.SetActive(true);
        hasGameStarted = true;
        SetDelay(2f); // Introduce a delay before checking cube state
    }
    public void QuitGame()
    {
        quitGameDialogBox.SetActive(true);
        hasGameStarted = false;
        cubeParent.GetComponent<Timer>().StopTimer();
        
    }
    public void GoToTitleScreen()
    {
        quitGameDialogBox.SetActive(false);
        cubeManager.SaveProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }
    public void ResumeGame()
    {
        quitGameDialogBox.SetActive(false);
        restartGameUI.SetActive(false);
        hasGameStarted = true;
    }
    public void AddCustomColor()
    {
        addCustomColorBtn.SetActive(false);
        colorPickerContainer.SetActive(true);
    }
   public void ToggleMenu()
    {
        if (sideMenuPanel.activeInHierarchy)
            sideMenuPanel.SetActive(false);
        else sideMenuPanel.SetActive(true);
    }
    public void ToggleTimerVisibility()
    {
        if (timerText.activeInHierarchy)
        {
            timerText.SetActive(false);
            toggleTimerBtn.transform.GetChild(0).GetComponent<Text>().text = "Show Timer";
        }
        else
        {
            timerText.SetActive(true);
            toggleTimerBtn.transform.GetChild(0).GetComponent<Text>().text = "Hide Timer";
        }
    }
    public void FinishGame()
    {
        mainCamera.GetComponent<CinemachineBrain>().enabled = true;
        hasGameStarted = false;
        camAnimationContainer.SetActive(true);
        undoBtn.SetActive(false);
        StartCoroutine(ShowCongratulateDialog());

    }
    public void CongratulatePlayer()
    {
        congratulationsContainer.SetActive(true);
        timeTaken.text = timerText.GetComponent<Text>().text;
    }
    public void GoToTitleScreenAfterGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void CloseCongratulatePlayerDialog()
    {
        congratulationsContainer.SetActive(false);
    }
    IEnumerator ShowCongratulateDialog()
    {
        yield return new WaitForSeconds(3f);
        CongratulatePlayer();
    }
    public void SetDelay(float t)
    {
        StartCoroutine(Delay(t));
    }
    IEnumerator Delay(float t)
    {
        yield return new WaitForSeconds(t);
        delayCheck = true;
    }
}
