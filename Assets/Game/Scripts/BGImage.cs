using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGImage : MonoBehaviour
{

    void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        transform.position = Camera.main.transform.position + Vector3.forward * this.transform.position.z;

        transform.localScale = new Vector3(1, 1, 1);
        Vector3 lossyScale = transform.lossyScale;

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;

        Vector3 newLocalScale = new Vector3(transform.localScale.x / lossyScale.x,
            transform.localScale.y / lossyScale.y,
            transform.localScale.z / lossyScale.z
        );

        transform.localScale = newLocalScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
