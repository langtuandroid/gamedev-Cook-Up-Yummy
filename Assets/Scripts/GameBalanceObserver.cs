using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBalanceObserver : MonoBehaviour
{
    void Start()
    {
        if (!GameBalance.observers.Contains(this))
            GameBalance.observers.Add(this);

        UpdateState(GameBalance.CurrentAmount);
    }

    public void UpdateState(int moneyAmount)
    {
        GetComponent<Text>().text = moneyAmount.ToString();        
    }

    public void UpdateState(int lastAmount,int newAmount)
    {
        StartCoroutine(lerpToNumber(lastAmount,newAmount));
    }
    IEnumerator lerpToNumber(int lastAmount,int newAmount)
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
