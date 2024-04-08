using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool keepImageEnabled = false;
    public bool enableImageAtEnd = false;
    public Transform endPos;

    Vector3 startPos;
    Vector3 tmpPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tmpPos.z = 0;
        transform.position = tmpPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform==endPos)
        {
            transform.position = collision.transform.position;
            transform.SetParent(endPos, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            MiniGameDragTheIngredientsManager.Instance.IngredientDragged();
            if (!keepImageEnabled)
                this.GetComponent<Image>().enabled = false;
            if (enableImageAtEnd)
                endPos.GetComponent<Image>().enabled = true;
            this.enabled = false;
        }
    }
}
