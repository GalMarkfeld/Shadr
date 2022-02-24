using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseButton;
    [SerializeField] public int current;

    public static System.Action onLevelRestart = delegate { };

    private void Awake()
    {
        pauseMenu.SetActive(false);
        Debug.Log("in pause start");
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);   // turn off the button while in the pause menu
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void restart()
    {
        FindObjectOfType<GameManager>().restartGame();
        pauseMenu.SetActive(false);
    }

    public void levelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
