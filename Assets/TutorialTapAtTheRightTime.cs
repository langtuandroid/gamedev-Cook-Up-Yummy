using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTapAtTheRightTime : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TutorialTapAtTheRightTime"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");
            StartTut();
            PlayerPrefs.SetString("TutorialTapAtTheRightTime", "CA");
            PlayerPrefs.Save();
        }
    }

    void StartTut()
    {
        Timer.Schedule(canvas.GetComponent<MenuManager>(), .05f, delegate
        {
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialTapAtTheRightTime").gameObject);
        });
    }

    public static TutorialTapAtTheRightTime Instance;

}