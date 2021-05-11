using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float countdown = .25f;
    private int index = 0;
    private int repeatCount = 0;
    private int dreamBubbleCount = 0;
    private int currentLevel;
    private bool hasPlayed;
    private bool hasBeenCalled;

    public Sprite[] tiredSprites;  
    public Image panelImage;
    public GameObject staticUI;
    public GameObject[] dreamBubbles;
    public float countdownSet;

    void Start()
    {
        currentLevel = GameStats.GetCurrentLevelNumber();
        countdown = countdownSet;
        hasPlayed = GameStats.GetHasPlayed();
    }

    //Handle the opening scene animation
    void Update()
    {
        if(hasPlayed && !hasBeenCalled)
        {
            CanvasGroup gpCanvas = panelImage.gameObject.GetComponent<CanvasGroup>();
            gpCanvas.alpha = 0f;
            gpCanvas.blocksRaycasts = false;
            //call HideStartMenuButtons from StaticUI
            GameObject staticUI = GameObject.FindGameObjectWithTag("StaticUI");
            StaticUI ui = staticUI.GetComponent<StaticUI>();
            ui.HideStartMenuButtons();
            hasBeenCalled = true;
            return;
        }

        if(dreamBubbleCount == 3){ return;}

        //play yawn sound
        if(repeatCount == 1)
        {
            FindObjectOfType<AudioManager>().Play("Yawn");
        }

        if(repeatCount == 3)
        {
            //in here we are finished with the sleeping animation - character is asleep
            //show dream bubbles
             if(countdown <= 0f)
             {
                dreamBubbles[dreamBubbleCount].SetActive(true);
                dreamBubbleCount += 1;  
                countdown -= Time.deltaTime;
                Mathf.Clamp(countdown, 0, Mathf.Infinity);
                countdown = countdownSet;
                return;
             }
        }

        if (index >= 5) 
        {
            index = 0;
            repeatCount +=1;
            return;
        }

         if(countdown <= 0f && !GameStats.IsGameOver())
         {
             //set panel background image
            panelImage.sprite = tiredSprites[index];
            //reset panel
            countdown = countdownSet;
            //increment index
            index += 1;
        }
        //continue countdown
        countdown -= Time.deltaTime;
        //prevent odd numbers
        Mathf.Clamp(countdown, 0, Mathf.Infinity);
    }
}