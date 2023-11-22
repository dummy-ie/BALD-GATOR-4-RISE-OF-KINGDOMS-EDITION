using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : Singleton<ScreenFader> {
    private Animator _animator;
    public Animator Animator { get { return _animator; } }
    [SerializeField]
    private AnimationClip[] _clips;
    public AnimationClip[] Clips { get {  return _clips; } }

    public WaitForSeconds FadeOut() {
        _animator.SetTrigger("FadeOut");
        return new WaitForSeconds(_clips[0].length);
    }

    public WaitForSeconds FadeIn() {
        _animator.SetTrigger("FadeIn");
        return new WaitForSeconds(_clips[1].length);
    }

    /*public void OnFadeComplete() {
        _fading = false;

    }*/

    protected override void OnAwake() {
        this._animator = GetComponent<Animator>();
    }
}
