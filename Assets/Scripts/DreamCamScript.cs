using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCamScript : MonoBehaviour
{
    public GameObject cam;
    public Transform playerLocation;

    void Update()
    {
        Vector3 pos = cam.transform.position;
        Vector3 playerPos = playerLocation.position;

        pos.x = playerPos.x;
        pos.y = playerPos.y;

        cam.transform.position = pos;
    }
}
