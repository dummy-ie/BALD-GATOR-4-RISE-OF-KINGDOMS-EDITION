using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuView : View {
    private Button _playButton;
    private Button _settingsButton;

    public override void Initialize() {
        this._root = this._document.rootVisualElement;
        this._playButton = this._root.Q<Button>("PlayButton");
        this._settingsButton = this._root.Q<Button>("SettingsButton");
        this._playButton.clicked += PlayButtonClicked;
        this._settingsButton.clicked += SettingsButtonClicked;
        AudioManager.Instance.Play(1, true);
    }

    public void PlayButtonClicked() {
        Debug.Log("Clicked Play Button");
        AudioManager.Instance.Play("Majestic_Sound");
        ViewManager.Instance.ScreenFader.FadeAndLoadScene("Port", ViewManager.Instance.GetView<GameView>());
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