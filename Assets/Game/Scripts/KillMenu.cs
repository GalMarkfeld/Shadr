using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillMenu : MonoBehaviour
{

    private static KillMenu instance;


    [SerializeField]
    public GameObject killMenu;
    public int current;

    private void Awake()
    {
        //killMenu.SetActive(false); ;
        Debug.Log("in kill menu awake");
        print(killMenu.activeInHierarchy);

        //killMenuObj = GameObject.FindGameObjectsWithTag("kill_menu")[0];
        //Check if instance already exists
        /*if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            Destroy(gameObject);
        }*/


        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(instance);
    }

    private void onEnable()
    {
        Debug.Log("enabled");
    }

    private void OnDisable()
    {
        Debug.Log("disabled");
    }

    public void killed()
    {
        //killMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void restart()
    {
        //killMenuObj.SetActive(true);
        Time.timeScale = 1f;    
        SceneManager.LoadScene(current);
    }

    public void levelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
