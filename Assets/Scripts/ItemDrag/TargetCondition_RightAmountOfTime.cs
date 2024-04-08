using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetCondition_RightAmountOfTime : TargetConditionBase {
    
    public float minTime;
    public float maxTime;
    public UnityEvent event_BecameGoodDone;
    public UnityEvent event_BecameOverDone;
    public UnityEvent event_TargetFail;

    float timer;
    ETargetDoneGood tgd;
    bool goodDoneFlag;
    bool overDoneFlag;

    void Start () 
    {
        timer = 0;
        tgd = ETargetDoneGood.UnderDone;
        if (effectTriggerEnum == EEffectTriggerEnum.OnTriggerEnter)
            throw new System.Exception("effectTriggerEnum should be set OnTriggerStay or OnRub for this effect");
    }

    #region implemented abstract members of TargetConditionBase

    public override void TargetCondition()
    {
        timer += Time.fixedDeltaTime;
        Debug.Log("Right Amount timer: " + timer);
        if (timer < maxTime)
        {
            if (timer > minTime && !goodDoneFlag)
            {
                Debug.Log("GoodDoned" + timer);
                goodDoneFlag = true;
                tgd = ETargetDoneGood.DoneGood;
                event_BecameGoodDone.Invoke();
            }
        }
        else if(!overDoneFlag)
        {
            Debug.Log("OverDoned" + timer);
            overDoneFlag = true;
            tgd = ETargetDoneGood.OverDone;
            event_BecameOverDone.Invoke();
        }
    }

    public override void TargetEffects_Start()
    {
        Debug.Log("TargetEffects_Start");
    }

    public override void TargetEffects_Stop()
    {
        if (tgd == ETargetDoneGood.DoneGood)
            event_TargetDone.Invoke();
        else if (tgd == ETargetDoneGood.OverDone)
            event_TargetFail.Invoke();

        timer = 0;
        tgd = ETargetDoneGood.UnderDone;
        goodDoneFlag = false;
        overDoneFlag = false;
    }
    #endregion
}

enum ETargetDoneGood
{
    UnderDone,
    DoneGood,
    OverDone
}