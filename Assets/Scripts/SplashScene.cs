using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    public string sceneToLoad = "HomeScene";

    void Start()
    {
        StartCoroutine(DelayAndLoadScene());
    }

    IEnumerator DelayAndLoadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneToLoad);
    }

  
}
