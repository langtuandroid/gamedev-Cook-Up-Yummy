using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMixFood : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TutorialMixFood"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");

            StartTut();
            PlayerPrefs.SetString("TutorialMixFood", "CA");
            PlayerPrefs.Save();
        }
    }

    void StartTut()
    {
        Timer.Schedule(canvas.GetComponent<MenuManager>(), 0.05f, delegate
        {
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialMixFood").gameObject);
        });
        
    }

    public static TutorialMixFood Instance;

}