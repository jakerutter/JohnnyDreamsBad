using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    private bool stopFixedUpdate;
    private float countdown = .25f;
    private int index = 0;
    private AudioManager audioPlayer;
    private bool haveAddedImage = false;
    private Image gpImage;
    private int currentLevel;
    private float endGameTimer = .25f;
    private bool showEndGameCalled;
    private bool loadedAndReady = false;

    public CharController charCtrl;
    public GameObject pauseMenu;
    public GameObject gameInfoTexts;
    public Sprite sleepyImage;
    public Sprite[] scaredSprites;
    public float countdownSet;
    public Button pauseEndGameButton;
    public Button resumeGameButton;
    public Button pauseInfoButton;
    public Button pauseRetryGameButton;

    void Awake()
    {
        countdown = countdownSet;
        audioPlayer = FindObjectOfType<AudioManager>();
        currentLevel = GameStats.GetCurrentLevelNumber();
    }

    void Update()
    {
        if(!GameStats.IsGameActive()){return;}

        if(Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Escape)){
            Time.timeScale = 0f;

            Button btn3 = pauseEndGameButton.GetComponent<Button>();
            btn3.onClick.AddListener(SoundOnClick);

            Button btn4 = resumeGameButton.GetComponent<Button>();
            btn4.onClick.AddListener(SoundOnClick); 

            Button btn5 = pauseInfoButton.GetComponent<Button>();
            btn5.onClick.AddListener(SoundOnClick);

            Button btn6 = pauseRetryGameButton.GetComponent<Button>();
            btn6.onClick.AddListener(SoundOnClick);

            pauseMenu.SetActive(true);
            audioPlayer.Play("MenuAction");
        }

        if(GameStats.IsGameOver() && !showEndGameCalled)
        {
            endGameTimer -= Time.deltaTime;

            if(endGameTimer <= 0f){
                ShowEndGameStats();
                showEndGameCalled = true;
            }
            
        }
    }

    void FixedUpdate()
    {
        //should only run when game is over
        if(!GameStats.IsGameOver()){return;}
        if(stopFixedUpdate){return;}
        if(!loadedAndReady){return;}
        GameObject gamePanel = GameObject.FindGameObjectWithTag("GamePanel");
        if(gamePanel == null){Debug.LogWarning("gamePanel is null in FixedUpdate of EndGameScript");}
        Image gpImage = gamePanel.GetComponent<Image>(); 

        if(!haveAddedImage)
        {
            if(gpImage == null)
            {
               //create image component  for game panel
                gamePanel.AddComponent<Image>();
            }
            haveAddedImage = true;
        }
        
        if(countdown <= 0f && index < 4 && GameStats.IsGameOver())
        {
            gpImage = gamePanel.GetComponent<Image>(); 

            index += 1;
            gpImage.sprite = scaredSprites[index];
            countdown = countdownSet;
        }

        countdown -= Time.deltaTime;
        Mathf.Clamp(countdown, 0, Mathf.Infinity);
        
        if(index == 4)
        {
            stopFixedUpdate = true;
            return;
        }  
    }

    public void EndGame()
    {
        // restart time
        Time.timeScale = 1f;
        // reopen the initial scene
        Debug.Log("About to LoadScene 0 from EndGame()");
        GameStats.SetGameOver(true);
        SceneManager.LoadScene(0);
    }

    private void ShowEndGameStats()
    {
        Debug.Log("kicking off showEndGameStats");
        //disable scene so there are no wraith / player collisions
        GameObject playArea = GameObject.FindGameObjectWithTag("Dreamscape1");
        playArea.SetActive(false);
        
        //set endGame  panel variables and display
        GameObject endGameCanvas = GameObject.FindGameObjectWithTag("EndGameCanvas");
        if(endGameCanvas != null)
        {
            CanvasGroup endCanvasGroup = endGameCanvas.GetComponent<CanvasGroup>();
            endCanvasGroup.alpha = 1f; //makes everything visible
            endCanvasGroup.blocksRaycasts = true; //can recieve input    
        } else
        {
            Debug.LogWarning("endGameCanvas is null in ShowEndGameStats");
        }

        // //ensure we are showing scared wake up image and title
        GameObject gp = GameObject.FindGameObjectWithTag("GamePanel");
        if(gp != null)
        {
            CanvasGroup gpCanvas = gp.GetComponent<CanvasGroup>();
            gpCanvas.alpha = 1f;
           
        } else 
        {
            Debug.LogWarning("game panel is null in ShowEndGameStats");
        }

        //Display stats
        string resultsNum = GameStats.GetLevelsPassed().ToString();

        GameObject gameResults = GameObject.FindGameObjectWithTag("Results");

        Text resultsText = gameResults.GetComponent<Text>();
        resultsText.text = resultsNum + " of 4"; 
        //IF WINNER SAY GREAT JOB
        if(GameStats.GetBlueCheck()){
            resultsText.text += "\n You Have Escaped All The Dreams!";
            //hide ChallengeText and RetryButton
            GameObject chall = GameObject.FindGameObjectWithTag("ChallengeText");
            chall.SetActive(false);
            // GameObject retry = GameObject.FindGameObjectWithTag("RetryButton");
            // retry.SetActive(false);
        }
        //set sprintCanvas to invisible
        GameObject sprintCanvas = GameObject.FindGameObjectWithTag("SprintCanvas");
        CanvasGroup sprint = sprintCanvas.GetComponent<CanvasGroup>();
        sprint.alpha = 0f;
        sprint.blocksRaycasts = false;
        // let scared images roll (fixed update)
        loadedAndReady = true;
    }

    public void ToggleGameInfoPauseMenu()
    {
        gameInfoTexts.SetActive(!gameInfoTexts.activeSelf);
    }

    public void ResumeGamePlay()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Retry()
    {
        Scene scene = SceneManager.GetActiveScene();
        //Debug.Log("Active Scene name is: " + scene.name + ". Active Scene index: " + scene.buildIndex);
        SceneManager.LoadScene(scene.buildIndex);

        Time.timeScale = 1f;
       
        GameStats.SetGameActive(true);
        GameStats.SetHasPlayed(true);
    }

    public void RetryFromEndGame()
    {
        GameStats.SetStartingFromEnd(true);
        GameStats.SetGameActive(false);
        GameStats.SetHasPlayed(false);
        GameStats.SetGameOver(false);
        GameStats.SetLevelsPassed(0);
        GameStats.SetCurrentLevel(1);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;

        //hide game over ui
        GameObject endGameCanvas = GameObject.FindGameObjectWithTag("EndGameCanvas");
        if(endGameCanvas != null)
        {
            CanvasGroup endCanvasGroup = endGameCanvas.GetComponent<CanvasGroup>();
            endCanvasGroup.alpha = 0f;
            endCanvasGroup.blocksRaycasts = false;       
        }

        //switch to dreamscape camera
        GameObject cam = GameObject.FindGameObjectWithTag("CamController");
        CameraController camCtrl = cam.GetComponent<CameraController>();
        Debug.LogWarning("Calling set active cam to Dream1Camera from RetryFromEndGame");
        camCtrl.SetActiveCamera("Dream1Camera");

        //hide game intro stuff / sleepy images and the like
        GameObject staticUI = GameObject.FindGameObjectWithTag("StaticUI");
        StaticUI stat = staticUI.GetComponent<StaticUI>();
        stat.HideStartMenuButtons();
        GameStats.SetStartingFromEnd(false);
    }

    void SoundOnClick()
    {
        audioPlayer.Play("MenuAction");
    }

    public void CallEndGameFinale()
    {
        GameStats.EndGameFinale();
    }
}