using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliceLine : MonoBehaviour ,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    public Vector3 pos = Vector3.zero;
    Vector2 ratio;
    Vector3 startScreenPos;
    void Start()
    {
        Resolution deviceRes = Screen.currentResolution;
        GameObject canvas = GameObject.Find("Canvas");
        ratio = new Vector3(Screen.width / canvas.GetComponent<CanvasScaler>().referenceResolution.x, Screen.height / canvas.GetComponent<CanvasScaler>().referenceResolution.y, 1f);
    }

    public UnityEngine.UI.Extensions.UILineRenderer lineRenderer;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 posToSet= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startScreenPos = Input.mousePosition;
        posToSet.z = 0;
        lineRenderer.transform.position = posToSet;
        lineRenderer.Points[0] = new Vector2(0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 posBeforeCalc = Input.mousePosition;

        pos.x = posBeforeCalc.x - startScreenPos.x;
        pos.y = posBeforeCalc.y - startScreenPos.y;
        //pos.x = posBeforeCalc.x / ratio.x;
        //pos.y = posBeforeCalc.y / ratio.x;
        pos.x /= ratio.y;
        pos.y /= ratio.y;
        pos.z = 0;

        lineRenderer.Points[0] = pos;
        lineRenderer.SetVerticesDirty();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lineRenderer.Points[0] = Vector2.zero;
        lineRenderer.SetVerticesDirty();
    }

}
