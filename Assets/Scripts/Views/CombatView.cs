using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatView : View
{
    private UIDocument _document;
    private VisualElement _root;

    private Button _attackButton;
    private Button _healButton;
    private Button _endTurnButton;

    private VisualElement _selectedTarget;
    private ProgressBar _targetHealthBar;
    private Label _targetName;

    private VisualElement _hotbar;
    private ProgressBar _currentTurnHealthBar;
    private Label _combatantName;

    private const string _animationClass = "animation-hide";

    public override void Initialize()
    {
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;

        // turn buttons
        _attackButton = _root.Q<Button>("AttackButton");
        _healButton = _root.Q<Button>("HealButton");
        _endTurnButton = _root.Q<Button>("EndTurnButton");

        _attackButton.clicked += AttackButtonClicked;
        _healButton.clicked += HealButtonClicked;
        _endTurnButton.clicked += EndTurnButtonClicked;

        // target bar at the top
        _selectedTarget = _root.Q<VisualElement>("SelectedTarget");
        _targetHealthBar = _root.Q<ProgressBar>("TargetHealthBar");
        _targetName = _root.Q<Label>("TargetName");

        // hotbar at the bottom
        _hotbar = _root.Q<VisualElement>("Hotbar");
        _currentTurnHealthBar = _root.Q<ProgressBar>("CurrentTurnHealthBar");
        _combatantName = _root.Q<Label>("CombatantName");
    }

    public void SetTargetData(Combatant combatant = null)
    {
        // If you pass null, just hide the target indicator.
        if (combatant == null)
        {
            // _selectedTarget.style.display = DisplayStyle.None;
            _selectedTarget.AddToClassList(_animationClass);
            return;
        }

        // _selectedTarget.style.display = DisplayStyle.Flex;
        _selectedTarget.RemoveFromClassList(_animationClass);
        // _targetHealthBar.value = ClassData.GetHealthPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth);
        StartCoroutine(InterpolateProgressBar(_targetHealthBar, ClassData.GetHealthPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth), 0.5f));
        _targetName.text = combatant.name;

        if (combatant.Data.Affiliation == Entity.AffiliationState.Ally)
            _targetName.style.color = Color.blue;
        else if (combatant.Data.Affiliation == Entity.AffiliationState.Enemy)
            _targetName.style.color = Color.red;
    }

    public void ToggleHotbar()
    {
        _hotbar.ToggleInClassList(_animationClass);
    }

    public void SetCurrentTurnData(Combatant combatant)
    {
        if (combatant.Data.Affiliation == Entity.AffiliationState.Ally)
            _combatantName.style.color = Color.blue;
        else if (combatant.Data.Affiliation == Entity.AffiliationState.Enemy)
            _combatantName.style.color = Color.red;

        _currentTurnHealthBar.title = combatant.Data.Health + " / " + combatant.Data.Class.MaxHealth;
        // _currentTurnHealthBar.value = ClassData.GetHealthPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth);
        StartCoroutine(InterpolateProgressBar(_currentTurnHealthBar, ClassData.GetHealthPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth), 0.5f));
        _combatantName.text = combatant.name;

        _attackButton.style.display = DisplayStyle.Flex;
        _healButton.style.display = DisplayStyle.Flex;

        if (combatant.Data.Class.Attack == null)
            _attackButton.style.display = DisplayStyle.None;

        if (combatant.Data.Class.Heal == null)
            _healButton.style.display = DisplayStyle.None;
    }

    private IEnumerator InterpolateProgressBar(ProgressBar bar, int newValue, float duration)
    {
        float time = 0;
        int oldValue = (int)bar.value;
        while (time <= duration)
        {
            time += Time.deltaTime;
            bar.value = Mathf.Lerp(oldValue, newValue, time / duration);
            yield return null;
        }
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
}
