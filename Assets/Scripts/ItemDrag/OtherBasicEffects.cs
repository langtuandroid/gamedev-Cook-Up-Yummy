using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherBasicEffects : MonoBehaviour {

    int tempAnimCounter;
    Image tmpImage;
    Color tmpColor;
    public void Effect_Destroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Enables or disables the dragable item. ItemDrag , Rigidbody2D and Collider2D components required 
    /// </summary>
    /// <param name="enabled">If set to <c>true</c> enabled.</param>
    public void Effect_EnableDragableItem(bool enabled)
    {
        GetComponent<ItemDrag>().isDraggable = enabled;
        GetComponent<Collider2D>().enabled = enabled;
        GetComponent<Rigidbody2D>().simulated = enabled;

        foreach (Collider2D col in GetComponent<ItemDrag>().targets)
        {
            col.enabled = enabled;
        }
    }

    public void Effect_PlayAnimation(string animStateName)
    {
        GetComponent<Animator>().Play(animStateName);
    }

    public void Effect_StopAnimation()
    {
        //GetComponent<Animator>().Stop();
    }

    public void IncreaseAnimatorCounter()
    {
        tempAnimCounter = GetComponent<Animator>().GetInteger("Counter");
        tempAnimCounter++;
        GetComponent<Animator>().SetInteger("Counter",tempAnimCounter);
    }

    public void Effect_SoftAppear(float ammount)
    {
        tmpImage = GetComponent<Image>();
        tmpImage.color = new Color(1,1,1,0);
        tmpColor = new Color(0,0,0,ammount);
        gameObject.SetActive(true);
//        StopCoroutine(CSoftAppear());
        StartCoroutine(CSoftAppear());
    }

    public void Effect_SoftDisappear(float ammount)
    {
        tmpImage = GetComponent<Image>();
        tmpImage.color = new Color(1,1,1,1);
        tmpColor = new Color(0,0,0,ammount);
        gameObject.SetActive(true);
//        StopCoroutine(CSoftDissapear());
        StartCoroutine(CSoftDissapear());
    }

    IEnumerator CSoftAppear()
    {
        yield return new WaitForEndOfFrame();
        while(tmpImage.color.a < 1)
        {
            tmpImage.color += tmpColor;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CSoftDissapear()
    {
        yield return new WaitForEndOfFrame();
        while(tmpImage.color.a > 0)
        {
            tmpImage.color -= tmpColor;
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
        tmpImage.color = new Color(1,1,1,1);    // Setting alpha back to 1 for further use of the item
    }
}
