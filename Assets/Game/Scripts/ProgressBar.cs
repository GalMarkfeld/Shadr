using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public RectTransform fill;
    private float total;
    private float startX;
    private float scaleAmount;

    // Start is called before the first frame update
    void Start()
    {
        total = endPosition.position.x - startPosition.position.x;
        startX = startPosition.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        scaleAmount = (startPosition.position.x - startX) / total;
        fill.localScale = new Vector3(scaleAmount, 1, 1);
        
      
    }
}
