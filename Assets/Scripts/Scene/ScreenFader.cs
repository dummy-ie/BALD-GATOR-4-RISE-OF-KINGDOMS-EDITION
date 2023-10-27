using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour {
    public Animator animator;
    private string _sceneName;
    private View _nextView;

    public void FadeAndLoadScene(string sceneName, View nextView = null) {
        _sceneName = sceneName;
        _nextView = nextView;
        Debug.Log(_sceneName);
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        Debug.Log(_sceneName + "Complete");
        if (_nextView != null) {
            ViewManager.Instance.Show(_nextView);
        }
        animator.SetTrigger("FadeIn");
        SceneManager.LoadScene(_sceneName);
    }
}
