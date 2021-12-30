using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    private int i;


    // Start is called before the first frame update
    void Start()
    {
        //transform.position = points[startingPoint].position;
        i = startingPoint;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Vector2.Distance(transform.position, points[i].position) < 0.1f)
        {
           
            if (++i == points.Length) 
            {
               
                i = 0;
            }         

        }

        Debug.Log(points[i].position);

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}
