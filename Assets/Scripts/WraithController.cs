using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithController : MonoBehaviour
{
    public Transform target;
    private float MoveSpeed = 10f;

       void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
         if(!GameStats.IsGameActive()){return;}
        float step = MoveSpeed * Time.deltaTime;

        Vector2 tempVector2 = Vector2.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime);
 
        transform.position = new Vector3(tempVector2.x, tempVector2.y, transform.position.z);
    }
}
