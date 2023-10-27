using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaLoader : MonoBehaviour
{
    // [SerializeField] private Scene _sceneToLoad;
    [SerializeField] private String _sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // SceneManager.LoadSceneAsync(_sceneToLoad.buildIndex, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
        }
    }
}
