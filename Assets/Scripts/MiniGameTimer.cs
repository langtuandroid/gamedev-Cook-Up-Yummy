using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MiniGameTimer : MonoBehaviour
{
    public Image progress;

    public static UnityEvent timeOut = new UnityEvent();

    private static float passedTime, limitTime;
    public Color colorPerfect = Color.green;
    public Color colorGreat = Color.blue;
    public Color colorOk = Color.cyan;
    public Color colorBad = Color.yellow;
    public Color colorFailed = Color.red;

    static bool started;

    // Start is called before the first frame update
    void Start()
    {
        progress.fillAmount = 0f;
        //StartTimer(8f);
    }

    public static void StartTimer(float duration)
    {
        limitTime = duration;
        passedTime = 0f;
        started = true;
    }

    public static float StopTimer()
    {
        started = false;
        return limitTime - passedTime;
    }

    public static void Pause()
    {
        started = false;
    }

    public static void UnPauseAndAddTime(float time)
    {
        passedTime -= time;
        started = true;
    }

    private void Update()
    {
        if (started)
        {
            passedTime += Time.deltaTime;
            progress.fillAmount = passedTime / limitTime;
            if(progress.fillAmount<.2f)
            {
                progress.color = colorPerfect;
            }
            else if(progress.fillAmount<.4f)
            {
                progress.color = colorGreat;
            }
            else if(progress.fillAmount<.6f)
            {
                progress.color = colorOk;
            }else if(progress.fillAmount<.8f)
            {
                progress.color = colorBad;
            }
            else
            {
                progress.color = colorFailed;
            }

            if (passedTime >= limitTime)
            {
                StopTimer();
                timeOut.Invoke();
            }
        }
    }
}
