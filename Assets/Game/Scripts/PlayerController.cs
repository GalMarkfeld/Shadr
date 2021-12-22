using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{


    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private Animator _anim;

    // Player-specific
    [SerializeField] private LayerMask groundLayerMask;
    private float baseMoveSpd = 6.5f;
    private float jumpForce = 18f;
    private float gravity = 0.4f;
    private float maxFallSpd = 35f;

    private Vector2 speedVec = new Vector2(0f, 0f);

    private bool grounded = false;
    private bool groundedPrev = false;

    
    // Color-related
    Color[] colors = { Color.black, Color.red };
    public SpriteRenderer renderer;
    int currentColor;

    // Subscribing to functions
    public static Action<bool> OnLevelKill = delegate { };
    public static Action OnLevelWin = delegate { };

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentColor = 0;
        //InputManager.OnJump += Jump;
        //InputManager.OnColorChange += ChangeColor;
      
    }

    private void Update()
    {
        float hinput = 0;
        bool jinput = Input.GetKeyDown(KeyCode.Space);
        bool sinput = Input.GetKeyDown(KeyCode.LeftShift);

        float hspeed = 0f;
        float vspeed = _rb.velocity.y;

        // Get raw inputs
        if (Input.GetKey(KeyCode.A))
        {
            hinput += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            hinput += 1f;
        }


        // OVERRIDE!

        hinput = 1f;
        if (GlobalVar.isDead) hinput = 0f;
        // OVERRIDE!

        hspeed = baseMoveSpd * hinput;

        if (grounded && jinput)
        {
            vspeed = jumpForce;
        }

        vspeed -= gravity;
        vspeed = Mathf.Max(vspeed, -maxFallSpd);

        if (jinput && grounded)
        {
            vspeed = jumpForce;
            //_rb.AddForce(GlobalVar.jumpForce * Vector3.up, ForceMode2D.Impulse);
        }

        groundedPrev = grounded;
        grounded = groundCheck();

        if (sinput)
        {
            ChangeColor();
        }

        // Cosmetic updates
        _anim.SetFloat("vspeed", vspeed);
        _anim.SetBool("grounded", grounded);

        speedVec = new Vector3(hspeed, vspeed);


        if (deathCheck()) OnLevelKill?.Invoke(false);

    }

    private bool deathCheck()
    {
        return (transform.position.y < -6);
    }

    private void FixedUpdate()
    {
        _rb.velocity = speedVec;
    }

    private void OnDestroy()
    {
        //InputManager.OnJump -= Jump;
        //InputManager.OnColorChange -= ChangeColor;
    }


    private bool groundCheck()
    {
        float epsilon = 1f;
        RaycastHit2D rc = Physics2D.BoxCast(_bc.bounds.center, new Vector2(_bc.size.x * 2, _bc.size.y / 4), 0f, Vector2.down, epsilon, groundLayerMask);
        return (rc.collider != null);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Draw a cube at the maximum distance
        //Gizmos.DrawWireCube(_bc.bounds.center - new Vector3(0, 1f), _bc.size/8);
    }


    private void ChangeColor()
    {
        currentColor = (currentColor + 1) % colors.Length;

        renderer.material.color = colors[currentColor];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            Color obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            if (colors[currentColor] != obstacleColor)
            {
                GlobalVar.isDead = true;
                OnLevelKill?.Invoke(true);                
            }
        } else if(collision.gameObject.tag == "Floor")

        {
            OnLevelKill?.Invoke(false);
        } else if (collision.gameObject.tag == "End")
        {
            OnLevelWin?.Invoke();
            GlobalVar.isDead = true;
        }
    }


}
