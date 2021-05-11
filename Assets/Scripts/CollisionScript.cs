using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private bool isBright = false;
    private bool gameOver = GameStats.IsGameOver();
    private AudioManager audioPlayer;
    private float radius = 30.0F;
    private float power = 300.0F;
    private int currentLevel;

    private GameObject redCheckImg;
    private GameObject whiteCheckImg;
    private GameObject blueCheckImg;

    public SceneHelper sceneHelper;
    public CameraController camCtrl;
    public GameObject Lights;
    public GameObject orb;
    public Light lightItem;
    public string[] suspenseSounds;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioManager>();
        currentLevel = GameStats.GetCurrentLevelNumber();

        if(currentLevel == 4)
        {
            redCheckImg = GameObject.FindGameObjectWithTag("RedCheck");
            whiteCheckImg = GameObject.FindGameObjectWithTag("WhiteCheck");
            blueCheckImg = GameObject.FindGameObjectWithTag("BlueCheck");
        }
    }

   void OnCollisionEnter(Collision collision)
   {
        GameObject obj1 = this.gameObject;
        GameObject obj2 = collision.gameObject;

        //Debug.Log("Triggered Obj1: name:" + obj1.name + " tag: " + obj1.tag);
        //Debug.Log("Triggered obj2: name:" + obj2.name + " tag: " + obj2.tag);

       if(obj1.tag == "LightOrb" && obj2.tag == "Player")
       {
            lightItem.intensity = 3;
            lightItem.range = 1000;
            isBright = true;
            audioPlayer.Play("Light");
            
            // repel wraiths here
            Vector3 explosionPos = obj1.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if(rb.gameObject.tag == "Wraith")
                    {
                        rb.AddExplosionForce(power, explosionPos, radius, 1.0F, ForceMode.VelocityChange);  
                    }   
                }
            }
       } else if(obj1.tag == "Wraith" && obj2.tag == "Player")
       {

            if(GameStats.IsGameOver()){return;}

            GameStats.EndGameFinale();
            
        //END LEVEL SCENARIO
       } else if(obj1.tag == "Door" && obj2.tag == "Player")
       {
           audioPlayer.Play("Door");
           
            int nextLevel = currentLevel + 1;
            GameStats.AddLevelPassed(1);
           GameStats.SetCurrentLevel(nextLevel);
            
           //switch camera and instantiate scene
            sceneHelper.ActivateDreamscape(GameStats.GetCurrentLevelNumber());
       } else if (obj1.tag == "WaterSpout" && obj2.tag == "Player")
       {

           var part = obj1.GetComponent<ParticleSystem>().emission;
           part.enabled = true; // Applies the new value directly to the Particle System
            GameObject player= GameObject.FindGameObjectWithTag("Player");
            CharController cc = player.GetComponent<CharController>();
            cc.ResetSprintCooldown();
       }
       //level 4 RED WHITE BLUE puzzle
       if(currentLevel == 4){

            bool redCheck = GameStats.GetRedCheck();
            bool whiteCheck = GameStats.GetWhiteCheck();
            bool blueCheck = GameStats.GetBlueCheck();

            if(obj1.tag == "RedOrb" && obj2.tag == "Player")
            {
               GameStats.SetRedCheck(true);
               GameStats.SetWhiteCheck(false);
               GameStats.SetBlueCheck(false);

               Debug.Log("TOUCHED RED ORB. redCheck =  "+redCheck + ". whiteCheck = "+whiteCheck + ". blueCheck = " + blueCheck);
               redCheckImg.GetComponent<CanvasGroup>().alpha = 1f;
               whiteCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
               blueCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
            }
            if (obj1.tag == "WhiteOrb" && obj2.tag == "Player")
            {
               Debug.Log("TOUCHED WHITE ORB. redCheck =  "+redCheck + ". whiteCheck = "+whiteCheck + ". blueCheck = " + blueCheck);
               if(redCheck && !whiteCheck)
               {
                    GameStats.SetWhiteCheck(true);
                    redCheckImg.GetComponent<CanvasGroup>().alpha = 1f;
                    whiteCheckImg.GetComponent<CanvasGroup>().alpha = 1f;
                    blueCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
               } else 
               {
                    GameStats.SetRedCheck(false);
                    GameStats.SetWhiteCheck(false);
                    redCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
                    whiteCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
                    blueCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
               }
            }
            if (obj1.tag == "BlueOrb" && obj2.tag == "Player")
            {
                Debug.Log("TOUCHED BLUE ORB. redCheck =  "+redCheck + ". blueCheck = "+blueCheck + ". whiteCheck = "+whiteCheck);
               if (redCheck && whiteCheck)
               {
                    GameStats.SetBlueCheck(true);
                    redCheckImg.GetComponent<CanvasGroup>().alpha = 1f;
                    whiteCheckImg.GetComponent<CanvasGroup>().alpha = 1f;
                    blueCheckImg.GetComponent<CanvasGroup>().alpha = 1f;
                    audioPlayer.Play("Door");
                    Debug.LogWarning("Winner winner chicken dinner!!");
                    GameStats.AddLevelPassed(1);
                    //End GAME! WINNER
                    GameStats.EndGameFinale();
               } else 
               {
                    GameStats.SetRedCheck(false);
                    GameStats.SetWhiteCheck(false);
                    GameStats.SetBlueCheck(false);
                    redCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
                    whiteCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
                    blueCheckImg.GetComponent<CanvasGroup>().alpha = 0f;
               }
            }
       }
   }

   void OnTriggerEnter(Collider other)
   {
        GameObject obj1 = this.gameObject;
        GameObject obj2 = other.gameObject;

        //Debug.Log("Triggered Obj1: name:" + obj1.name + " tag: " + obj1.tag);
        //Debug.Log("Triggered obj2: name:" + obj2.name + " tag: " + obj2.tag);

        if(obj1.tag == "Wraith" && obj2.tag == "Warning")
        {
            int randInt = Random.Range(0,5);
            audioPlayer.Play(suspenseSounds[randInt]);
        }
   }

   void FixedUpdate()
   {
       if(!isBright)
       {
           CancelInvoke();
           return;
        }

       InvokeRepeating("LowerBrightness", 5.0f, 0.5f);
   }

   void LowerBrightness()
   {

       if(lightItem.range > 100)
       {          
            lightItem.range -= 20;
       } else {
           isBright = false;
           lightItem.range = 100;
           lightItem.intensity = 1;
       }  
   }
}
