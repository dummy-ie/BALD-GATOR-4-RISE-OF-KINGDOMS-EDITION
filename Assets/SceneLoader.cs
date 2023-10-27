using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    AsyncOperation _loadOperation;
    string _nextScene;
    /*private IEnumerator SceneLoading() { 
        _loadOperation = SceneManager.LoadSceneAsync()
    }*/

    void Start() {
        //StartCoroutine(SceneLoading());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
