using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoosterManage : MonoBehaviour
{
    public int cooldownTime = 20;         // how long the player must wait between using this speed booster again
    public int cooldownCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTime > 0) cooldownTime--;
    }
}
