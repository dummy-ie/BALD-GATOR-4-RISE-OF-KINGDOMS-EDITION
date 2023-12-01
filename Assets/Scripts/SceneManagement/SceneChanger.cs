using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private SceneConnection _sceneConnection;
    [SerializeField]
    private AssetReference _targetSceneReference;
    //[SerializeField]
    private string _targetSceneName;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private EBGMIndex _nextSceneBGM;
    [SerializeField]
    private ESFXIndex _playSFX;

    //private SceneConnection _sceneConnection;

    // Start is called before the first frame update
    void Start()
    {
        if (_sceneConnection == SceneConnection.ActiveConnection)
        {
            FindObjectOfType<PlayerController>().transform.position = _spawnPoint.position; // maybe store a player reference in the scriptableObject?
        }

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        /*AsyncOperationHandle handle = _sceneConnectionReference.LoadAssetAsync<SceneConnection>();
        handle.Completed += (AsyncOperationHandle handle) => {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                _sceneConnection = (SceneConnection)_sceneConnectionReference.Asset;
            else
                Debug.LogError($"{_sceneConnectionReference.RuntimeKey}.");
        };*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneConnection.ActiveConnection = _sceneConnection;
            //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
            //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
            //SceneLoader.Instance.LoadScene(_targetSceneName, true);
            //AudioManager.Instance.PlayBGM(_nextSceneBGM, 0);
            SceneLoader.Instance.LoadSceneWithFade(_targetSceneReference);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Switching Scene to : {_targetSceneName}");
            SceneConnection.ActiveConnection = _sceneConnection;
            //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
            //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
            //SceneLoader.Instance.LoadScene(_targetSceneName, true);
            AudioManager.Instance.PlayBGM(_nextSceneBGM, 0);
            AudioManager.Instance.PlaySFX(_playSFX);
            SceneLoader.Instance.LoadSceneWithFade(_targetSceneReference);
            PartyManager.Instance.SpawnPartyMembers(_spawnPoint);
        }
    }
}
