using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public void SelectLevel(string levelName)
    {
        Debug.Log(levelName);
        SceneManager.LoadScene(levelName);
    }
}
