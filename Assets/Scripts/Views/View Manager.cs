using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : Singleton<ViewManager> {
    //[SerializeField]
    //private View _startingView;

    [SerializeField]
    private View[] _views;

    [SerializeField]
    public Stack<View> _currentViews = new();

    public T GetView<T>() where T : View { 
        for (int i = 0; i < this._views.Length; i++) { 
            if (this._views[i] is T view) {
                return view;
            }
        }
        return default;
    }
    
    public void Show<T>() where T : View {
        for (int i = 0; i < this._views.Length; i++) {
            if (this._views[i] is T view) {
                //if (this._currentViews.Count != 0) {
                //    this._currentViews.Peek();//.Hide();
                //    this._currentViews.Pop();
                //}
                view.Show();
                this._currentViews.Push(view);
            }
        }
    }

    public void Show(View view) {
        //if (this._currentViews.Count != 0) {
        //    this._currentViews.Peek();//.Hide();
        //    this._currentViews.Pop().Hide();
        //}
        view.Show();
        this._currentViews.Push(view);
    }
    
    public void PopUp<T>() where T : View {
        for (int i = 0; i < this._views.Length; i++) {
            if (this._views[i] is T view) {
                view.SortingOrder = _currentViews.Peek().SortingOrder + 1;
                view.Show();
                this._currentViews.Push(view);
            }
        }
    }

    public void PopUp(View view) {
        view.SortingOrder = _currentViews.Peek().SortingOrder + 1;
        view.Show();
        this._currentViews.Push(view);
    }

    public void HideRecentView() {
        this._currentViews.Pop().Hide();
    }
    public void InitializeViews()
    {
        _views = FindObjectsOfType<View>(true);
        for (int i = 0; i < this._views.Length; i++)
        {
            _views[i].Initialize();
            _views[i].Hide();
            if (_views[i].OnStart)
            {
                _views[i].Show();
            }
        }
    }

    protected override void OnAwake() {
        InitializeViews();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        InitializeViews();
    }
    private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable() {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}

