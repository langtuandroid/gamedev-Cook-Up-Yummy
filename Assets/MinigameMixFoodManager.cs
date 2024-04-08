using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameMixFoodManager : MonoBehaviour
{

    GameObject canvas;
    Transform prefabHolder;

    int phaseIndex;
    Dish dish;
    GameObject prefab;


    private int timesToClick = 3;
    private int clickCount;

    void Start()
    {
        clickCount = 0;
        dish = GlobalVariables.dishInCreation;
        phaseIndex = GlobalVariables.creationPhaseIndex;
        if (canvas == null)
            canvas = GameObject.Find("Canvas");
        if (prefabHolder == null)
            prefabHolder = canvas.transform.Find("PrefabHolder");

        Toast.Instance.Reset();

        if(dish.creationPhases.Count>phaseIndex)
        {
            if(dish.creationPhases[phaseIndex].gameVersionPrefab!=null)
                prefab = GameObject.Instantiate(dish.creationPhases[phaseIndex].gameVersionPrefab);

            prefab.transform.SetParent(prefabHolder, false);
        }
    }

    public void Clicked()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play_MixingFood();
        if (clickCount + 1 < timesToClick)
        {
            float diff = GoalCircle.ChangeDirection();
            clickCount++;
            prefab.GetComponent<Animator>().SetTrigger("next");

            diff /= 360;

            Quality quality = Quality.Failed;
            if (diff <= QualityManager.PerfectTolerance)
                quality = Quality.Perfect;
            else if (diff <= QualityManager.GreatTolerance)
                quality = Quality.Great;
            else if (diff <= QualityManager.GoodTolerance)
                quality = Quality.Good;
            else if (diff <= QualityManager.OKTolerance)
                quality = Quality.OK;

            QualityOfPhase qp = new QualityOfPhase();
            qp.quality = quality;
            qp.phaseName = "Mix food";

            QualityManager.qualities.Add(qp);

            if (quality == Quality.Failed)
                MiniGameFailed();

            Toast.Instance.Show((int)quality,1.5f);
            GoalCircle.SetZoneAtRandomPos();

        }
        else
        {
            //GoalCircle.SetZoneAtRandomPos();
            float diff = GoalCircle.StopMoving();
            clickCount++;
            prefab.GetComponent<Animator>().SetTrigger("next");

            diff /= 360;

            Quality quality = Quality.Failed;
            if (diff <= QualityManager.PerfectTolerance)
                quality = Quality.Perfect;
            else if (diff <= QualityManager.GreatTolerance)
                quality = Quality.Great;
            else if (diff <= QualityManager.GoodTolerance)
                quality = Quality.Good;
            else if (diff <= QualityManager.OKTolerance)
                quality = Quality.OK;

            QualityOfPhase qp = new QualityOfPhase();
            qp.quality = quality;
            qp.phaseName = "Mix food";

            QualityManager.qualities.Add(qp);


            if (quality == Quality.Failed)
                MiniGameFailed();

            Toast.Instance.Show((int)quality,1.5f);
        }


        if (clickCount + 1 > timesToClick)
        {
            MiniGameDone();
        }
    }

    public void MiniGameDone()
    {
        canvas.transform.Find("TapZone").gameObject.SetActive(false);
        Debug.Log("*********MINIGAME MIXFOOD DONE*********");
        Toast.Stay();
        GlobalVariables.creationPhaseIndex++;
        prefab.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        GoalCircle.StopMoving();
        Timer.Schedule(this, 3.8f, delegate
        {
            if (dish.creationPhases.Count > GlobalVariables.creationPhaseIndex)
            {
                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                //SceneManager.LoadScene(dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
            }
            else
            {
                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
            }
        });
    }

    void MiniGameFailed()
    {
        Quality quality = Quality.Failed;
        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Mix food";
        Toast.Stay();
        QualityManager.qualities.Add(qp);

        Timer.Schedule(this, 1f, delegate
        {
            EndScreenManager.failed = true;
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
        });
       
        //SceneManager.LoadScene("EndGameScreen");
    }

}
