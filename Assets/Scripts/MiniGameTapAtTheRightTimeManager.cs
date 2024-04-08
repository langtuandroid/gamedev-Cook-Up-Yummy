using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MiniGameTapAtTheRightTimeManager : MonoBehaviour
{
    private Dish currentlyMaking;
    private DishCreationPhase phase;
    private Transform prefabHolder;
    private GameObject canvas;
    private GameObject progressBar;

    private int amountToDo;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        Toast.Instance.Reset();
        if (prefabHolder == null)
            prefabHolder = canvas.transform.Find("PrefabHolder");
        if (progressBar == null)
            progressBar = canvas.transform.Find("ProgressBar").gameObject;

        currentlyMaking = GlobalVariables.dishInCreation;
        phase = currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex];
        progressBar.GetComponent<GoalProgressBar>().SetGoalValue(currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex].barPerfectGoal);
        StartMiniGame();

        if (phase.ingredientAmount > 1)
            amountToDo = phase.ingredientAmount;
        else
            amountToDo = 1;
    }

    public void Tapped()
    {
        canvas.transform.Find("TapZone").gameObject.SetActive(false);
        amountToDo--;
        float a=GoalProgressBar.Stop();
        prefabHolder.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Start");
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play_EggBreak();
        Debug.Log("Valjanost "+a.ToString());

        float diff = Mathf.Abs(a - currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex].barPerfectGoal);

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


        if (quality > Quality.Failed)
        {
            Toast.Instance.Show((int)quality);
            if (amountToDo > 0)
            {
                Debug.Log("DURATION " + prefabHolder.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
                Toast.Stay();

                Timer.Schedule(this, 3f, delegate
                {
                    canvas.GetComponent<MenuManager>().BlackTransition();
                });
                Timer.Schedule(this, 3.8f, delegate
                  {
                      Toast.Instance.Reset();
                      StartMiniGame();
                  });
            }
            else
            {
                Timer.Schedule(this, 3.8f, delegate
                {
                    MiniGameDone();
                });
            }
        }
        else
        {
            Toast.Instance.Show((int)quality);
            EndScreenManager.failed = true;
            Timer.Schedule(this, 3.8f, delegate
            {
                canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
            });
            //SceneManager.LoadScene("EndGameScreen");
        }
    }

    void StartMiniGame()
    {
        canvas.transform.Find("TapZone").gameObject.SetActive(true);

        GoalProgressBar.StartArrowBarMoving(currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit);
        Toast.Instance.Reset();
        if(prefabHolder.childCount>0)
        {
            for (int i = prefabHolder.childCount - 1; i >= 0; i--)
                GameObject.Destroy(prefabHolder.GetChild(i).gameObject);
        }

        GameObject prefab = GameObject.Instantiate(currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex].ingredient.prefabTapAtTheRightTime);
        prefab.transform.SetParent(prefabHolder, false);
    }

    void MiniGameDone()
    {
        Toast.Stay();
        EndScreenManager.failed = false;
        Debug.Log("*********MINIGAME TAP AT THE RIGHT TIME DONE*********");
        GlobalVariables.creationPhaseIndex++;
        if (currentlyMaking.creationPhases.Count > GlobalVariables.creationPhaseIndex)
        {
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
            //SceneManager.LoadScene(currentlyMaking.creationPhases[GlobalVariables.creationPhaseIndex].sceneName); 
        }
        else
        {
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
        }
    }
}
