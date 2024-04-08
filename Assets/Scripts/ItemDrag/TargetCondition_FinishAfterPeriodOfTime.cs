using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetCondition_FinishAfterPeriodOfTime : TargetConditionBase {

    [Range (0.5f,10)]
    /// <summary>
    /// If set to 0, event_TargetDone will be fired OnTriggerEnter
    /// </summary>
    public float periodOfTime;

    float timer;
    bool targetDone;

    void Start () 
    {
        if (effectTriggerEnum == EEffectTriggerEnum.OnTriggerEnter)
            throw new System.Exception("effectTriggerEnum should be set OnTriggerStay or OnRub for this effect");
    }

    #region implemented abstract members of TargetConditionBase

    public override void TargetCondition()
    {
        if (!targetDone)
        {
            if (timer < periodOfTime)
            {
                timer += Time.fixedDeltaTime;
                Debug.Log("Object: "+name +"Timer: " + timer);
            }
            else
            {
                targetDone = true;
                event_TargetDone.Invoke();           
            }
        }
    }

    public override void TargetEffects_Start()
    {
        Debug.Log("TargetEffects_Start");
    }

    public override void TargetEffects_Stop()
    {
        Debug.Log("TargetEffects_Stop");
    }
    #endregion
}
