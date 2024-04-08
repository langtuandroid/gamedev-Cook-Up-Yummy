using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyCoinsReward : MonoBehaviour
{
    Transform canvas;

    public List<int> rewardsForEachDay;

    int currentDay;
    static int reward=0;
    private IEnumerator Start()
    {
        currentDay = PlayerPrefs.GetInt("DRCurrentDay");
        
        canvas = GameObject.Find("Canvas").transform;

        TimeSpan diff;
        if(PlayerPrefs.GetString("DailyRewardClaimed")!="")
        {
            DateTime time = DateTime.Parse(PlayerPrefs.GetString("DailyRewardClaimed"));
            diff = DateTime.Now - time;
            Debug.Log("Sati proslo od daily reward-a: " + diff.TotalHours);
            if (diff.TotalHours > 24&&diff.TotalHours<48)
            {
                
               
                currentDay++;
                reward = rewardsForEachDay[currentDay];
                if (currentDay > rewardsForEachDay.Count - 1)
                    currentDay = 0;
                PlayerPrefs.SetInt("DRCurrentDay", currentDay);
                PlayerPrefs.Save();
                ShowDailyRewardPopUp();
            }
            else if(diff.TotalHours>24)
            {
                currentDay = 0;
                //DODAJ MU prvi dan
                reward = rewardsForEachDay[currentDay];
                PlayerPrefs.SetInt("DRCurrentDay", currentDay);
                PlayerPrefs.Save();
                ShowDailyRewardPopUp();
            }

        }
        else
        {
            currentDay = 0;
            reward = rewardsForEachDay[currentDay];
            PlayerPrefs.SetInt("DRCurrentDay", currentDay);
            PlayerPrefs.Save();
            ShowDailyRewardPopUp();
        }
        yield return null;
    }

    void ShowDailyRewardPopUp()
    {
        PlayerPrefs.SetString("DailyRewardClaimed", DateTime.Now.ToString());
        PlayerPrefs.Save();
        canvas.transform.Find("PopUps/PopUpDailyReward").gameObject.SetActive(true);
        canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(false);
        canvas.transform.Find("PopUps/PopUpDailyReward/Holder/TakeReward").GetComponent<Button>().onClick.AddListener(delegate
        {
            GameBalance.Instance.Add(reward);
            if (SoundManager.Instance != null)
                SoundManager.Instance.Play_CashRegisterSound();
        });

        canvas.transform.Find("PopUps/PopUpDailyReward/Holder/DoubleReward").GetComponent<Button>().onClick.AddListener(delegate
        {
           /* if (Advertisements.Instance.IsRewardVideoAvailable())
            {
                //AdsManager.videoRewarded.RemoveAllListeners();
                //AdsManager.videoRewarded.AddListener(delegate
                //{
                    
                   

                Implementation.Instance.ShowRewardedVideo();
                GameBalance.Instance.Add(reward * 2);
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.Play_CashRegisterSound();
                    canvas.transform.Find("PopUps/PopUpDailyReward/Holder/TakeReward").GetComponent<Button>().onClick.RemoveAllListeners();
                    canvas.transform.Find("PopUps/PopUpDailyReward/Holder/TakeReward").GetComponent<Button>().onClick.Invoke();
                    canvas.transform.Find("PopUps/PopUpDailyReward").gameObject.SetActive(false);
                    canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(true);
                
                //AdsManager.Instance.ShowVideoReward();
            }
            else
            {*/
                canvas.GetComponent<MenuManager>().ShowVideoNotReadyPopUp();
            //}
        });


        for(int i=0;i<=currentDay;i++)
        {
            canvas.transform.Find("PopUps/PopUpDailyReward/Holder/Rewards").GetChild(i).GetChild(0).gameObject.SetActive(true);
            
        }

        PlayerPrefs.SetString("DailyRewardClaimed", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
}
 