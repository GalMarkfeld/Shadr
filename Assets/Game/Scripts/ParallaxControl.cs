using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxControl : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxFactor;

    private void Awake()
    {
//        InputManager.OnRestart += resetStartPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        float movementRelativeToCam = (cam.transform.position.x * (1 - parallaxFactor));
        float distance = (cam.transform.position.x * parallaxFactor);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // could shorten this with absolute value
        if (movementRelativeToCam > startPos + length) startPos += length;
        else if (movementRelativeToCam < startPos - length) startPos -= length;
    }
}
