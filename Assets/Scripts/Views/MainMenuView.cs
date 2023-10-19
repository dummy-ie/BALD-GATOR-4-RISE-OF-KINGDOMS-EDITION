using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuView : View {

    private Button _playButton;

    public override void Initialize() {
        this._root = this._document.rootVisualElement;
        this._playButton = (Button)this._root.Q("PlayButton");
        if (this._playButton == null)
            Debug.Log("null");
        Debug.Log("Initialized Main Menu");
        this._playButton.clicked += PlayButtonClicked;
    }

    public void PlayButtonClicked() {
        Debug.Log("Clicked Play Button");
        this.Hide();
        ViewManager.Instance.GetView<SettingsView>().Show();
    }
}
