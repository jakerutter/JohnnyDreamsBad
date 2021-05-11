using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticUI : MonoBehaviour
{
    public Text gameTitle;
    public Text gameInfo;
    public Text gameKeys;
    public Button enterGameButton;
    public Button gameInfoButton;
    public GameObject bubbleHolder;
    public Image panelImage;
    public GameObject sprintGameObj;

    private int currentLevel;
    private bool gameInfoVisible;
    private AudioManager audioPlayer;
    private CanvasGroup sprintCanvasGroup;
    private CanvasGroup hintCanvas;
    private GameObject hint;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioManager>();
        sprintCanvasGroup = sprintGameObj.GetComponent<CanvasGroup>();  
        if(sprintCanvasGroup == null)
        {
            GameObject sc = GameObject.FindGameObjectWithTag("SprintCanvas");
            sprintCanvasGroup = sc.GetComponent<CanvasGroup>();
        }
        hint = GameObject.FindGameObjectWithTag("HintText");
        if(hint != null)
        {
           hintCanvas = hint.GetComponent<CanvasGroup>(); 
        }
        
    }

     void Start()
    {
        currentLevel = GameStats.GetCurrentLevelNumber();

        if(currentLevel == 1 && !GameStats.GetHasPlayed())
        {   
            Button btn = enterGameButton.GetComponent<Button>();
            btn.onClick.AddListener(SoundOnClick);

            Button btn2 = gameInfoButton.GetComponent<Button>();
            btn2.onClick.AddListener(SoundOnClick); 
        }
    }

    void SoundOnClick()
    {
        audioPlayer.Play("MenuAction");
    }

    //toggle sprint text accoridng to sprint cooldown
    public void ToggleSprintText(bool activate)
    {    
        if (activate)
        {
            sprintCanvasGroup.alpha = 1f; //makes everything visibile
        }
        else 
        {
            sprintCanvasGroup.alpha = 0f; //this makes everything transparent
        }
    }

    public void HideStartMenuButtons()
    {
        enterGameButton.gameObject.SetActive(false);
        gameInfoButton.gameObject.SetActive(false);
        gameTitle.gameObject.SetActive(false);
        gameInfo.gameObject.SetActive(false);
        gameKeys.gameObject.SetActive(false);
        bubbleHolder.SetActive(false);

        GameStats.SetGameActive(true);
        GameStats.SetHasPlayed(true);
            
        CanvasGroup panelCanvas = panelImage.gameObject.GetComponent<CanvasGroup>();
        panelCanvas.alpha = 0f;
        panelCanvas.blocksRaycasts = false;

        sprintCanvasGroup.alpha = 1f; //this makes element visible
        
        //show hints for level 1
        if(GameStats.GetCurrentLevelNumber() == 1)
        {
           hintCanvas.alpha = 1f; 
        }

        audioPlayer.Play("Dreamscape1");   
    }

    public void DisplayGameInfo()
    {
        if(!gameInfoVisible)
        {
            gameInfo.gameObject.SetActive(true);
            gameKeys.gameObject.SetActive(true);  
        } else {
            gameInfo.gameObject.SetActive(false);
            gameKeys.gameObject.SetActive(false);  
        }
        gameInfoVisible = !gameInfoVisible;
    }

    void Update()
    {
        if(GameStats.IsGameOver()){return;}
        if (GameStats.GetCurrentLevelNumber() != 2)
        {
            return;
        }

        //showing the hint in ui for player distance from door
        float distance; //get distance from player object to door object
        GameObject playerPos = GameObject.FindGameObjectWithTag("Player");
        GameObject doorPos = GameObject.FindGameObjectWithTag("Door");
        distance = Vector3.Distance(playerPos.transform.position, doorPos.transform.position);
        //format number to desired decimals then display
        Text hintText = hint.GetComponent<Text>();
        hintText.text = distance.ToString("0.00");
    }
}
