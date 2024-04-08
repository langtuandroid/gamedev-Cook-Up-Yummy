using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalProgressBar : MonoBehaviour
{
    public Transform arrow;
    public Transform leftBorder,rightBorder;
    public Transform zone;
    [Header("samo za test")]
    public float timeToFill;

    public bool twoway = false;
    bool ascend=true;

    public static float timePassed,limitTime;
    public static bool started;


    public static UnityEvent timeOut = new UnityEvent();
    private float a;

    void Start()
    {
        //StartArrowBarMoving(timeToFill);
    }

    public static void StartArrowBarMoving( float time)
    {
        limitTime = time;
        timePassed = 0f;
        started = true;
    }

    public void SetGoalValue(float goal)
    {
        Vector3 posToSet = zone.GetComponent<RectTransform>().anchoredPosition3D;
        posToSet.x = goal * zone.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x - (zone.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.x / 2);
        zone.GetComponent<RectTransform>().anchoredPosition3D = posToSet;
    }

    public static float Stop()
    {
        started = false;
        return timePassed/limitTime;
    }

    void Update()
    {
        if(started)
        {
            if (!twoway)
            {
                timePassed += Time.deltaTime;
                a = timePassed / limitTime;
                arrow.transform.position = Vector3.Lerp(leftBorder.transform.position, rightBorder.transform.position, a);
                if (timePassed > limitTime)
                {
                    timeOut.Invoke();
                    Stop();
                }
            }
            else
            {
                if (ascend)
                {
                    timePassed += Time.deltaTime;
                    a = timePassed / limitTime;
                    arrow.transform.position = Vector3.Lerp(leftBorder.transform.position, rightBorder.transform.position, a);
                    if (timePassed > limitTime)
                    {
                        ascend = false;
                    }
                }
                else
                {
                    timePassed -= Time.deltaTime;
                    a = timePassed / limitTime;
                    arrow.transform.position = Vector3.Lerp(leftBorder.transform.position, rightBorder.transform.position, a);
                    if (timePassed <= 0.01f)
                    {
                        ascend = true;
                    }
                }
            }
        }
    }


}
