using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{

    public static float PerfectTolerance = 0.05f;
    public static float GreatTolerance = 0.12f;
    public static float GoodTolerance = 0.18f;
    public static float OKTolerance = 0.24f;

    public static List<QualityOfPhase> qualities=new List<QualityOfPhase>();
    void Start()
    {
        
    }

    public static void NormalizeQualities()
    {
        List<int> toRemove = new List<int>();
        QualityOfPhase lastQuality=null;
        for(int i=0;i<qualities.Count;i++)
        {
            QualityOfPhase q = qualities[i];
            if(lastQuality!=null&&q.phaseName==lastQuality.phaseName)
            {
                if (q.quality != Quality.Failed)
                    lastQuality.quality = (Quality)(((int)q.quality + (int)lastQuality.quality) / 2);
                else
                    lastQuality.quality = Quality.Failed;
                toRemove.Add(i);
            }
            else
            {
                lastQuality = q;
            }
        }
        toRemove.Sort();
        for (int j = toRemove.Count - 1; j >= 0; j--)
            qualities.RemoveAt(toRemove[j]);
    }

    public static float FinalQuality()
    {
        float returnQuality=0f;
        int num = 0;
        foreach(QualityOfPhase q in qualities)
        {
            num += (int)q.quality;
        }

        returnQuality= (float)num / (float)qualities.Count;
        return returnQuality/5f;
    }
}

[System.Serializable]
public class QualityOfPhase
{
    public Quality quality;
    public string phaseName;

}

public enum Quality
{
    Failed = 1,
    OK = 2,
    Good = 3,
    Great = 4,
    Perfect = 5
}

