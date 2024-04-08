using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast : MonoBehaviour
{
    public static Toast Instance;
    public static bool done;
    private void Awake()
    {
        Instance = this;    
    }

    public void Show(int index,float time)
    {
        foreach (Transform t in transform.Find("ToastBg"))
            t.gameObject.SetActive(false);

        if (SoundManager.Instance != null)
            SoundManager.Instance.Play_ActionCompleted();
        Animator anim = GetComponent<Animator>();
        transform.Find("ToastBg").gameObject.SetActive(true);
        transform.Find("ToastBg").GetChild(index - 1).gameObject.SetActive(true);
        Timer.Schedule(this, time, delegate
        {
            if(!done)
                transform.Find("ToastBg").gameObject.SetActive(false);
        });
    }

    public void Reset()
    {
        done = false;
        transform.Find("ToastBg").gameObject.SetActive(false);
    }

    public static void Stay()
    {
        done = true;
    }

    public void Show(int index)
    {
        Show(index, 3f);
        //foreach (Transfor\m t in transform.Find("ToastBg"))
        //    t.gameObject.SetActive(false)

        //Animator anim = GetComponent<Animator>();
        //transform.Find("ToastBg").GetChild(index-1).gameObject.SetActive(true);
        //Timer.Schedule(this, 1.5f, delegate
        //{
        //    anim.SetTrigger("Show");
        //    if (SoundManager.Instance != null)
        //        SoundManager.Instance.Play_ActionCompleted();
        //});
    }
}
