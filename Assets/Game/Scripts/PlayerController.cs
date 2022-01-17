using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private Animator _anim;

    // Player-specific
    [SerializeField] private LayerMask groundLayerMask;
    public float baseMoveSpd = 6.65f;
    private float jumpForce = 23f;
    private float wallJumpForce = 17f;
    private float gravity = 0.35f;
    public float maxFallSpd = 35f;
    private float playerJumpHoldFactor = 0.25f;  //higher makes the not holding lerp vspeed to 0 more quickly

    private Vector2 speedVec = new Vector2(0f, 0f);

    private float hinput = 0f;

    public bool startFacingLeft = false; // could be done directly through hinput, but I don't want to confuse by touching hinput more
    private bool grounded = false;
    private bool groundedPrev = false;
    private bool playerJump = false;        // tracks whether the player initialized a jump or not

    //Timers
    private int jinputCounter = 0;
    private int jumpBufferTime = 6;     // how long before a jump input exists
    private int coyoteCounter = 0;
    private int coyoteTime = 8;        // how many frames of leeway the player gets to jump after they shouldn't be able to



    // Color-related
    Color[] colors = { Color.black, Color.red };
    public SpriteRenderer renderer;
    int currentColor;

    // Subscribing to functions
    public static Action<bool> OnLevelKill = delegate { };
    public static Action OnLevelWin = delegate { };

    //Gal edit
    public static Action<String> NoticeUser = delegate { };


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {

        resetState();
        InputManager.OnRestart += resetState;
        //InputManager.OnJump += Jump;
        //InputManager.OnColorChange += ChangeColor;

    }

    private void Update()
    {
        bool jinput =       Input.GetKeyDown(KeyCode.Space);
        bool jinputHold =   Input.GetKey(KeyCode.Space);
        bool sinput =       Input.GetKeyDown(KeyCode.LeftShift);

        float hspeed = 0f;
        float vspeed = _rb.velocity.y;

        int wallDir = 0; // 0 for no touch, +/- 1 for right/left wall touch

        /*
        // Get raw inputs
        if (Input.GetKey(KeyCode.A))
        {
            hinput += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            hinput += 1f;
        }
        */

        // OVERRIDE!
        if (GlobalVar.isDead) hinput = 0f;
        // OVERRIDE!


        // Update timers
        if (jinputCounter > 0) jinputCounter--;
        if (jinput) jinputCounter = jumpBufferTime;

        if (coyoteCounter > 0) coyoteCounter--;



        hspeed = baseMoveSpd * hinput;

        vspeed -= gravity;


        groundedPrev = grounded;    
        grounded = groundCheck();   // check if any ground is being touched
        wallDir = wallTouchCheck(); // check if any walls are being touched

        if (grounded)
        {
            if (jumpBuffered())
            {
                coyoteCounter = coyoteTime;
            }
        }

        if (coyoteCounter > 0)
        {
            useCoyoteTime();
            vspeed = jumpForce;
            playerJump = true;
        }

        if (!grounded)
        {
            if (vspeed < -0.25) {
                playerJump = false;
            } else if (!jinputHold && playerJump)
            {
                vspeed = Mathf.Lerp(vspeed, 0, playerJumpHoldFactor);
            }

           if (wallDir != 0 && jumpBuffered())
            {
                useJumpInput();
                hinput *= -1;
                hspeed = baseMoveSpd * hinput;
                vspeed = wallJumpForce;
                playerJump = true;
            }
        }


        if (vspeed > 0) coyoteCounter = 0;  // stop them from abusing the leeway if they're already rising


        // Color Switch updates
        if (sinput)
        {
            ChangeColor();
        }


        // Cosmetic updates
        _anim.SetFloat("vspeed", vspeed);
        _anim.SetBool("grounded", grounded);


        // Physics Updates
        vspeed = Mathf.Max(vspeed, -maxFallSpd);
        speedVec = new Vector3(hspeed, vspeed);
        updateHeading();


        if (deathCheck()) OnLevelKill?.Invoke(false);

        _rb.velocity = speedVec;
    }

    private bool deathCheck()
    {
        return (transform.position.y < -6);
    }

    private bool jumpBuffered()
    {
        return (jinputCounter > 0);
    }

    private void useJumpInput()
    {
        jinputCounter = 0;
        return;
    }

    private void useCoyoteTime()
    {
        coyoteCounter = 0;
        useJumpInput();
    }

    private void FixedUpdate()
    {
 //       _rb.velocity = speedVec;
    }

    private void OnDestroy()
    {
        InputManager.OnRestart -= resetState;
        //InputManager.OnJump -= Jump;
        //InputManager.OnColorChange -= ChangeColor;
    }


    private bool groundCheck()
    {
        float distance = 1f;
        RaycastHit2D rc = Physics2D.BoxCast(_bc.bounds.center, new Vector2(_bc.size.x * 0.5f, _bc.size.y / 4), 0f, Vector2.down, distance, groundLayerMask);
        return (rc.collider != null);
    }

    private int wallTouchCheck()
    {
        float distance = 1f;
        RaycastHit2D rc = Physics2D.BoxCast(_bc.bounds.center, new Vector2(_bc.size.x, _bc.size.y / 4), 0f, new Vector2(Math.Sign(hinput), 0), distance, groundLayerMask);
        if (rc.collider == null) return 0;
        return (Math.Sign(rc.transform.position.x - transform.position.x));
    }

    private void updateHeading()
    {
        if (Math.Sign(hinput) > 0)
        {
            renderer.flipX = false;
        } else
        {
            renderer.flipX = true;
        }
    }


    private void resetState()
    {

        currentColor = 0;
        renderer.material.color = colors[currentColor];

        // Heading
        hinput = startFacingLeft ? -1f : 1f;
        updateHeading();
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
        if(collision.gameObject.tag == "moving_palform")
        {
            Color obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().material.color;
            Debug.Log("this is obstacle: "+obstacleColor);
            if (colors[currentColor] != obstacleColor)
            {
                Debug.Log(colors[currentColor]);
                GlobalVar.isDead = true;
                OnLevelKill?.Invoke(true);
            }
        }
        else if (collision.gameObject.tag == "obstacle")
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
            Debug.Log("inside player controller");
            OnLevelWin?.Invoke();
            GlobalVar.isDead = true;
        } 
        //Gal edit: invoke event with proper trigger tag                
        else if (collision.gameObject.tag.Contains("Notice"))
        {
            NoticeUser?.Invoke(collision.gameObject.tag);
        }
    }


}
