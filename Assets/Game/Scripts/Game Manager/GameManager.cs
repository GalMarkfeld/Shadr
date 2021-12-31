using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager inst;
    //public RunnerGameConfig gameConfig;
    //public Transform startPos;

    [Header("UI Reference")]
    public Text obstacleWrongColorKill;

    //Gal edit: made instructions into 1 text that changes
    public Text Notice;


    public Text restartText;

    //public Text scoreText;
    //public Text highScoreText;

    //[Header("Levels")]
    ////public List<Level> levels = new List<Level>();

    //[Space]
    //public UnityEvent LevelFinishedEvent;

    //public static Action<Level> OnLevelStart = delegate { };



    //private int _score;
    //private int _highScore;
    private int _level = 0;
    public Transform startPosition;
    
    //private Level _currentLevel;


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
    }

    //Gal edit
    private void setNotice(String tag)
    {
        
        if (tag== "Shift Notice")
        {
            Notice.text = "PRESS SHIFT!";
        }
        else if (tag == "Jump Notice")
        {
            Notice.text = "PRESS SPACE!";
        }
        else if (tag == "Double Shift Notice")
        {
            Notice.text = "PRESS SHIFT MULTIPLE TIMES!";
        }
        else if (tag == "Combination Notice")
        {
            Notice.text = "TRY BOTH OPTIONS!";

        } 
    }

    private void Start()
    {
        restartGame();
    }


    private void killPlayer(bool isObstacle)
    {


        if (_level == 0)
        {

            clearText(Notice);
          


            //GameConfig.cameraSpeed = 0;

            if (isObstacle)
            {
                
                obstacleWrongColorKill.text = "SWITCH TO THE SAME COLOR!";
                
            
            }
            restartText.text = "PRESS ENTER TO RESTART!";




            //float timeToWait = 0.5f;
            //float done = 0.0f;

            //while(Time.time > done)
            //{
            //    done = Time.time + timeToWait;

            //}



            
            
        }
        
        
        //call restart game

    }

    //IEnumerator Reset(float Count)
    //{
    //    yield return new WaitForSeconds(Count); //Count is the amount of time in seconds that you want to wait.
    //                                            //And here goes your method of resetting the game...
    //    yield return restartGame();
    //}    //IEnumerator Reset(float Count)
    //{
    //    yield return new WaitForSeconds(Count); //Count is the amount of time in seconds that you want to wait.
    //                                            //And here goes your method of resetting the game...
    //    yield return restartGame();
    //}



    private void playerWon()
    {
        if (_level == 0)
        {
            //GameConfig.cameraSpeed = 0;

            obstacleWrongColorKill.text = "You Won!";
            clearText(Notice);
            
        }
    }

    //IEnumerator _wait(float time, Action callback)
    //{
    //    yield return new WaitForSeconds(time);
    //    callback();
    //}


    private void restartGame()
    {
        GameObject player = GameObject.Find("Player");
        GameObject levelStart = GameObject.Find("PlayerSpawn");

        player.transform.position = levelStart.transform.position;
        GlobalVar.isDead = false;

        //inst.transform.Find("Player").transform.position = inst.transform.position + new Vector3(-30,5,0);

        //GameConfig.cameraSpeed = 5;

        clearText(obstacleWrongColorKill);
        clearText(restartText);
        

        //curretn color = 0
        //make player white 
        //
    }


    private void clearText(Text text)
    {
        text.text = "";
    }

    //////////private void Awake()
    //////////{
    //////////    #region Singleton
    //////////    if (inst == null)
    //////////    {
    //////////        inst = this;
    //////////    }
    //////////    else
    //////////    {
    //////////        Debug.LogWarning("More then one GameManger was created");
    //////////        Destroy(gameObject);
    //////////    }
    //////////    #endregion

    //////////   //sign to events


    //////////}

    //private void Start()
    //{

    //}

    //private void AddScore(Transform transform)
    //{
    //    _score++;

    //    if (_score > _highScore)
    //        _highScore = _score;

    //    UpdateScoreText();
    //}

    //private void UpdateScoreText()
    //{
    //    scoreText.text = _score.ToString();
    //    highScoreText.text = _highScore.ToString();
    //}

    //private void LevelFail()
    //{
    //    _score = 0;
    //    UpdateScoreText();

    //    Invoke(nameof(MovePlayerWhenKilled), 0.02f);
    //    Invoke(nameof(RestartGame), 1f);
    //}

    //private void LevelFinifhed()
    //{
    //    LevelFinishedEvent.Invoke();
    //    _level++;

    //    if (_level >= levels.Count)
    //        _level = 0;

    //    Invoke(nameof(StartLevel), 1f);
    //}

    //private void MovePlayerWhenKilled()
    //{
    //    ball.position = Vector3.up * 200f;
    //}

    //private void StartLevel()
    //{
    //    if (_currentLevel)
    //        Destroy(_currentLevel.gameObject);

    //    _currentLevel = Instantiate(levels[_level], helix).GetComponent<Level>();
    //    ball.position = startPos.position;
    //    OnLevelStart.Invoke(_currentLevel);
    //}

    //public void RestartGame()
    //{
    //    if(_currentLevel)
    //        Destroy(_currentLevel.gameObject);

    //    _currentLevel = Instantiate(levels[0], helix).GetComponent<Level>();

    //    ball.position = startPos.position;
    //    helix.rotation = Quaternion.identity;

    //    _score = 0;
    //    UpdateScoreText();
    //}
}
