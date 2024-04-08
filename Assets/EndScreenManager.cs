using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public static bool failed;


    GameObject canvas;

    private Transform star1, star2, star3;
    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        PositionStarsOnProgressBar();
        canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar").GetComponent<Image>().fillAmount = 0f;
        if (failed)
        {
            canvas.transform.Find("DishFailed").gameObject.SetActive(true);
            ShowQualitiesFailedDish();
        }
        else
        {
            canvas.transform.Find("DishFailed").gameObject.SetActive(false);
            ShowQualitiesFinishedDish();
            PositionStarsOnProgressBar();
        }
    }

    void ShowQualitiesFinishedDish()
    {
        canvas.transform.Find("TopUI/ButtonOK").gameObject.SetActive(false);
        Transform contentHolder = canvas.transform.Find("DishFinished/DishHolder/Scroll View/Viewport/Content");
        GameObject newObject;
        float finalQuality = QualityManager.FinalQuality();
        QualityManager.NormalizeQualities();

        foreach (QualityOfPhase q in QualityManager.qualities)
        {
            newObject = GameObject.Instantiate(contentHolder.GetChild(0).gameObject);
            newObject.transform.SetParent(contentHolder, false);

            newObject.transform.Find("NameOfPhase").GetComponent<Text>().text = q.phaseName;
            newObject.transform.Find("QualityTextHolder").GetChild((int)(q.quality) - 1).gameObject.SetActive(true);
        }

        GameObject.Destroy(contentHolder.GetChild(0).gameObject);

        canvas.transform.Find("DishFinished/DishHolder/TitleHolder/Title").GetComponent<Text>().text = GlobalVariables.dishInCreation.name;
        canvas.transform.Find("DishFinished/DishHolder/DishIconHolder/Icon").GetComponent<Image>().sprite = GlobalVariables.dishInCreation.sprite;
        //canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar").GetComponent<Image>().fillAmount = finalQuality;
        canvas.transform.Find("DishFinished/Score/PercentText").GetComponent<Text>().text = (Mathf.RoundToInt(finalQuality * 100)) + "%";

        int reward = 0;
        int stars = 0;

        if (Mathf.RoundToInt(finalQuality * 100)>= (Mathf.RoundToInt(GlobalVariables.dishInCreation.goals.goal3Star)))
        {
            reward = GlobalVariables.dishInCreation.earnings3Star;
            stars = 3;
        }
        else if (Mathf.RoundToInt(finalQuality * 100) >= (Mathf.RoundToInt(GlobalVariables.dishInCreation.goals.goal2Star)))
        {
            reward = GlobalVariables.dishInCreation.earnings2Star;
            stars = 2;
        }
        else if (Mathf.RoundToInt(finalQuality * 100) >= (Mathf.RoundToInt(GlobalVariables.dishInCreation.goals.goal1Star)))
        {
            reward = GlobalVariables.dishInCreation.earnings1Star;
            stars = 1;
        }
        else
        {
            reward = GlobalVariables.dishInCreation.Cost();
        }

        canvas.transform.Find("DishFinished/Reward/CoinsText").GetComponent<Text>().text = reward.ToString();
        //GameBalance.Instance.Add(reward);
        if (stars > GlobalVariables.dishInCreation.Stars)
        {
            GameBalance.Instance.AddStars(stars - GlobalVariables.dishInCreation.Stars);
            GlobalVariables.dishInCreation.Stars = stars;
        }


     /*   canvas.transform.Find("DishFinished/Reward/DoubleRewardButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            if (Advertisements.Instance.IsRewardVideoAvailable())
            {
                //AdsManager.videoRewarded.RemoveAllListeners();
                //AdsManager.videoRewarded.AddListener(delegate
                //{
                    

                //    //Timer.Schedule(this, 3f, delegate
                //    //{
                //    //    canvas.GetComponent<MenuManager>().LoadSceneWithComingTransitionAndInterstitial("GroceriesAndRestaurant");
                //    //});
                //});
                Implementation.Instance.ShowRewardedVideo();
                GameBalance.Instance.Add(reward * 2);
                canvas.transform.Find("DishFinished/Reward/DoubleRewardButton").GetComponent<Button>().interactable = false;
                canvas.transform.Find("DishFinished/Reward/TakeRewardButton").gameObject.GetComponent<Button>().interactable = false;
                canvas.transform.Find("TopUI/BackButton").gameObject.SetActive(false);

                Timer.Schedule(this, .5f, delegate
                {
                    canvas.GetComponent<MenuManager>().ShowPopUpMessage("Reward", "You got " + reward * 2 + " reward", delegate
                    {
                        canvas.GetComponent<MenuManager>().LoadSceneWithComingTransitionAndInterstitial("GroceriesAndRestaurant");
                    });
                });
                //AdsManager.Instance.ShowVideoReward();
            }
            else
            {
                canvas.GetComponent<MenuManager>().ShowVideoNotReadyPopUp();
            }
        });
*/
       /* canvas.transform.Find("DishFinished/Reward/TakeRewardButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            GameBalance.Instance.Add(reward);
        });*/

        StartCoroutine(fillQualityBar(finalQuality));
    }

    IEnumerator fillQualityBar(float finalQuality)
    {
        yield return new WaitForSeconds(2.5f);
        float a = 0f;
        while(a<1f)
        {
            yield return new WaitForSeconds(0.03f);
            a += 0.01f;
            if ((finalQuality/(1/a))>GlobalVariables.dishInCreation.goals.goal1Star/100f)
            {
                star1.GetChild(0).gameObject.SetActive(true);
                star1.GetComponent<StarFlyToPoint>().Fly();
            }
            if ((finalQuality / (1 / a)) > GlobalVariables.dishInCreation.goals.goal2Star/100f)
            {
                star2.GetChild(0).gameObject.SetActive(true);
                star2.GetComponent<StarFlyToPoint>().Fly();
            }
            if ((finalQuality / (1 / a)) > GlobalVariables.dishInCreation.goals.goal3Star / 100f)
            {
                star3.GetChild(0).gameObject.SetActive(true);
                star3.GetComponent<StarFlyToPoint>().Fly();
            }

            canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar").GetComponent<Image>().fillAmount = finalQuality/(1/a);
        }
    }

    void PositionStarsOnProgressBar()
    {
        Vector3 anchoredPosition;
        anchoredPosition = canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/1Star").GetComponent<RectTransform>().anchoredPosition3D;
        anchoredPosition.x = GlobalVariables.dishInCreation.goals.goal1Star * 6.67f;
        canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/1Star").GetComponent<RectTransform>().anchoredPosition3D = anchoredPosition;
        star1 = canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/1Star");
        star2 = canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/2Star");
        star3 = canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/3Star");

        anchoredPosition = canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/2Star").GetComponent<RectTransform>().anchoredPosition3D;
        anchoredPosition.x = GlobalVariables.dishInCreation.goals.goal2Star * 6.67f;
        canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/2Star").GetComponent<RectTransform>().anchoredPosition3D = anchoredPosition;

        anchoredPosition = canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/3Star").GetComponent<RectTransform>().anchoredPosition3D;
        anchoredPosition.x = GlobalVariables.dishInCreation.goals.goal3Star * 6.67f;
        canvas.transform.Find("DishFinished/Score/ProgressBarHolder/ProgressBar/3Star").GetComponent<RectTransform>().anchoredPosition3D = anchoredPosition;
    }

    void ShowQualitiesFailedDish()
    {
        canvas.transform.Find("TopUI/ButtonOK").gameObject.SetActive(true);

        Transform contentHolder = canvas.transform.Find("DishFailed/DishHolder/Scroll View/Viewport/Content");
        GameObject newObject;
        float finalQuality = QualityManager.FinalQuality();
        QualityManager.NormalizeQualities();

        foreach (QualityOfPhase q in QualityManager.qualities)
        {
            newObject = GameObject.Instantiate(contentHolder.GetChild(0).gameObject);
            newObject.transform.SetParent(contentHolder, false);

            newObject.transform.Find("NameOfPhase").GetComponent<Text>().text = q.phaseName;
            newObject.transform.Find("QualityTextHolder").GetChild((int)(q.quality) - 1).gameObject.SetActive(true);
        }

        GameObject.Destroy(contentHolder.GetChild(0).gameObject);

        canvas.transform.Find("DishFailed/DishHolder/TitleHolder/Title").GetComponent<Text>().text = GlobalVariables.dishInCreation.name;
    }


    public void LoadDishMenuWhenReturnedToRestaurantScene()
    {
        SceneManager.sceneLoaded += DishMenuSceneLoadedCallBack;
    }

    private void DishMenuSceneLoadedCallBack(Scene arg0, LoadSceneMode arg1)
    {
        GameObject.Find("GroceriesAndRestaurantManager").GetComponent<GroceriesAndRestaurantManager>().OpenDishInformationScreen(GlobalVariables.dishInCreation);
        
        SceneManager.sceneLoaded -= DishMenuSceneLoadedCallBack;

    }

    public void LoadMenuPickerWhenreturnedToRestaurantScene()
    {
        SceneManager.sceneLoaded += MenuPickerSceneLoadedCallBack;
    }

    private void MenuPickerSceneLoadedCallBack(Scene arg0, LoadSceneMode arg1)
    {
        GameObject.Find("GroceriesAndRestaurantManager").GetComponent<GroceriesAndRestaurantManager>().OpenChooseMenuScreenFocusLastUnlocked();
        SceneManager.sceneLoaded -= MenuPickerSceneLoadedCallBack;
    }
}
