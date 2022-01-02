using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{   
    //moving platforms
    public float speed; //platfrom speed
    public int startingPoint; //starting point
    public Transform[] points; //array of points
    private int i; //point index

    public int StartColor; //0 for red, 1 for black

    //change color
    public bool switchOn; //on if we switch colors
    public bool useCustomColor; // true if the spriteRenderer is given a different color beforehand
    public float changeSpeed; //switch speed
    public SpriteRenderer renderer; 
    private Color[] colors = { Color.red, Color.black };
    private int j; //color index


    // Start is called before the first frame update
    void Start()
    {           
        i = startingPoint; //init i to the start color

        renderer.material.color = colors[StartColor]; // change platfrom to start color

        //color switching
        j = StartColor; // init j for for first color
        if (switchOn) //if we want to switch colors
        {
            InvokeRepeating("changePlatform", 0.01f, changeSpeed);//call change platform from 0.01s in changespeed interval
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the distantce from the platform to the dsetination point is small enough
        if (Vector2.Distance(transform.position, points[i].position) < 0.1f)
        {
            //move to the next platform
            i = (i + 1) % points.Length;

        }

       
        //move towards the destination point
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        if (useCustomColor) renderer.material.color = Color.white;
    }


    private void changePlatform()
    {
        //change color index cyclic
        j = (j + 1) % colors.Length;

        //change color
        renderer.material.color = colors[j];

    }


}



