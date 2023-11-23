using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuView : View {
    private UIDocument _document;
    private VisualElement _root;
    private Button _playButton;
    private Button _settingsButton;

    public override void Initialize() {
        this._document = GetComponent<UIDocument>();
        this._root = _document.rootVisualElement;
        this._playButton = this._root.Q<Button>("PlayButton");
        this._settingsButton = this._root.Q<Button>("SettingsButton");
        this._playButton.clicked += PlayButtonClicked;
        this._settingsButton.clicked += SettingsButtonClicked;
        AudioManager.Instance.Play(1, true);
    }

    public override void SetSortingOrder(int value) {
        base.SetSortingOrder(value);
        _document.sortingOrder = _sortingOrder;
    }

    public void PlayButtonClicked() {
        Debug.Log("Clicked Play Button");
        AudioManager.Instance.Play("Majestic_Sound");
        SceneLoader.Instance.LoadScene("Port", ViewManager.Instance.GetView<GameView>());
        ViewManager.Instance.Show(ViewManager.Instance.GetComponentInChildren<GameView>());
    }

    public void SettingsButtonClicked() {
        Debug.Log("Clicked Settings Button");
        ViewManager.Instance.PopUp<SettingsView>();
    }

    private void OnDisable() {
        AudioManager.Instance.Stop();
    }
}