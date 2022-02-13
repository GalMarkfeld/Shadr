using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowingLaser : MonoBehaviour
{
    public float laserSpeed;
    public int laserColor; //0 for red, 1 for black
    public float laserLength; 
    public SpriteRenderer renderer;
    //public Color[] colors = { Color.black, Color.red }; //64 for strong intensity. otherwise wouldnt glow
    //public Color[] colors = new Color[2];
    public Transform player;
    public float distanceFromLaser = 10;


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
