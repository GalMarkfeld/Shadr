using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    public static dontDestroy instance = null;
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            Destroy(gameObject);
        }


        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
}
