using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSliceManager : MonoBehaviour
{
    public float threshold=1f;

    Dish inCreation;
    
    Transform activeSliceLine;
    Vector3 point1, point2;
    GameObject canvas;
    Transform prefabHolder;
    int startPointIndex = 0;
    Vector3 startPoint;

    GameObject ingredientPrefabInstantiate;

    Vector3[] tmpCorners=new Vector3[4];
    Vector3 mousePos;

    Quality quality;

    bool gameDone=false;

    void Start()
    {
        Toast.Instance.Reset();
        canvas = GameObject.Find("Canvas");
        prefabHolder = canvas.transform.Find("PrefabHolder");
        inCreation = GlobalVariables.dishInCreation;

        ingredientPrefabInstantiate = GameObject.Instantiate(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].ingredient.prefabSlice);
        ingredientPrefabInstantiate.transform.SetParent(prefabHolder, false);

        MiniGameTimer.timeOut.RemoveAllListeners();
        MiniGameTimer.timeOut.AddListener(delegate
        {
            MiniGameFailed();
        });


        MiniGameTimer.StartTimer(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit);
        



        /*
         * 
         */
        //ingredientPrefabInstantiate = prefabHolder.transform.GetChild(0).gameObject;
        //MiniGameTimer.StartTimer(5f);
        
        /** 
         * **/

        activeSliceLine = prefabHolder.GetChild(0).transform.Find("Lines").GetChild(0);
        activeSliceLine.gameObject.SetActive(true);
        point1 = activeSliceLine.GetChild(0).transform.position;
        point2 = activeSliceLine.GetChild(1).transform.position;

    }

  

    public void MouseDown()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPoint = mousePos;
        if (Vector3.Distance(mousePos, point1) > Vector3.Distance(mousePos, point2))
            startPointIndex = 2;
        else
            startPointIndex = 1;
    }

    public void MouseUp()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float diff=0;

        //if(startPointIndex==1)
        //{
        //    Debug.Log(startPoint + " " + point1);
        //    diff = Vector2.Distance(startPoint, point1);
        //    diff += Vector2.Distance(mousePos, point2);
        //    Debug.Log(mousePos + " " + point2);
        //}
        //else if(startPointIndex==2)
        //{
        //    diff = Vector2.Distance(startPoint, point2);
        //    diff += Vector2.Distance(mousePos, point1);
        //}


        //OVO JE FIX ZA X, TREBA I ZA Y
        if (startPointIndex == 1)
        {
            Debug.Log(startPoint + " " + point1);
            diff = Vector2.Distance(startPoint, point1);
            diff += Vector2.Distance(mousePos, point2);
            Debug.Log(mousePos + " " + point2);
            if ((int)activeSliceLine.GetComponent<RectTransform>().rotation.eulerAngles.z != 0)
            {
                if (diff > threshold && Mathf.Abs(startPoint.x - point1.x) < 0.2f && Math.Abs(startPoint.y - mousePos.y) > 0.5f)
                    diff = 0.5f;
            }
            else
            {
                if (diff > threshold && Mathf.Abs(startPoint.y - point1.y) < 0.2f && Math.Abs(startPoint.x - mousePos.x) > 0.5f)
                    diff = .5f;
            }

        }
        else if (startPointIndex == 2)
        {
            diff = Vector2.Distance(startPoint, point2);
            diff += Vector2.Distance(mousePos, point1);
            if ((int)activeSliceLine.GetComponent<RectTransform>().rotation.eulerAngles.z != 0)
            {
                if (diff > threshold && Mathf.Abs(startPoint.x - point2.x) < 0.2f && Math.Abs(startPoint.y - mousePos.y) > 0.5f)
                    diff = .5f;
            }
            else
            {
                if (diff > threshold && Mathf.Abs(startPoint.y - point2.y) < 0.2f && Math.Abs(startPoint.x - mousePos.x) > 0.5f)
                    diff = .5f;
            }
        }

        Debug.Log("DIFF " + diff);
        if(diff<threshold&&!gameDone)
        {
            Vector3 posToSet;
            Debug.Log("ISECENO");
            if (SoundManager.Instance != null)
                SoundManager.Instance.Play_KnifeCut();
            int indexOfSliceLine = activeSliceLine.GetSiblingIndex();
            for(int i=0;i<=indexOfSliceLine;i++)
            {
                posToSet=ingredientPrefabInstantiate.transform.Find("Parts").GetChild(i).GetComponent<RectTransform>().anchoredPosition3D;
                if ((int)activeSliceLine.GetComponent<RectTransform>().rotation.eulerAngles.z !=0)
                    posToSet.x -= 10f;
                else
                    posToSet.y -= 30f;
                ingredientPrefabInstantiate.transform.Find("Parts").GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = posToSet;
            }

            activeSliceLine.gameObject.SetActive(false);
            if(ingredientPrefabInstantiate.transform.Find("Lines").childCount>indexOfSliceLine+1)
            {
                activeSliceLine = ingredientPrefabInstantiate.transform.Find("Lines").GetChild(indexOfSliceLine + 1);
                indexOfSliceLine++;
                activeSliceLine.gameObject.SetActive(true);
                point1 = activeSliceLine.GetChild(0).transform.position;
                point2 = activeSliceLine.GetChild(1).transform.position;
            }
            else
            {
                MiniGameDone();
            }
        }
    }

    void MiniGameDone()
    {
        Toast.Stay();
        float remainingTime = MiniGameTimer.StopTimer();
        gameDone = true;
        float factor = remainingTime / inCreation.creationPhases[GlobalVariables.creationPhaseIndex].timeLimit;
        factor *= 5;

        int rounded = (int)Mathf.CeilToInt(factor);

        Quality quality = (Quality)rounded;
        

        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Slice along the line";

        QualityManager.qualities.Add(qp);
        Debug.Log("KVALITET " + quality.ToString());

        Toast.Instance.Show((int)quality);

        if (quality != Quality.Failed)
        {
            Timer.Schedule(this, 3.8f, delegate
              {
                  Debug.Log("MINI IGRA SECKANJE ZAVRSENA");
                  GlobalVariables.creationPhaseIndex++;
                  if (inCreation.creationPhases.Count > GlobalVariables.creationPhaseIndex)
                  {
                      if (SceneManager.GetActiveScene().name == inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName)
                          canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                      else
                          canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                      //SceneManager.LoadScene(inCreation.creationPhases[GlobalVariables.creationPhaseIndex].sceneName);
                  }
                  else
                  {
                      canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
                      Debug.Log("JELO GOTOOVo");
                  }
              });
        }
        else
        {
            MiniGameFailed();
        }
    }

    void MiniGameFailed()
    {
        gameDone = true;
        Toast.Stay();
        Quality quality = Quality.Failed;
        QualityOfPhase qp = new QualityOfPhase();
        qp.quality = quality;
        qp.phaseName = "Slice along the line";
        Toast.Instance.Show((int)quality);

        QualityManager.qualities.Add(qp);
        Timer.Schedule(this, 3.8f, delegate
        {
            EndScreenManager.failed = true;
            canvas.GetComponent<MenuManager>().LoadSceneWithComingTransition("EndGameScreen");
        });
    }
}
