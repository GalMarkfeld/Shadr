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
        int adjustedInd = SceneManager.GetActiveScene().buildIndex + 1;
        if (adjustedInd > SceneManager.sceneCount) adjustedInd = 1;     // return to level select if they try going to an out of bounds level
        SceneManager.LoadScene(adjustedInd);
    }

}
