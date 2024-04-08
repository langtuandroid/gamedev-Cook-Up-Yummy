using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFlyToPoint : MonoBehaviour
{
    public Transform endPoint;


    private Vector3 startPoint;

    public void Fly()
    {
        StartCoroutine(flyToPoint());
    }

    IEnumerator flyToPoint()
    {
        startPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        float a = 0f;
        while(a<1f)
        {
            a += 0.05f;
            yield return new WaitForSeconds(.01f);
            transform.position = Vector3.Lerp(startPoint, endPoint.transform.position, a);
        }
        //endPoint.gameObject.SetActive(true);
    }
}
