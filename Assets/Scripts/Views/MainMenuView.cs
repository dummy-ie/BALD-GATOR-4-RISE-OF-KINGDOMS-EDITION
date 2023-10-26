using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuView : View {
    private Button _playButton;

    public override void Initialize() {
        this._root = this._document.rootVisualElement;
        this._playButton = (Button)this._root.Q("PlayButton");
        //Debug.Log("Initialized Main Menu");
        this._playButton.clicked += PlayButtonClicked;
    }

    public void PlayButtonClicked() {
        Debug.Log("Clicked Play Button");
        //ViewManager.Instance.Show<SettingsView>();
        this.Hide();
        AudioManager.Instance.Play("Majestic_Sound");
        SceneManager.LoadScene("SampleScene");
    }
}