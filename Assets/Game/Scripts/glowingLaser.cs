using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowingLaser : MonoBehaviour
{
    public float laserSpeed;
    public int laserColor; //0 for red, 1 for black
    public float laserLength; 
    public SpriteRenderer renderer;
    private Color[] colors = { Color.black, Color.red }; //64 for strong intensity. otherwise wouldnt glow
    public Transform player;
    public float distanceFromLaser = 10;

    private void Start()
    {

        renderer.material.color = colors[laserColor];

        //emission is required for glow, emission color must have intensity
        renderer.material.EnableKeyword("_EMISSION"); //enable the change in emission
        renderer.material.SetColor("_EmissionColor", colors[laserColor] * Mathf.Pow(2,10)); //the exponent is the intensity of the color

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
