using UnityEngine;
using System.Collections;

public class Timer
{
    private static MonoBehaviour behaviour;
    public delegate void delegat<T>(T param);
    public delegate void delegat();

    public static void Schedule<T>(MonoBehaviour _behaviour, float delay, delegat<T> task,T param)
    {
        behaviour = _behaviour;
        behaviour.StartCoroutine(Execute(task,param, delay));
    }

    private static IEnumerator Execute<T>(delegat<T> task,T param, float delay)
    {
        yield return new WaitForSeconds(delay);
        task(param);
    }

    public static void Schedule(MonoBehaviour _behaviour, float delay, delegat task)
    {
        behaviour = _behaviour;
        behaviour.StartCoroutine(Execute(task, delay));
    }

    private static IEnumerator Execute(delegat task, float delay)
    {
        yield return new WaitForSeconds(delay);
        task();
    }
}
