using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialtapWhenItsDone : MonoBehaviour
{
    GameObject canvas;
    public static bool done = false;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TutorialTapWhenItsDone"))
        {
            done = true;
            gameObject.SetActive(false);
        }
        else
        {
            canvas = GameObject.Find("Canvas");
            StartTut();
            PlayerPrefs.SetString("TutorialTapWhenItsDone", "CA");
            PlayerPrefs.Save();
            
        }
    }

    void StartTut()
    {
        Timer.Schedule(canvas.GetComponent<MenuManager>(), 1f, delegate
        {
            canvas.GetComponent<MenuManager>().ShowPopUpMenu(canvas.transform.Find("PopUpTutorialTapWhenItsDone").gameObject);
        });
    }

    public void InstantiateVersionPrefab(GameObject prefab)
    {
        GameObject newOb = GameObject.Instantiate(prefab);
        newOb.transform.SetParent(canvas.transform.Find("PopUpTutorialTapWhenItsDone/AnimationHolder/Body/ContentHolder/Background/VersionPrefabHolder"), false);
        foreach (Canvas c in newOb.GetComponentsInChildren<Canvas>())
            GameObject.Destroy(c);
    }

    public static TutorialtapWhenItsDone Instance;


}
