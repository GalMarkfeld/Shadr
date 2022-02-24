using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject[] respawns;
    public static GameManager inst;
    public GameObject theDeathScreen;
    public GameObject winScreen;
    public GameObject pauseButton;

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

    //[Space]
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
        print("in game manager awake");
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

        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;

        theDeathScreen = GameObject.FindGameObjectsWithTag("kill_menu")[0];
        winScreen = GameObject.FindGameObjectsWithTag("win_menu")[0];
        pauseButton = GameObject.FindGameObjectsWithTag("pause_button")[0];

        print(theDeathScreen);
        print(winScreen);
        winScreen.gameObject.SetActive(false);
        theDeathScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    //Gal edit
    private void setNotice(String text)
    {
        if(Notice)
        {
            Notice.text = text;
            // Roey updated
            if (avoidTextPrompts)
            {
                Debug.Log("Roey update - ignoring text in non-tutorial level!");
                clearText(Notice);
                disableText();

            }
        }     
    }

    private void Start()
    {
        restartGame();
    }


    private void killPlayer(bool isObstacle)
    {

        if (currentLevel == 2 && obstacleWrongColorKill)
        {
            obstacleWrongColorKill.gameObject.SetActive(false);
            Notice.gameObject.SetActive(false);
            restartText.gameObject.SetActive(false);
        }
        /*clearText(Notice);

        if (isObstacle)
        {

            obstacleWrongColorKill.text = "SWITCH TO THE SAME COLOR!";


        }
        restartText.text = "PRESS ENTER TO RESTART!";*/
        if (theDeathScreen == null)
        {
            restartGame();
        }
        else
        {
            Time.timeScale = 0f;
            theDeathScreen.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        }

    }


    private void playerWon()
    {
        print(this);
        try
        {
            winScreen.gameObject.SetActive(true);
            pauseButton.SetActive(false);
        }
        catch
        {
            return;
            //SceneManager.LoadScene(1);
        }
        /*if (winScreen == null)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            winScreen.gameObject.SetActive(true);
            pauseButton.SetActive(false);
        }*/


        clearText(Notice);
    }
    

    public void restartGame()
    {
        if(currentLevel==2 && obstacleWrongColorKill)
        {
            obstacleWrongColorKill.gameObject.SetActive(true);
            Notice.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);
        }

        if(theDeathScreen != null)
        {
            theDeathScreen.gameObject.SetActive(false);
            winScreen.gameObject.SetActive(false);
            pauseButton.SetActive(true);    // do Pause button's specific restart
            Time.timeScale = 1f;
        }

        GameObject player = GameObject.Find("Player");
        GameObject levelStart = GameObject.Find("PlayerSpawn");

        player.transform.position = levelStart.transform.position;
        GlobalVar.isDead = false;

        clearText(obstacleWrongColorKill);
        clearText(restartText);

        onLevelRestart?.Invoke();       // do Player's specific restart 
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
