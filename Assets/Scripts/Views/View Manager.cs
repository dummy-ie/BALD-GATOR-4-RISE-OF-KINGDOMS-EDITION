using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewManager : MonoBehaviour {
    public static ViewManager Instance;

    [SerializeField]
    private ScreenFader _screenFader;

    public ScreenFader ScreenFader {
        get { return _screenFader; }
    }

    [SerializeField]
    private View _startingView;
    [SerializeField]
    private View _loadingScreen;

    [SerializeField]
    private View[] _views;

    private View _currentView;
    private List<View> _currentPopUps;

    public T GetView<T>() where T : View { 
        for (int i = 0; i < this._views.Length; i++) { 
            if (this._views[i] is T view) {
                Debug.Log("Returning View.");
                return view;
            }
        }
        Debug.Log("Returning Default");
        return default;
    }
    
    public void Show<T>() where T : View {
        for (int i = 0; i < this._views.Length; i++) {
            if (this._views[i] is T view) {
                Debug.Log("Showing View.");
                this._currentView.Hide();
                foreach (View popUp in this._currentPopUps) { 
                    popUp.Hide();
                }
                this._currentPopUps.Clear();
                this.Show(this._views[i]);
            }
        }
    }

    public void Show(View view) {
        if (this._currentView != null) {
            this._currentView.Hide();
        }
        //foreach (View popUp in this._currentPopUps) { 
        //    popUp.Hide();
        //}
        //this._currentPopUps.Clear();
        view.Show();
        this._currentView = view;
    }

    public void PopUp<T>() where T : View {
        for (int i = 0; i < this._views.Length; i++) {
            if (this._views[i] is T view) {
                Debug.Log("Popping Up View.");
                this._views[i].Show();
                this._currentPopUps.Add(view);
            }
        }
    }

    public void PopUp(View view) {
        view.Show();
        this._currentPopUps.Add(view);
    }

    public void HidePopUp(View view) {
        view.Hide();
        this._currentPopUps.Remove(view);
    }

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        for (int i = 0; i < this._views.Length;i++) {
            _views[i].Initialize();
            _views[i].Hide();
        }
        if (this._startingView != null) {
            Show(this._startingView);
        }
    }
}
