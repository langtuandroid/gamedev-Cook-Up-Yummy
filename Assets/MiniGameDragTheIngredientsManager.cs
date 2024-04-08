using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameDragTheIngredientsManager : MonoBehaviour
{
    public static MiniGameDragTheIngredientsManager Instance;
    Dish inCreation;

    GameObject prefab;
    int ingredientsToDrag;
    GameObject canvas;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        Toast.Instance.Reset();
        inCreation = GlobalVariables.dishInCreation;
        prefab = GameObject.Instantiate(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].gameVersionPrefab);
        prefab.transform.SetParent(canvas.transform.Find("VersionPrefabHolder"), false);
        foreach (Transform t in prefab.transform)
            if (t.name.Contains("Start"))
                ingredientsToDrag++;


        MiniGameTimer.StartTimer(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit);
        

        MiniGameTimer.timeOut.RemoveAllListeners();
        MiniGameTimer.timeOut.AddListener(delegate
        {
            MiniGameFailed();
        });

    }
   

    public void IngredientDragged()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play_DragIngredientCompletedSound();
        ingredientsToDrag--;
        if (ingredientsToDrag <= 0)
            MiniGameDone();
    }

    void MiniGameDone()
    {
        Toast.Stay();
        float remainingTime=MiniGameTimer.StopTimer();

        float factor = remainingTime / inCreation.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit;
        factor *= 5;

        int rounded = (int)Mathf.Ceil(factor);
        if (rounded == 0)
            rounded = 1;
        Quality quality=(Quality)rounded;
        
        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Drag the ingredients";

        QualityManager.qualities.Add(qp);
        Debug.Log("KVALITET " + quality.ToString());

        if (quality != Quality.Failed)
        {
            Toast.Instance.Show((int)quality);
            Timer.Schedule(this, 3.8f, delegate
            {
                Debug.Log("*********MINIGAME DRAG THE INGREDIENTS DONE*********");
                GlobalVariables.creationPhaseIndex++;
                if (inCreation.creationPhases.Count > GlobalVariables.creationPhaseIndex)
                {
                    canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                    //SceneManager.LoadScene(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                }
                else
                {
                    canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
                    Debug.Log("Jelo gotovo!");
                }
            });
        }
        else
        {
            Toast.Instance.Show((int)quality);
            Toast.Stay();
            MiniGameFailed();
        }
    }

    void MiniGameFailed()
    {
        Toast.Stay();

        EndScreenManager.failed = true;
        Toast.Instance.Show((int)Quality.Failed);
        Timer.Schedule(this, 3.8f, delegate
        {
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
        });
        //SceneManager.LoadScene("EndGameScreen");
    }
}