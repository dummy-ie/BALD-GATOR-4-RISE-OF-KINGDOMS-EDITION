using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatView : View
{
    private UIDocument _document;
    private VisualElement _root;
    // private Button _closeButton;
    private Button _attackButton;
    private Button _healButton;
    private Button _endTurnButton;
    public override void Initialize() {
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;
        _attackButton = _root.Q<Button>("AttackButton");
        _healButton = _root.Q<Button>("HealButton");
        _endTurnButton = _root.Q<Button>("EndTurnButton");

        _attackButton.clicked += AttackButtonClicked;
        _healButton.clicked += HealButtonClicked;
        _endTurnButton.clicked += EndTurnButtonClicked;
    }

    private void AttackButtonClicked()
    {
        CombatManager.Instance.AttackSelectedTarget();
    }

    private void HealButtonClicked()
    {
        CombatManager.Instance.HealSelectedTarget();
    }
    
    private void EndTurnButtonClicked()
    {
        CombatManager.Instance.EndCurrentTurn();
    }

    // public void CloseButtonClicked() {
    //     Debug.Log("Clicked");
    //     ViewManager.Instance.HideRecentView();
    //     AudioManager.Instance.Stop();
    // }
}
