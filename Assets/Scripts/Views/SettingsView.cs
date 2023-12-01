using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsView : View
{
    private UIDocument _document;
    private VisualElement _root;
    private Button _closeButton;
    public override void Initialize() {
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;
        this._closeButton = this._root.Q<Button>("CloseButton");
        this._closeButton.clicked += CloseButtonClicked;
        //Debug.Log("Initialized Settings View");
    }

    public void CloseButtonClicked() {
        Debug.Log("Clicked");
        ViewManager.Instance.HideRecentView();
        //AudioManager.Instance.Stop();
    }
}
