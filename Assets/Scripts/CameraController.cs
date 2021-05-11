using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    public GameObject[] cameras;

    private int currentLevel;

    void Awake()
    {
        currentLevel = GameStats.GetCurrentLevelNumber();
        // if(GameStats.IsGameOver())
        // {
        //     //ensure we are showing scared wake up image and title
        //     GameObject gp = GameObject.FindGameObjectWithTag("GamePanel");
        //     if(gp != null)
        //     {
        //         CanvasGroup gpCanvas = gp.GetComponent<CanvasGroup>();
        //         gpCanvas.alpha = 1f;
        //     } else 
        //     {
        //         Debug.LogWarning("game panel is null in CamController");
        //     }
        // }
    }

    void Start()
    {
        //initial load should go to IntroCamera (working)
        //retry should load the current level's camera (working)
        //ending game should load IntroCamera (working)
        //loading from end game cut scene loads dream1camera

        if(GameStats.GetStartingFromEnd()){
            Debug.Log("Dream1Camera because starting from end");
            SetActiveCamera("Dream1Camera");
            return;
        }

        if (currentLevel == 1 && !GameStats.GetHasPlayed())
        {
            Debug.Log("Intro Camera becuase currentlevel = 1 and GameHasPlayed is false");
           foreach(GameObject camera in cameras)
            {
                CamScript cs = camera.GetComponent<CamScript>();

                if (cs.GetCameraName() == "IntroCamera")
                {
                    camera.SetActive(true);
                } else {
                    camera.SetActive(false);
                }
            }
         } else if(!GameStats.IsGameOver())
            {
                Debug.Log("Dream1Camera because game is not over");
                SetActiveCamera("Dream1Camera");
            } else {
                Debug.Log("IntroCamera because game is over");
                SetActiveCamera("IntroCamera");
            }
    }

    //switch on desired camera, switch off all others
    public void SetActiveCamera(string cameraName)
    {
        Debug.Log("Setting active camera to " + cameraName);

        foreach (GameObject camera in cameras)
        {
            CamScript cs = camera.GetComponent<CamScript>();
    
            if(cs.GetCameraName() == cameraName)
            {
                camera.SetActive(true);
            } else {
                camera.SetActive(false);
            }
        }
    }

}
