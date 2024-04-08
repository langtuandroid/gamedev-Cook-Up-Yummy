using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler {

    public bool isDraggable = true;    //FIXME Enable dragging from game manager

    [Header ("Targets")]
    public int targetsToBeDone;
    public List<Collider2D> targets;  // Array of targets for this item

    [Header("Drag behaviour")]
    public bool lepring;
    public float draggingSpeed;
    public bool backToStartPosition;

    [Header("Resize and Rotate")]
    public bool scaleWhenGrabbed;
    public Vector3 scaleVector;
    public bool resizeWhenGrabbed;
    public Vector2 resizeVector;
    public bool rotateWenGrabbed;
    public Vector3 rotationVector;
    public Vector2 offsetVector;
    [Header ("Item events")]
    public float waitBeforeItemDone;
    public UnityEvent itemDone;
    public UnityEvent onBeginDrag;
    public UnityEvent onEndDrag;
    public UnityEvent onTriggerEnter;

    Transform startParent;
    Vector3 startPos;
    Vector3 startScale;
    Vector2 startDeltaSize;
    Quaternion startRot;
    Vector3 tempVector;
    Vector3 targetMovePosition;
    bool onTargetPlace;
    //pointer movement
    Vector3 lastFramePointerPos;
    Vector3 currentPointerPos;
    Vector3 topLimit;
    float pointerDistance;
    //Target
    TargetConditionBase currentTarget;
    int currentTargetIndex;
    int targetsDone;
    Coroutine lastCoroutine;



    void Start()
    {
        startPos = transform.localPosition;
        startParent = transform.parent;
        startScale = transform.localScale;
        startDeltaSize = GetComponent<RectTransform>().sizeDelta;
        startRot = transform.rotation;
        targetsDone = 0;

        if (GameObject.Find("Canvas/TopLimit") == null)
            throw new System.Exception("There is no 'TopLimit' game object on the scene!");
        else
            topLimit = GameObject.Find("Canvas/TopLimit").transform.position;
    }

    #region Drag interfaces implementation
    public void OnBeginDrag(PointerEventData eventData)
    {

        onBeginDrag.Invoke();
        if (!isDraggable)
            return;
        if (scaleWhenGrabbed)
            transform.localScale = scaleVector;
        if (resizeWhenGrabbed)
            GetComponent<RectTransform>().sizeDelta = resizeVector;
        if (rotateWenGrabbed)
            transform.rotation = Quaternion.Euler(rotationVector);
        transform.SetParent(GameObject.Find("Canvas").transform);

        lastFramePointerPos = Input.mousePosition;

    }
    public void OnDrag(PointerEventData eventData)
    {

        if (!isDraggable)
            return;
        tempVector = Camera.main.ScreenToWorldPoint(eventData.position + offsetVector * 100);
        tempVector.z = 90;
        if (tempVector.y > topLimit.y)
            tempVector.y = topLimit.y;
        targetMovePosition = tempVector;

        if (lepring)
        {
            transform.position = Vector3.Lerp(transform.position, targetMovePosition, draggingSpeed * Time.fixedTime / 1.5f);
            if (Vector3.Distance(transform.localPosition, targetMovePosition) < 0.2f)
            {
                transform.position = targetMovePosition;
            }
        }
        else
        {
            transform.position = targetMovePosition;
        }
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag.Invoke();
        if (backToStartPosition)
            BackToStartPosition();
        else
            transform.SetParent(startParent);
    }
    #endregion

    #region Trigger events
    /// <summary>
    /// Raises the TriggerEnter2D event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter.Invoke();
        Debug.Log("Colission: " + name + " " + other.name);
        if (TargetHit(other))
        {
//            Debug.Log("Target enum: "+currentTarget.name + " " + currentTarget.effectTriggerEnum);
            switch (currentTarget.effectTriggerEnum)
            {
                case(EEffectTriggerEnum.OnTriggerEnter):
                    currentTarget.TargetCondition();
                    break;
                case(EEffectTriggerEnum.OnTriggerStay):
                    onTargetPlace = true;
                    if (lastCoroutine != null)
                        StopCoroutine(lastCoroutine);
                    lastCoroutine = StartCoroutine(CTriggerStay(currentTarget));
                    break;
                case (EEffectTriggerEnum.OnRub):
                    Debug.Log("OnRub switch");
                    onTargetPlace = true;
                    if (lastCoroutine != null)
                        StopCoroutine(lastCoroutine);
                    lastCoroutine = StartCoroutine(CRub(currentTarget));
                    break;
                default:
                    Debug.Log("EffectTriggerEnum type not handeled");
                    break;
            }
        }
    }

    /// <summary>
    /// Cs the trigger stay.
    /// </summary>
    /// <returns>The trigger stay.</returns>
    /// <param name="target">Target.</param>
    IEnumerator CTriggerStay(TargetConditionBase target)
    {
        while (onTargetPlace)
        {
            target.GetComponent<TargetConditionBase>().TargetCondition();
            Debug.Log("TriggerStay: " + target.name);
            yield return new WaitForFixedUpdate(); 
        }
        yield return null;
    }

    /// <summary>
    /// Cs the rub.
    /// </summary>
    /// <returns>The rub.</returns>
    /// <param name="target">Target.</param>
    IEnumerator CRub(TargetConditionBase target)
    {
        while (onTargetPlace)
        {
            currentPointerPos = Input.mousePosition;
            if (Vector3.SqrMagnitude(lastFramePointerPos - currentPointerPos) > 0.5f)    
            {   
                target.GetComponent<TargetConditionBase>().TargetCondition();
                Debug.Log("Rubbing: " + target.name);
            }
            lastFramePointerPos = currentPointerPos;
            yield return new WaitForFixedUpdate(); 
        }
    }

    /// <summary>
    /// Raises the trigger exit2 d event.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (currentTargetIndex >= 0)
        {
            if (targets.Count>currentTargetIndex&&other == targets[currentTargetIndex])
            {
                if (currentTarget != null)
                {
                    currentTarget.TargetEffects_Stop();
                    DisconnectFromTarget();
                }
            }
        }
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Helper method.
    /// </summary>
    /// <returns><c>true</c> if "other" is a valid target, <c>false</c> otherwise.</returns>
    /// <param name="other">Other.</param>
    bool TargetHit(Collider2D other)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == other)
            {
                currentTargetIndex = i;
                currentTarget = other.GetComponent<TargetConditionBase>();    //Get refference to target
                if (currentTarget == null)
                    throw new System.Exception("You are missing a TargetCondition component");
                else
                    currentTarget.event_TargetDone.AddListener(TargetDone);  // subscribe to currentTargets targetDoneEvent
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// This method is subscribed to targets event_TargetDone when OnTriggerEnter occurs. Calls DisconectFromTarget() and ItemDone() if all targets are completed.
    /// </summary>
    /// <param name="target">Target.</param>
    void TargetDone()
    {
        Debug.Log("Target done");
        if(currentTarget.countTarget)
            targetsDone++;
        DisconnectFromTarget();
        if (targetsDone == targetsToBeDone)
            ItemDone();
    }

    void ItemDone()
    {
        Debug.Log("Item is done: " + name);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        isDraggable = false;
        if (backToStartPosition)
            BackToStartPosition();
        Debug.Log("crtn started");
        StartCoroutine(CItemDone());
    }

    IEnumerator CItemDone()
    {
        yield return new WaitForSeconds(1f);
        itemDone.Invoke();
    }

    /// <summary>
    /// Resets flags, stops coroutines and unsubscribes from events.
    /// </summary>
    void DisconnectFromTarget()
    {
        if (currentTarget == null)
            return;
        Debug.Log("DisconnectFromTarget");
        onTargetPlace = false;
        if(lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        currentTarget.event_TargetDone.RemoveListener(TargetDone);
        
        currentTarget = null;
//        Debug.Log("Disconeted");
    }

    /// <summary>
    /// Returns item to start position and reset its transform state (parent,scale,size,rotation)
    /// </summary>
    void BackToStartPosition()
    {
        transform.SetParent(startParent);
        if (scaleWhenGrabbed)
            transform.localScale = startScale;
        if (resizeWhenGrabbed)
            GetComponent<RectTransform>().sizeDelta = startDeltaSize;
        if (rotateWenGrabbed)
            transform.rotation = startRot;
        StartCoroutine(LerpToTargetPos(startPos));
    }

    IEnumerator LerpToTargetPos(Vector3 localTargetPos)
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.localPosition = Vector3.Lerp(transform.localPosition,localTargetPos, 30 * Time.deltaTime / 1.5f);
            if (Vector3.Distance(transform.localPosition, localTargetPos) < 0.2f)
            {
                transform.localPosition = localTargetPos;
                break;
            }
        }
    }

    public int GetCurrentTargetIndex()
    {
        return currentTargetIndex;
    }

    public void DecreseTargetsToBeDone()
    {
        targetsToBeDone--;
        if (targetsToBeDone == targetsDone)
            ItemDone();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            OnEndDrag(null);
        }
    }
    #endregion

}
