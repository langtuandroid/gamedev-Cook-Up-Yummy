using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameKneadingTheDoughManager : MonoBehaviour
{
    public static MiniGameKneadingTheDoughManager Instance;

    public Animator doughAnimator;

    float maxTime=8f;


    Dish inCreation;

    private void Awake()
    {
        Instance = this;
        canvas = GameObject.Find("Canvas");
    }
    GameObject canvas;

    void Start()
    {
        Toast.Instance.Reset();
        inCreation = GlobalVariables.dishInCreation;
        maxTime = inCreation.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit;
        MiniGameTimer.StartTimer(maxTime);
        MiniGameTimer.timeOut.AddListener(delegate
        {
            TimerExpired();
        });
    }

    public void Done()
    {
        Toast.Stay();
        float remainingTime = MiniGameTimer.StopTimer();
        float factor = remainingTime / inCreation.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit;
        factor *= 5;
        int rounded = (int)Mathf.Ceil(factor);
        Quality quality = (Quality)rounded;

        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Kneading the dough";

        QualityManager.qualities.Add(qp);
        Debug.Log("KVALITET " + quality.ToString());

        Toast.Instance.Show((int)quality);

        if (quality != Quality.Failed)
        {
            GlobalVariables.creationPhaseIndex++;
            if (GlobalVariables.creationPhaseIndex >= inCreation.creationPhases.Count)
            {
                Debug.Log("JELO GOTOVO");
            }
            else
            {
                Toast.Instance.Show((int)quality);
                Timer.Schedule(this, 3f, delegate
                {
                    canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                });
            }
        }
        else
            MiniGameFailed();
    }

    public void TimerExpired()
    {
        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = Quality.Failed;
        qp.phaseName = "Kneading the dough";
        QualityManager.qualities.Add(qp);

        MiniGameFailed();
    }

    void MiniGameFailed()
    {
        Toast.Instance.Show((int)Quality.Failed);
        Toast.Stay();
        Timer.Schedule(this, 3f, delegate
        {
            EndScreenManager.failed = true;
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
        });
    }
}
