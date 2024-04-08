using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicEffects_ItemGroupManager : MonoBehaviour {

    public List<GameObject> items;
    public UnityEvent finishedGroupOfItems;

    int counter;

    void Start()
    {
        counter = 0;
    }

    public void SetCounterToZero()
    {
        counter = 0;
    }

    public void SetCounterToMax()
    {
        counter = items.Count - 1;
    }

    public void Effect_ActivateOneByOneForward(bool destroyPrevious)
    {
        if (destroyPrevious)
            Destroy(items[counter]);
        counter++;
        Debug.Log("Counter: " + counter +" List count: " + items.Count);
        if (counter < items.Count)
        {
            items[counter].SetActive(true);
        }
        else
        {
            finishedGroupOfItems.Invoke();
            Debug.Log("Group of items finished");
        }
    }

    public void Effect_DeactivateOneByOneForward(bool destroyPrevious)
    {
        items[counter].SetActive(false);
        if (destroyPrevious)
            Destroy(items[counter]);
        counter++;
        if (counter >= items.Count)
            finishedGroupOfItems.Invoke();
    }

}
