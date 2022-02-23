using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="RunnerGameConfig")]
public static class GlobalVar
{
    public static Color activeColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] public static float cameraFollowFactor = 0.3f;

    public static bool isDead = false;


    public static float cameraSpeed = 5;

    public static GameObject killMenuObject;


}
