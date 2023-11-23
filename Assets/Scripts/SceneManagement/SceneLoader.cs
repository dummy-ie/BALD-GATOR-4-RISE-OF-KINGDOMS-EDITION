using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string _sceneName;
    public void LoadSceneWithoutFade(string sceneName)
    {
        _sceneName = sceneName;
        SceneManager.LoadScene(_sceneName);
    }

    private IEnumerator FadeAndLoadScene()
    {
        yield return ScreenFader.Instance.FadeOut();
        SceneManager.LoadScene(_sceneName);
        yield return ScreenFader.Instance.FadeIn();
    }

    private IEnumerator FadeAndLoadAsyncScene()
    {
        Debug.Log("yea");
        yield return ScreenFader.Instance.FadeOut();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log("grr");
            yield return null;
        }
        yield return ScreenFader.Instance.FadeIn();
    }

    public void LoadScene(string sceneName, bool async = true)
    {
        _sceneName = sceneName;
        if (async)
        {
            StartCoroutine(FadeAndLoadAsyncScene());
        }
        else
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }
}
