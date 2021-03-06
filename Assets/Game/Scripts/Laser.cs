using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int laserSpeed;
    public int laserColor; //0 for red, 1 for black
    public int laserLength; 
    public SpriteRenderer renderer;
    private Color[] colors = { Color.red, Color.black };
    public Transform player;
    public float distanceFromLaser = 10;

    private void Start()
    {
        renderer.material.color = colors[laserColor]; //init laser to desired color
    }

    // Update is called once per frame
    void Update()
    {
        //extend laser when player is near
        if(Mathf.Abs(player.position.x - transform.position.x ) < distanceFromLaser)
        {
            //if the current laser length is smaller than the desired length
            if (transform.localScale.y < laserLength)
            {
                transform.localScale += new Vector3(0, laserSpeed * Time.deltaTime, 0);
            }

        }


    }
}
