using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    //public static GameManager inst;

    // Start is called before the first frame update
    public void SelectLevel(int sceneID)
    {
        //inst.currentLevel = sceneID;
        Debug.Log(sceneID);
        SceneManager.LoadScene(sceneID);
    }

    public void RestartLevel(int sceneID)
    {
        //inst.currentLevel = sceneID;
        Debug.Log(sceneID); 
        SceneManager.LoadScene(sceneID);
    }
}
