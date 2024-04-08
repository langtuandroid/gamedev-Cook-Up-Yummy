using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Upisuje u MoneyAmount prefs

public class GameBalance : MonoBehaviour
{
    public static GameBalance Instance;

    public static List<GameBalanceObserver> observers = new List<GameBalanceObserver>();
    public static List<StarsObserver> starsObservers = new List<StarsObserver>();

    static int currentAmount;
    static int starsAmount;
    public static int CurrentAmount
    {
        get
        {
            return currentAmount;
        }
    }

    public static int StarsAmount
    {
        get
        {
            return starsAmount;
        }
    }

   

    private void Awake()
    {
        Instance = this;
        PlayerPrefs.SetInt("MoneyAmount", 1000000);
        PlayerPrefs.SetInt("StarsAmount", 10000000);
        LoadCoinsFromPrefs();
        LoadStarsFromPrefs();
    }


    public void UpdateObservers()
    {
        foreach(GameBalanceObserver gbo in observers)
        {
            if(gbo!=null)
            {
                gbo.UpdateState(currentAmount);
            }
        }
    }

    public void UpdateObserversWithLerp(int lastAmount,int newAmount)
    {
        foreach (GameBalanceObserver gbo in observers)
        {
            if (gbo != null)
            {
                gbo.UpdateState(lastAmount,currentAmount);
            }
        }
    }


    public void Add(int amount)
    {
        currentAmount += amount;
        SaveCoinsToPrefs();
        UpdateObserversWithLerp (currentAmount-amount,currentAmount);
    }

    public bool Spend(int amount)
    {
        if (currentAmount >= amount)
        {
            currentAmount -= amount;
            SaveCoinsToPrefs();
            UpdateObserversWithLerp(currentAmount+amount,currentAmount);
            return true;
        }
        else
            return false;
    }

    public void AddStars(int amount)
    {
        starsAmount += amount;
        SaveStarsToPrefs();
    }

    public void SaveStarsToPrefs()
    {
        PlayerPrefs.SetInt("StarsAmount", starsAmount);
        PlayerPrefs.Save();
    }

    public void LoadStarsFromPrefs()
    {
        if (PlayerPrefs.HasKey("StarsAmount"))
            starsAmount = PlayerPrefs.GetInt("StarsAmount");
        else
            starsAmount = 0;
    }

    public void SaveCoinsToPrefs()
    {
        PlayerPrefs.SetInt("MoneyAmount", currentAmount);
        PlayerPrefs.Save();
    }

    void LoadCoinsFromPrefs()
    {
        if (PlayerPrefs.HasKey("MoneyAmount"))
            currentAmount = PlayerPrefs.GetInt("MoneyAmount");
        else
            currentAmount = 0;
    }
}
