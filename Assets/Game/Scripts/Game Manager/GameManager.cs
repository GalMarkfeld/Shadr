using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menusCanvas;
    public int menuOffset = 420;
    //[SerializeField]
    public GameObject killMenuObj;
    //public GameObject mainCanvasObj;

    public GameObject[] respawns;
    public static GameManager inst;
    
    //public RunnerGameConfig gameConfig;
    //public Transform startPos;

    [Header("UI Reference")]
    public Text obstacleWrongColorKill;
    //Gal edit: made instructions into 1 text that changes
    public Text Notice;
    public Text restartText;

    public bool avoidTextPrompts;

    //public Text scoreText;
    //public Text highScoreText;

    [Header("Levels")]
    public int currentLevel = 0;
    //public List<Level> levels = new List<Level>();

    [Space]
    public DeathMenu theDeathScreen;
    public WinMenu winScreen;
    public GameObject pauseButton;
    //public UnityEvent LevelFinishedEvent;

    //public static Action<Level> OnLevelStart = delegate { };

    

    //private int _score;
    //private int _highScore;
    private int _level = 0;
    public Transform startPosition;

    private Level _currentLevel;

    public static Action onLevelRestart = delegate { };

    void Awake()
    {
        #region Singleton
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Debug.LogWarning("More then one GameManger was created");
            Destroy(gameObject);
        }
        #endregion

        startPosition = transform;

        PlayerController.OnLevelKill += killPlayer;
        PlayerController.OnLevelWin += playerWon;
        InputManager.OnRestart += restartGame;

        //Gal edit
        PlayerController.NoticeUser += setNotice;
        //PlayerController.onLevelRestart += 

        //killMenuObj = GameObject.FindGameObjectsWithTag("kill_menu")[0];
        //mainCanvasObj = GameObject.FindGameObjectsWithTag("main_canvas")[0];
        //Hide kill menu

       // menusCanvas = GameObject.Find("Canvas-Menus");

        //menusCanvas.transform.position += new Vector3(0, 0, menuOffset);
        //ActivateMenu(false);
        //killMenuObj.SetActive(false);
        /*respawns = GameObject.FindGameObjectsWithTag("kill_menu");
        foreach (GameObject respawn in respawns)
        {
            print("setting dont destroy");
            DontDestroyOnLoad(respawn);
        }*/
    }

    private void Update()
    {
        
    }

    //Gal edit
    private void setNotice(String text)
    {
        
        Debug.Log("In set notice");
        Notice.text = text;
        

        // Roey updated
        if (avoidTextPrompts)
        {
            Debug.Log("Roey update - ignoring text in non-tutorial level!");
            clearText(Notice);
            disableText();

        }
    }

    private void Start()
    {
        restartGame();
    }


    private void killPlayer(bool isObstacle)
    {


        if (currentLevel == 6)
        {

            clearText(Notice);
          
            if (isObstacle)
            {

                obstacleWrongColorKill.text = "SWITCH TO THE SAME COLOR!";
                
            
            }
            restartText.text = "PRESS ENTER TO RESTART!";
  
            
        }
        else
        {
            Time.timeScale = 0f;
            theDeathScreen.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);

            //SceneManager.LoadScene(0);
            //We should add here code that will call the level selection menu
            //mainCanvasObj.SetActive(false);
            //menusCanvas.transform.position -= new Vector3(0, 0, menuOffset);
            //killMenuObj.SetActive(true);
            //ActivateMenu(true);
        }
        
        //call restart game

    }


    private void playerWon()
    {
        Debug.Log("player won");
        //obstacleWrongColorKill.text = "You Won!";
        winScreen.gameObject.SetActive(true);
        pauseButton.SetActive(false);

        if (currentLevel == 6)
        {
            //GameConfig.cameraSpeed = 0;

            //obstacleWrongColorKill.text = "You Won!";
            clearText(Notice);

        }
    }
    

    public void restartGame()
    {
        theDeathScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;

        GameObject player = GameObject.Find("Player");
        GameObject levelStart = GameObject.Find("PlayerSpawn");

        player.transform.position = levelStart.transform.position;
        GlobalVar.isDead = false;

        clearText(obstacleWrongColorKill);
        clearText(restartText);

        onLevelRestart?.Invoke();       // do Player's specific restart 

        pauseButton.SetActive(true);    // do Pause button's specific restart

    }


    private void clearText(Text text)
    {
        text.text = "";
    }

    public void restartLevel()
    {

        restartGame();

    }

    private void ActivateMenu(bool status)
    {
        respawns = GameObject.FindGameObjectsWithTag("kill_menu");
        foreach (GameObject respawn in respawns)
        {
            print("setting active false");
            respawn.SetActive(status);
        }
    }

    private void disableText()
    {
        Notice.gameObject.SetActive(false);
        obstacleWrongColorKill.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
    }

}
