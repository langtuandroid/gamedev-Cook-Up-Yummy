using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public float time;
    public string sceneToLoad;
    public Image progressBar;
    void Start()
    {
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneToLoad);
    }
   
}
