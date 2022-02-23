using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public string mainMenuLevel;

    public void restartGame()
    {
        FindObjectOfType<GameManager>().restartGame();
    }

    public void quitToMain()
    {
        SceneManager.LoadScene(mainMenuLevel);
    }

    public void SelectLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

}
