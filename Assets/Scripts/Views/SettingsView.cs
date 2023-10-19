using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsView : View
{
    private Button _backButton;
    public override void Initialize() {
        this._root = this._document.rootVisualElement;
        this._backButton = (Button)this._root.Q("BackButton");
        this._backButton.clicked += BackButtonClicked;
    }

    public void BackButtonClicked() {
        Debug.Log("Clicked");
        ViewManager.Instance.Show<MainMenuView>();
    }
}
