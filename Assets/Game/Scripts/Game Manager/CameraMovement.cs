using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //public float camera_speed = 1;
    [SerializeField] private GameObject player;

    // Update is called once per frame
    void LateUpdate()
    {
        // Interpolate towards the player's position
        transform.position = player.transform.position;
        //transform.position = Vector2.Lerp(player.transform.position, transform.position, GlobalVar.cameraFollowFactor);

        //transform.position += new Vector3(camera_speed * Time.deltaTime, 0);
        //transform.position += new Vector3(GameConfig.cameraSpeed * Time.deltaTime, 0);
    }
}
