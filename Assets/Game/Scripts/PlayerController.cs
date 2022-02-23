using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private Animator _anim;
    [SerializeField] private ParticleSystem _ps_dust;
    [SerializeField] private ParticleSystem _ps_dust_WJ;

    // Player-specific
    [SerializeField] private LayerMask groundLayerMask;
    public float baseMoveSpd = 6.65f;
    private float curMoveSpd = 0f;
    private float jumpForce = 21f;
    private float wallJumpForce = 17f;
    private float gravity = 0.35f;
    public float maxFallSpd = 35f;
    private float playerJumpHoldFactor = 0.25f;  //higher makes the not holding lerp vspeed to 0 more quickly

    private float jumpPadForce = 40f;
    private float jumpPadForceLarge = 40f;//55f;

    private float SpeedBoostLossFactor = 0.005f;

    private Vector2 speedVec = new Vector2(0f, 0f);
    private float holdOverVSpeed = 0f;

    private float hinput = 1f;

    public bool startFacingLeft = false; // could be done directly through hinput, but I don't want to confuse by touching hinput more
    private bool grounded = false;
    private bool groundedPrev = false;
    private bool playerJump = false;        // tracks whether the player initialized a jump or not
    private int wallDir = 0; // 0 for no touch, +/- 1 for right/left wall touch
    private int wallDirPrev = 0;

    //Timers
    private int jinputCounter = 0;
    private int jumpBufferTime = 6;     // how long before a jump input exists
    private int coyoteCounter = 0;
    private int coyoteTime = 8;        // how many frames of leeway the player gets to jump after they shouldn't be able to

    private int SpeedBoostMaintainCounter = 0;
    private int SpeedBoostMaintainTime = 60 * 3;


    // Color-related
    //Color[] colors = { Color.black, Color.red };
    [SerializeField] private SpriteRenderer renderer;
    private Transform spriteAnchor;

    int currentColor;
    //gal edit:
    public Color[] colors = new Color[2];
    public Color[,] colorOptions = {    { Color.red, Color.black },
                                        { new Color(87/255f,66/255f,245/255f), new Color(245/255f,99/255f,66/255f) },                                        
                                        { new Color(214/255f,41/255f,155/255f), new Color(41/255f,214/255f,100/255f) },
                                        { new Color(255/255f,97/255f,165/255f), new Color(61/255f,13/255f,255/255f) },
                                        { new Color(97/255f,255/255f,187/255f), new Color(149/255f,10/255f,255/255f) },
                                        { new Color(105/255f,154/255f,219/255f), new Color(96/255f,184/255f,121/255f) },
                                        };
    GameObject[] obstacles;


    // Other Visual-related
    private float scaleLerpFactor = 0.09f;



    // Subscribing to functions
    public static Action<bool> OnLevelKill = delegate { };
    public static Action OnLevelWin = delegate { };

    //Gal edit
    public static Action<String> NoticeUser = delegate { };


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();        // we use the InChildren variant since we separated the sprite into a child of the player for Squash/Stretch effect
        _bc = GetComponent<BoxCollider2D>();
        spriteAnchor = transform.Find("SpriteAnchor");
        _anim = spriteAnchor.Find("Sprite").GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {

        resetState();

        InputManager.OnRestart += resetState;

        curMoveSpd = baseMoveSpd;

    }

    private void Update()
    {
        // Begin each frame by getting inputs and carrying over relevant velocities and visual data
        bool jinput =       Input.GetKeyDown(KeyCode.Space);
        bool jinputHold =   Input.GetKey(KeyCode.Space);
        bool sinput =       Input.GetKeyDown(KeyCode.LeftShift);

        float hspeed = 0f;
        float vspeed = _rb.velocity.y;

        vspeed += holdOverVSpeed;           // add any vertical velocity gained between updates (don't want to mix addForce and this custom stuff)
        holdOverVSpeed = 0;

        Vector3 spriteScale = spriteAnchor.localScale;


        // Unused player input
        #region
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
        #endregion


        // Input override if the player has lost
        if (GlobalVar.isDead) hinput = 0f;

        // Update timers
        if (jinputCounter > 0) jinputCounter--;
        if (jinput) jinputCounter = jumpBufferTime;

        if (coyoteCounter > 0) coyoteCounter--;
        if (SpeedBoostMaintainCounter > 0) SpeedBoostMaintainCounter--;
        // Update horizontal speed

        if (SpeedBoostMaintainCounter <= 0) {
            curMoveSpd = Mathf.Lerp(curMoveSpd, baseMoveSpd, SpeedBoostLossFactor);
        }
        
        groundedPrev = grounded;    
        grounded = groundCheck();   // check if any ground is being touched
        wallDirPrev = wallDir;
        wallDir = wallTouchCheck(); // check if any walls are being touched

        bool wallSliding = wallDir != 0 && !jumpBuffered() && (vspeed < 0);

        hspeed = curMoveSpd * hinput;
        vspeed -= gravity * (wallSliding? 0.5f : 1f);



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
            setScale(new Vector3(0.5f, 1.3f));
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

                makeWJDust();
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
        _anim.SetBool("wallSliding", wallSliding);

        if (!groundedPrev && grounded)        // if the player has just landed
        {
            makeDust();
            setScale(new Vector3(1.3f, 0.5f));
        }

        if (hinput != 0)
        {
            updateHeading();
            setScale(Vector3.Lerp(spriteAnchor.localScale, new Vector3(Math.Sign(hinput), 1f, 1f), scaleLerpFactor));
        }

        // Physics Updates
        vspeed = Mathf.Max(vspeed, -maxFallSpd);
        speedVec = new Vector3(hspeed, vspeed);
        
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
        RaycastHit2D rc = Physics2D.BoxCast(_bc.bounds.center, new Vector2(_bc.size.x * 0.15f, _bc.size.y / 5), 0f, Vector2.down, distance, groundLayerMask);
        return (rc.collider != null);
    }

    private int wallTouchCheck()
    {
        float distance = 1f;
        RaycastHit2D rc = Physics2D.BoxCast(_bc.bounds.center, new Vector2(_bc.size.x, _bc.size.y / 4), 0f, new Vector2(Math.Sign(hinput), 0), distance, groundLayerMask);
        if (rc.collider == null) return 0;
        return (Math.Sign(rc.transform.position.x - transform.position.x));
    }

    private void setScale(Vector3 newScale)
    {
        spriteAnchor.localScale = newScale;
    }

    private void updateHeading()
    {
        Vector3 scale = spriteAnchor.localScale;
        scale.x = Math.Abs(scale.x) * Math.Sign(hinput);
        setScale(scale);
    }


    private void resetState()
    {

        ////////////////////////////  gal edit  ///////////////////////////////////////

        int currentColorOptions = Random.Range(0, colorOptions.GetLength(0) -1);
        
        colors[0] = colorOptions[currentColorOptions,0];
        colors[1] = colorOptions[currentColorOptions,1];


        obstacles = GameObject.FindGameObjectsWithTag("obstacle0");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<SpriteRenderer>().color = colors[0];
        }

        obstacles = GameObject.FindGameObjectsWithTag("obstacle1");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<SpriteRenderer>().color = colors[1];
        }

        obstacles = GameObject.FindGameObjectsWithTag("moving_palform");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<movingPlatform>().colors = colors;
            int startingColor = obstacle.GetComponent<movingPlatform>().StartColor;
            obstacle.GetComponent<SpriteRenderer>().material.color = colors[startingColor];

        }
        
                             
        /////////////////////////////////////////////////////////////////////////

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
        //print("there was a collision\n");

        Color obstacleColor;
        switch (collision.gameObject.tag)
        {
            case "moving_palform":
                obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().material.color;
                Debug.Log("this is obstacle: " + obstacleColor);
                if (colors[currentColor] != obstacleColor)
                {
                    Debug.Log(colors[currentColor]);
                    GlobalVar.isDead = true;
                    OnLevelKill?.Invoke(true);
                }

                break;

            case "obstacle0":                
                obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().color;               
                if (colors[currentColor] != obstacleColor)
                {
                    GlobalVar.isDead = true;
                    OnLevelKill?.Invoke(true);
                }
                print("obs");
                break;

            case "obstacle1":
                obstacleColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
                if (colors[currentColor] != obstacleColor)
                {
                    GlobalVar.isDead = true;
                    OnLevelKill?.Invoke(true);
                }
                print("obs");
                break;


            case "jump_pad":
                
                bool jHold = Input.GetKey(KeyCode.Space);
                holdOverVSpeed = jumpPadForce; // jHold ? jumpPadForce : jumpPadForceLarge;

                print("jump pad");
                break;

            case "speed_boost":

                SpeedBoosterManage booster = collision.gameObject.GetComponent<SpeedBoosterManage>();
                if (booster.cooldownCounter <= 0) {
                    curMoveSpd *= 1.4f;
                    SpeedBoostMaintainCounter = SpeedBoostMaintainTime;

                    booster.cooldownCounter = booster.cooldownTime;
                }

                break;
            

            case "Floor":
                OnLevelKill?.Invoke(false);

                break;

            case "End":
                Debug.Log("inside player controller");
                OnLevelWin?.Invoke();
                GlobalVar.isDead = true;

                break;

            case "laser":        
                    Debug.Log(colors[currentColor]);
                    GlobalVar.isDead = true;
                    OnLevelKill?.Invoke(true);              

                break;
        }


        


        //Gal edit: invoke event with proper trigger tag                
        if (collision.gameObject.tag.Contains("Notice"))
        {
            NoticeUser?.Invoke(collision.gameObject.GetComponent<Notice>().text);
        }
    }


    private void makeDust()
    {
        _ps_dust.Play();
    }

    private void makeWJDust()
    {
    //    int dir = Math.Sign(direction);
  //      var settings = _ps_dust_WJ.main;
//        settings.startSpeed = dir;
        _ps_dust_WJ.Play();
    }




}
