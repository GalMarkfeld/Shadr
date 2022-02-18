using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameObject pauseMenu;
    public int current;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        Debug.Log("in pause start");
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void restart()
    {
        Time.timeScale = 1f;    
        SceneManager.LoadScene(current);
    }

    public void levelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
