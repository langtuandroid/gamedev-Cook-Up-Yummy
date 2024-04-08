using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCondition_DragToPlace : TargetConditionBase {

    void Start () 
    {
        // effectTriggerEnum is allways overriden for this effect. It should allways be set OnTriggerEnter.
       effectTriggerEnum = EEffectTriggerEnum.OnTriggerEnter;
    }

    #region implemented abstract members of TargetEffectsAbstract
    public override void TargetCondition()
    {
        event_TargetDone.Invoke();
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
