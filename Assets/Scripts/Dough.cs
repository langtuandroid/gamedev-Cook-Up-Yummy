using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dough : MonoBehaviour
{
    Animator animator;

    float a;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Kneading");
        animator.speed = 0;
        a = 0f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play_DoughStretch();
        a += Time.deltaTime/ animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Debug.Log(a);
        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        animator.Play(0, 0, a);
        if (a >= 1)
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            MiniGameKneadingTheDoughManager.Instance.Done();
        }
    }
}
 