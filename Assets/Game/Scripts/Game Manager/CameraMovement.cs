using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //public float camera_speed = 1;

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(camera_speed * Time.deltaTime, 0);
        transform.position += new Vector3(GameManager.inst.gameConfig.cameraSpeed * Time.deltaTime, 0);
    }
}
