using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlice : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TutorialSliceFood"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");

            StartTut();
            PlayerPrefs.SetString("TutorialSliceFood", "CA");
            PlayerPrefs.Save();
        }
    }

    void StartTut()
    {
        Timer.Schedule(this, .05f, delegate
        {
            MiniGameTimer.StopTimer();
        });
        Timer.Schedule(canvas.GetComponent<MenuManager>(), .05f, delegate
        {
            canvas.transform.Find("PopUpTutorialSliceFood/AnimationHolder/Body/ButtonExit").GetComponent<Button>().onClick.AddListener(delegate
            {
                MiniGameTimer.UnPauseAndAddTime(1f);
            });
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialSliceFood").gameObject);
            
        });
    }

    public static TutorialSlice Instance;

}