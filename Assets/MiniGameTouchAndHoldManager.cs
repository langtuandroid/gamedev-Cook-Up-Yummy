using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameTouchAndHoldManager : MonoBehaviour
{
    Dish dish;
    Image tapZone;
    GameObject canvas;

    Transform gameVersionHolder, prefabHolder;

    GameObject gameVersion, prefab;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        Toast.Instance.Reset();
        tapZone = canvas.transform.Find("TapZone").GetComponent<Image>();
        gameVersionHolder = canvas.transform.Find("VersionPrefabHolder");
        prefabHolder = canvas.transform.Find("PrefabHolder");

        dish = GlobalVariables.dishInCreation;
        gameVersion = GameObject.Instantiate(dish.creationPhases[GlobalVariables.creationPhaseIndex].gameVersionPrefab);
        gameVersion.transform.SetParent(gameVersionHolder, false);
        prefab = GameObject.Instantiate(dish.creationPhases[GlobalVariables.creationPhaseIndex].ingredient.prefabTouchAndHold);
        prefab.transform.SetParent(prefabHolder, false);

        canvas.transform.Find("ProgressBar").GetComponent<GoalProgressBar>().SetGoalValue(dish.creationPhases[GlobalVariables.creationPhaseIndex].barPerfectGoal);
        
        if(!TutorialTouchAndHold.done)
        {
            TutorialTouchAndHold.Instance.InstantiateVersionPrefab(dish.creationPhases[GlobalVariables.creationPhaseIndex].gameVersionPrefab);
        }
    }

    public void Touch()
    {
        prefab.GetComponent<Animator>().SetTrigger("Start");
        GoalProgressBar.StartArrowBarMoving(dish.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit);
        tapZone.raycastTarget = false;
        if (gameVersionHolder.GetChild(0) != null && gameVersionHolder.GetChild(0).GetComponent<Animator>() != null)
            gameVersionHolder.GetChild(0).GetComponent<Animator>().enabled = true;

        if(dish.creationPhases[GlobalVariables.creationPhaseIndex].soundEffect!=null)
        {
            if(SoundManager.Instance!=null)
                SoundManager.Instance.PlayEffect(dish.creationPhases[GlobalVariables.creationPhaseIndex].soundEffect);
        }
    }

    public void TouchEnded()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopEffect();
        float a = GoalProgressBar.Stop();
        prefab.GetComponent<Animator>().SetTrigger("Stop");
        
        float diff = Mathf.Abs(a - dish.creationPhases[GlobalVariables.creationPhaseIndex].barPerfectGoal);
        Quality quality = Quality.Failed;
        if (diff <= QualityManager.PerfectTolerance)
            quality = Quality.Perfect;
        else if (diff <= QualityManager.GreatTolerance)
            quality = Quality.Great;
        else if (diff <= QualityManager.GoodTolerance)
            quality = Quality.Good;
        else if (diff <= QualityManager.OKTolerance)
            quality = Quality.OK;

        Toast.Instance.Show((int)quality);

        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Touch and hold";

        QualityManager.qualities.Add(qp);


        if (quality == Quality.Failed)
            MiniGameFailed();
        else
        {
            Timer.Schedule(this, 2f, delegate
            {
                MiniGameDone();
            });
        }

    }

    private void MiniGameFailed()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopEffect();
        Toast.Instance.Show(1);
        Toast.Stay();
        Timer.Schedule(this, 2.5f, delegate
          {
              EndScreenManager.failed = true;
              canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
          });
        //SceneManager.LoadScene("EndGameScreen");
    }

    public void MiniGameDone()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopEffect();
        Toast.Stay();
        Timer.Schedule(this, 2.5f, delegate
          {
              Debug.Log("*********MINIGAME TOUCH AND HOLD DONE*********");
              GlobalVariables.creationPhaseIndex++;
              if (dish.creationPhases.Count > GlobalVariables.creationPhaseIndex)
              {
                  if (SceneManager.GetActiveScene().name == dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName)
                      canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                  else
                      canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
              }
              else
              {
                  Debug.Log("JELO GOTOVO!");
                  canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
              }
          });
    }
}
