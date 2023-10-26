using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    /*[Header("Fader")]
    [SerializeField]
    private UnityEngine.UI.Image _fadeOutImage;
    [SerializeField]
    private float _fadeSpeed = 0.8f;

    [Header("Extras")]
    [SerializeField]
    private string nextSceneName;
    IEnumerator Fade()
    {
        float alpha = 1.0f;
        float fadeEnd = 0.0f;
        while (alpha >= fadeEnd)
        {
            Debug.Log(alpha + " Fading");
            SetColorImage(ref alpha);
            yield return null;
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        yield return Fade();
        Debug.Log("Load Scene");
        SceneManager.LoadScene(nextSceneName);
    }
    private void SetColorImage(ref float alpha)
    {
        _fadeOutImage.color = new Color(_fadeOutImage.color.r, _fadeOutImage.color.g, _fadeOutImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / _fadeSpeed) * -1;
    }

    void OnEnable() { 
        StartCoroutine(Fade());
    }*/

    //public static SceneFader Instance;

    public Animator animator;
    private string _sceneName;
    private View _nextView;

    public void Fade(string sceneName, View nextView = null) {
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
        SceneManager.LoadScene(_sceneName);
    }

    /*void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }*/
    void Start() {
        //animator = this.GetComponentInChildren<Animator>();
    }
}
