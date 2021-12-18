using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{


    private Rigidbody2D _rb;
    public float jumpForce;
    Color[] colors = { Color.black, Color.grey, Color.red };
    public SpriteRenderer renderer;
    int currentColor;


    public static Action<bool> OnLevelKill = delegate { };
    public static Action OnLevelWin = delegate { };

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentColor = 0;
        InputManager.OnJump += Jump;
        InputManager.OnColorChange += ChangeColor;
      
    }

    private void OnDestroy()
    {
        InputManager.OnJump -= Jump;
        InputManager.OnColorChange -= ChangeColor;
    }


    private void Jump()
    {

        
        //_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        //game config option
        _rb.AddForce(GameManager.inst.gameConfig.jumpForce * Vector3.up, ForceMode2D.Impulse);


    }

    private void ChangeColor()
    {

        

        ++currentColor;

        if (currentColor == 3)
        {
            currentColor = 0;
        }
       

        renderer.material.color = colors[currentColor];

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    // if (collision.gameObject.tag == "obstacle")
    //    {
    //        Color obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
    //        if (colors[currentColor] != obstacleColor )
    //        {
    //            print("player color:" + colors[currentColor]);
    //            print("obstacle color: " + obstacleColor);

    //            print("wrong color kill");
    //            OnLevelKill?.Invoke();
    //        }
    //    }
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            Color obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            if (colors[currentColor] != obstacleColor)
            {
                
                OnLevelKill?.Invoke(true);
            }
        } else if(collision.gameObject.tag == "Floor")
        {
            OnLevelKill?.Invoke(false);
        } else if (collision.gameObject.tag == "End")
        {
            OnLevelWin?.Invoke();
        }
    }


}
