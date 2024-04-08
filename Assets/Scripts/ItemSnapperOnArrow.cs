using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSnapperOnArrow : MonoBehaviour
{
    public Transform itemsToSnapHolder;
    //carousel NOT implemented
    bool carousel;
    public int startIndex;
    public Button arrowLeft, arrowRight;
    public Transform leftOutSidePoint, rightOutSidePoint;

    int currentIndex;

    private void Start()
    {
        currentIndex = startIndex;

        if (currentIndex == 0)
            arrowLeft.gameObject.SetActive(false);

        int counter = 0;
        foreach(Transform t in itemsToSnapHolder)
        {
            if (currentIndex == counter)
                t.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            else if (counter<currentIndex)
            {
                t.position = leftOutSidePoint.position;
            }
            else
            {
                t.position = rightOutSidePoint.position;
            }
            counter++;
        }
            
    }

    private void OnEnable()
    {
        Start();
    }



    public void GoRight()
    {
        if (!carousel)
        {
            if (currentIndex != itemsToSnapHolder.childCount - 1)
            {
                int oldIndex = currentIndex;
                currentIndex = (currentIndex + 1) % itemsToSnapHolder.childCount;
                StartCoroutine(Move(itemsToSnapHolder.GetChild(currentIndex), Vector3.zero));
                StartCoroutine(Move(itemsToSnapHolder.GetChild(oldIndex), leftOutSidePoint.position));
                if (currentIndex == itemsToSnapHolder.childCount - 1)
                {
                    arrowRight.gameObject.SetActive(false);
                }
                arrowLeft.gameObject.SetActive(true);

            }
        }
        else
        {

        }
    }

    public void GoLeft()
    {
        if (!carousel)
        {
            if (currentIndex != 0)
            {
                int oldIndex = currentIndex;
                currentIndex = (currentIndex - 1) % itemsToSnapHolder.childCount;
                StartCoroutine(Move(itemsToSnapHolder.GetChild(currentIndex), Vector3.zero));
                StartCoroutine(Move(itemsToSnapHolder.GetChild(oldIndex), rightOutSidePoint.position));
                if (currentIndex == 0)
                {
                    arrowLeft.gameObject.SetActive(false);
                }
                    arrowRight.gameObject.SetActive(true);
            }
        }
        else
        {

        }
    }

    IEnumerator Move(Transform t1,Vector3 end)
    {
        float a = 0f;
        Vector3 startpos = t1.position;
        end.z = 0f;
        while (a < 1f)
        {
            t1.position = Vector2.Lerp(startpos, end, a);
            a += 0.025f;
            yield return new WaitForEndOfFrame();
        }
        startpos = t1.GetComponent<RectTransform>().anchoredPosition3D;
        startpos.z = 0f;
        t1.GetComponent<RectTransform>().anchoredPosition3D = startpos;
    }
}
