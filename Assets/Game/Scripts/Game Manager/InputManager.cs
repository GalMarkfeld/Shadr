using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action OnJump = delegate { };
    public static Action OnColorChange = delegate { };
    public static Action OnRestart = delegate { };


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnColorChange?.Invoke();
            
        } 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
            
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnRestart?.Invoke();
        }
    }
}
