using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private SceneConnection _sceneConnection;

    [SerializeField]
    private string _targetSceneName;

    [SerializeField]
    private Transform _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (_sceneConnection == SceneConnection.ActiveConnection)
        {
            FindObjectOfType<PlayerController>().transform.position = _spawnPoint.position; // maybe store a player reference in the scriptableObject?
        }

        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneConnection.ActiveConnection = _sceneConnection;
            //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
            //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
            SceneLoader.Instance.LoadScene(_targetSceneName, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneConnection.ActiveConnection = _sceneConnection;
            //StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(_targetSceneName));
            //SceneLoader.Instance.LoadSceneWithoutFade(_targetSceneName);
            SceneLoader.Instance.LoadScene(_targetSceneName, true);
        }
    }
}
