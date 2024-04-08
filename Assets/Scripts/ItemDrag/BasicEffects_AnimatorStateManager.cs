using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEffects_AnimatorStateManager : MonoBehaviour {

    public Animator anim;

    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        if (anim == null)
        {
            throw new System.Exception("There is no 'Animator' component set in inspector or attached to this GameObject! ");
        }
    }

    public void Effect_IncreaseStateCounter()
    {
        anim.SetInteger("Counter",anim.GetInteger("Counter")+1);
    }

    public void Effect_DecreaseStateCounter()
    {
        anim.SetInteger("Counter",anim.GetInteger("Counter")-1);
    }

}
