using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialKneadingTheDough : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TutorialKneading"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");

            StartTut();
            PlayerPrefs.SetString("TutorialKneading", "CA");
            PlayerPrefs.Save();
        }
    }

    void StartTut()
    {
        Timer.Schedule(this, 0.05f, delegate
        {
            MiniGameTimer.StopTimer();
        });
        Timer.Schedule(canvas.GetComponent<MenuManager>(), .2f, delegate
        {
            canvas.transform.Find("PopUpTutorialKneadingTheDough/AnimationHolder/Body/ButtonExit").GetComponent<Button>().onClick.AddListener(delegate
            {
                MiniGameTimer.UnPauseAndAddTime(1f);
            });
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialKneadingTheDough").gameObject);

        });
    }

   

    public static TutorialKneadingTheDough Instance;


}
