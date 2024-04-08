using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCircle : MonoBehaviour
{
    public Transform zone;
    public Transform knob;

    public float time;

    static bool started;
    static float limitTime;
    static int direction;
    static Vector3 pos;
    static bool changeZone;
    static Vector3 rot;
    static float difference;

    float knobdegrees;
    float zonedefrees;

    void Start()
    {
        StartMoving(2, 0);
        SetZoneAtRandomPos();
    }

    public static void SetZoneAtRandomPos()
    {
        rot = new Vector3(0f, 0f, Random.Range(0, 360f));
        changeZone = true;
    }

    public static void StartMoving(float time, int directio = 0)
    {
        started = true;
        limitTime = time;
        direction = directio;
    }

    public static float StopMoving()
    {
        started = false;
        return difference;
    }

    public static float ChangeDirection()
    {
        if (direction != 0)
            direction = 0;
        else
            direction = -1;

        return difference;
    }


    private void Update()
    {
        if (changeZone)
        {
            zone.transform.rotation = Quaternion.Euler(rot);
            changeZone = false;
        }

        pos = knob.transform.rotation.eulerAngles;
        if(started)
        {
            if(direction!=0)
            {
                pos.z += 360 * Time.deltaTime / limitTime;
            }
            else
            {
                pos.z -= 360 * Time.deltaTime / limitTime;
            }
            knob.transform.rotation = Quaternion.Euler(pos);
            knobdegrees = knob.transform.rotation.eulerAngles.z;
            zonedefrees = zone.transform.rotation.eulerAngles.z;
            if (knobdegrees < 0)
                knobdegrees = (knobdegrees % 360 + 360);
            if (zonedefrees < 0)
                zonedefrees = (zonedefrees % 360 + 360);

            



            //if (difference > 180)
            //{
            //    knobdegrees = knob.transform.rotation.eulerAngles.z;
            //    zonedefrees = zone.transform.rotation.eulerAngles.z;
            //    if (knobdegrees > 0)
            //        knobdegrees = (knobdegrees % 360 - 360);
            //    if (zonedefrees > 0)
            //        zonedefrees = (zonedefrees % 360 - 360);
            //}
            difference = knobdegrees - zonedefrees;
            difference = Mathf.Abs(difference);
            if (difference > 180)
                difference = 360 - difference;

            Debug.Log("DIFF " + difference);
        }
    }
}
