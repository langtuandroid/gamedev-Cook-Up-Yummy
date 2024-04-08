using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// Target effects abs class.
/// </summary>
public abstract class TargetConditionBase : MonoBehaviour
{
    public bool countTarget = true;
    public EEffectTriggerEnum effectTriggerEnum;
    public UnityEvent event_TargetDone;
    public abstract void TargetCondition();
    public abstract void TargetEffects_Start();
    public abstract void TargetEffects_Stop();
}

/// <summary>
/// Used for deciding when to trigger PlayTargetEffect.
/// </summary>
public enum EEffectTriggerEnum
{
    OnTriggerEnter,
    OnTriggerStay,
    OnRub
}