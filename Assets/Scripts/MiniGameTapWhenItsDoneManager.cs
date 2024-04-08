using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTapWhenItsDoneManager : MonoBehaviour
{
    public static MiniGameTapWhenItsDoneManager Instance;
    public Dish dish;

    Quality quality;

    GameObject canvas;
    Transform versionPrefabHolder, prefabHolder;
    GameObject versionPrefab, prefab;

    private bool done = false;  
    private void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        canvas = GameObject.Find("Canvas");
        prefabHolder = canvas.transform.Find("PrefabHolder");
        versionPrefabHolder = canvas.transform.Find("VersionPrefabHolder");
        Toast.Instance.Reset();
        GoalProgressBar.timeOut.RemoveListener(delegate
        {
            TimerExpired();
        });
        GoalProgressBar.timeOut.AddListener(delegate
        {
            TimerExpired();
        });

        dish = GlobalVariables.dishInCreation;

        versionPrefab = GameObject.Instantiate(dish.creationPhases[GlobalVariables.creationPhaseIndex].gameVersionPrefab);
        versionPrefab.transform.SetParent(versionPrefabHolder, false);
        if (dish.creationPhases[GlobalVariables.creationPhaseIndex].ingredient!=null && dish.creationPhases[GlobalVariables.creationPhaseIndex].ingredient.prefabTapWhenItsDone != null)
        {
            prefab = GameObject.Instantiate(dish.creationPhases[GlobalVariables.creationPhaseIndex].ingredient.prefabTapWhenItsDone);
            prefab.transform.SetParent(prefabHolder, false);
        }
        canvas.transform.Find("ProgressBar").GetComponent<GoalProgressBar>().SetGoalValue(dish.creationPhases[GlobalVariables.creationPhaseIndex].barPerfectGoal);

        yield return new WaitForSeconds(.8f);

        if (dish.creationPhases[GlobalVariables.creationPhaseIndex].soundEffect != null)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayEffect(dish.creationPhases[GlobalVariables.creationPhaseIndex].soundEffect);

        }

        GoalProgressBar.StartArrowBarMoving(dish.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit);
        if(versionPrefabHolder.GetChild(0).GetChild(0).GetComponent<Animator>()!=null)
            versionPrefabHolder.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Start");
    }

    public void Done()
    {
        if (!done)
        {
            done = true;
            if (SoundManager.Instance != null)
                SoundManager.Instance.StopEffect();
            if (versionPrefabHolder.GetChild(0).GetChild(0).GetComponent<Animator>() != null)
                versionPrefabHolder.GetChild(0).GetChild(0).GetComponent<Animator>().SetTrigger("Stop");
            float a = GoalProgressBar.Stop();

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

            QualityOfPhase qp = new QualityOfPhase();
            qp.quality = quality;
            qp.phaseName = "Tap at the right time";

            QualityManager.qualities.Add(qp);


            Toast.Instance.Show((int)quality);

            if (quality == Quality.Failed)
            {
                MiniGameFailed();
            }
            else
            {
                Timer.Schedule(this, 1f, delegate
                {
                    MiniGameDone();
                });
            }
        }
    }

    private void MiniGameFailed()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopEffect();
        Toast.Stay();
        Timer.Schedule(this, 3.8f, delegate
        {
            EndScreenManager.failed = true;
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
            //SceneManager.LoadScene("EndGameScreen");
        });
    }

    public void TimerExpired()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopEffect();
        quality = Quality.Failed;
        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Tap at the right time";
        QualityManager.qualities.Add(qp);
        Toast.Instance.Show((int)quality);
        MiniGameFailed();

    }

    void MiniGameDone()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.StopEffect();
        canvas.transform.Find("TapZone").gameObject.SetActive(false);
        Toast.Stay();
        Debug.Log("*********MINIGAME TAP WHEN ITS DONE DONE*********");
        GlobalVariables.creationPhaseIndex++;

        Timer.Schedule(this, 3.8f, delegate
          {
              if (dish.creationPhases.Count > GlobalVariables.creationPhaseIndex)
              {
                  if (SceneManager.GetActiveScene().name == dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName)
                      canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                  else
                      canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(dish.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
              }
              else
              {
                  Debug.Log("JELO GOTOVO");
                  canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
              }
          });
    }
}
