using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    AssetReference _nextSceneReference;

    private IEnumerator Start()
    {
        AsyncOperationHandle<IList<GameObject>> singletons =
                Addressables.LoadAssetsAsync<GameObject>("Singleton", singletons => Instantiate(singletons));

        yield return singletons;
        SceneLoader.Instance.LoadSceneWithoutFade(_nextSceneReference);
    }
}
