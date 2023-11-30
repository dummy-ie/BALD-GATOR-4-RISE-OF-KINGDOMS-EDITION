using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string _sceneName;
    private SceneData _activeScene;
    public SceneData ActiveScene {
        get { return _activeScene; }
    }

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
    public void LoadSceneWithoutFade(AssetReference sceneData)
    {
        AsyncOperationHandle handle = sceneData.LoadAssetAsync<SceneData>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                StartCoroutine(SceneLoad((SceneData)sceneData.Asset));
            else
                Debug.LogError($"{sceneData.RuntimeKey}.");
        };
    }
    public void LoadSceneWithFade(AssetReference sceneData)
    {
        AsyncOperationHandle handle = sceneData.LoadAssetAsync<SceneData>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                StartCoroutine(SceneLoadWithFade((SceneData)sceneData.Asset));
            else
                Debug.LogError($"{sceneData.RuntimeKey}.");
        };
    }
    private IEnumerator SceneLoad(SceneData sceneData) {
        Debug.Log("Loading Scene...");
        _activeScene = sceneData;
        AsyncOperationHandle<SceneInstance> handle = sceneData.SceneReference.LoadSceneAsync();
        sceneData.Operation = handle;
        while (!handle.IsDone) {
            yield return null;
        }
    }

    private IEnumerator SceneLoadWithFade(SceneData sceneData) {
        Debug.Log("Loading Scene...");
        _activeScene = sceneData;
        yield return ScreenFader.Instance.FadeOut();
        AsyncOperationHandle<SceneInstance> handle = sceneData.SceneReference.LoadSceneAsync();
        sceneData.Operation = handle;
        while (!handle.IsDone) {
            yield return null;
        }
        yield return ScreenFader.Instance.FadeIn();
    }
}
