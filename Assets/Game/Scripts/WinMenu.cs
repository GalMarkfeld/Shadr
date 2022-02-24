using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// appears when the player wins a level:
// go to next level
// go to level select
// go to main menu

public class WinMenu : MonoBehaviour
{

    public int currentLevel;

    public void quitToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void SelectLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void goToNext()
    {
        if(currentLevel==7)
        {
            SceneManager.LoadScene(0);
        }
        SceneManager.LoadScene(currentLevel+1);
    }

}
