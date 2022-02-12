using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowingLaser : MonoBehaviour
{
    public int laserSpeed;
    public int laserColor; //0 for red, 1 for black
    public int laserLength; 
    public SpriteRenderer renderer;
    private Color[] colors = { new Color(64,0,0,1), new Color(0,64,0,1) }; //64 for strong intensity. otherwise wouldnt glow
    public Transform player;
    public float distanceFromLaser = 10;

    private void Start()
    {

       
        renderer.material.EnableKeyword("_EMISSION");
        renderer.material.SetColor("_EmissionColor", colors[laserColor]);

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
