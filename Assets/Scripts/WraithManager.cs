using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithManager : MonoBehaviour
{
    private float countdown;
    private Transform target; 
    private CollisionScript col;
    private WraithController wraithCtrl;

    public float wraithCountdown = 3f;
    public Transform[] spawnPoints;
    public GameObject wraithPrefab; 
    public GameObject gameManager;

    void Start()
    {
        if (GameStats.GetCurrentLevelNumber() == 4)
        {
            wraithCountdown = 10f;
        }
        countdown = wraithCountdown;
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if(!GameStats.IsGameActive()){return;}
        countdown -= Time.fixedDeltaTime;
        if(countdown <= 0f)
        {
            Spawn();
            countdown = wraithCountdown;
        }
    }
    
    void Spawn()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {   
            float num = Random.Range(0f,1f);

            if(num > .5f)
            {
                //the Euler rotates the sprite so they're facing the overhead cam
                GameObject wraith = Instantiate(wraithPrefab, spawnPoint.position, Quaternion.Euler(new Vector3(0, 90, -32)));
            }
        }
    }   
}
