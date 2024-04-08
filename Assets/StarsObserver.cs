using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsObserver : MonoBehaviour
{
    void Start()
    {
        if (!GameBalance.starsObservers.Contains(this))
            GameBalance.starsObservers.Add(this);

        UpdateState(GameBalance.StarsAmount);
        
    }


    public void UpdateState(int starsAmount)
    {
        GetComponent<Text>().text = starsAmount.ToString();
    }

    public void UpdateState(int lastAmount, int newAmount)
    {
        StartCoroutine(lerpToNumber(lastAmount, newAmount));
    }

    IEnumerator lerpToNumber(int lastAmount, int newAmount)
    {
        float a = 0;
        while (a < 1f)
        {
            a += 0.2f;
            yield return new WaitForSeconds(0.04f);
            GetComponent<Text>().text = ((int)Mathf.Lerp(lastAmount, newAmount, a)).ToString();
        }
        GetComponent<Text>().text = newAmount.ToString();
    }


}
