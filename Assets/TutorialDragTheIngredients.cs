using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDragTheIngredients : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TutorialDragTheIngredients"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");
            StartTut();
            PlayerPrefs.SetString("TutorialDragTheIngredients", "CA");
            PlayerPrefs.Save();
        }
    }

    void StartTut()
    {
        Timer.Schedule(this, .05f, delegate
        {
            MiniGameTimer.StopTimer();
        });
        Timer.Schedule(canvas.GetComponent<MenuManager>(), .2f, delegate
        {
            canvas.transform.Find("PopUpTutorialDragIngredients/AnimationHolder/Body/ButtonExit").GetComponent<Button>().onClick.AddListener(delegate
            {
                MiniGameTimer.UnPauseAndAddTime(1f);
            });
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialDragIngredients").gameObject);
        });
    }



    public static TutorialDragTheIngredients Instance;


}
