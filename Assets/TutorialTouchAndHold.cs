using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTouchAndHold : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if(PlayerPrefs.HasKey("TutorialTouchAndHold"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");
            StartTut();
            PlayerPrefs.SetString("TutorialTouchAndHold", "CA");
            PlayerPrefs.Save();
            
        }
    }

    void StartTut()
    {
        Debug.Log("Start tut  touchandhold");
        Timer.Schedule(canvas.GetComponent<MenuManager>(), .05f, delegate
        {
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialTouchAndHolder").gameObject);
        });
    }

    public void InstantiateVersionPrefab(GameObject prefab)
    {
        GameObject newOb = GameObject.Instantiate(prefab);
        newOb.transform.SetParent(canvas.transform.Find("PopUpTutorialTouchAndHolder/AnimationHolder/Body/ContentHolder/Background/VersionPrefabHolder"), false);
        foreach (Canvas c in newOb.GetComponentsInChildren<Canvas>())
            GameObject.Destroy(c);
    }

    public static TutorialTouchAndHold Instance;
}
