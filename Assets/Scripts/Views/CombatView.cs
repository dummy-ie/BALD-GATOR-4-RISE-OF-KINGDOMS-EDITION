using System;
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
    private ProgressBar _currentTurnMovementBar;
    private Label _combatantName;
    private List<Button> _actionPoints;

    private Button _switchCameraButton;



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
        _currentTurnMovementBar = _root.Q<ProgressBar>("CurrentTurnMovementBar");
        _combatantName = _root.Q<Label>("CombatantName");
        _actionPoints = _root.Query<Button>("ActionPoint").ToList();

        // switch cam button
        _switchCameraButton = _root.Q<Button>("SwitchCameraButton");
        _switchCameraButton.clicked += SwitchCameraButtonClicked;

        Hide();
    }

    public void SetAttackHitPercentage(int dieFaces = 0, int modifier = 0, int difficultyClass = 0)
    {
        float percentage = InternalDice.Instance.GetPercentageOfRoll(dieFaces, modifier, difficultyClass);

        if (percentage < 0)
        {
            _attackButton.text = string.Empty;
            return;
        }
        // else if (percentage == 0)
        // {
        //     _attackButton.text = "0%";
        //     return;
        // }

        _attackButton.text = Math.Floor(percentage).ToString() + "%";
    }

    public void SetTargetData(Combatant combatant = null)
    {
        if (_selectedTarget == null)
            return;

        // If you pass null, just hide the target indicator.
        if (combatant != null)
        {
        }
        else
        {
            // _selectedTarget.style.display = DisplayStyle.None;
            _selectedTarget.AddToClassList(_animationClass);
            return;
        }

        // _selectedTarget.style.display = DisplayStyle.Flex;
        _selectedTarget.RemoveFromClassList(_animationClass);
        // _targetHealthBar.value = ClassData.GetHealthPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth);
        StartCoroutine(InterpolateProgressBar(_targetHealthBar, ClassData.GetPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth), 0.5f));
        _targetName.text = combatant.name;

        if (combatant.Data.Affiliation == Entity.AffiliationState.Ally)
            _targetName.style.color = Color.blue;
        else if (combatant.Data.Affiliation == Entity.AffiliationState.Enemy)
            _targetName.style.color = Color.red;
    }

    public void ToggleHotbar()
    {
        _hotbar.ToggleInClassList(_animationClass);
        _switchCameraButton.ToggleInClassList(_animationClass);
    }

    public void SetCurrentTurnData(Combatant combatant)
    {
        if (combatant.Data.Affiliation == Entity.AffiliationState.Ally)
            _combatantName.style.color = Color.blue;
        else if (combatant.Data.Affiliation == Entity.AffiliationState.Enemy)
            _combatantName.style.color = Color.red;

        _currentTurnHealthBar.title = combatant.Data.Health + " / " + combatant.Data.Class.MaxHealth;
        // _currentTurnHealthBar.value = ClassData.GetHealthPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth);
        StartCoroutine(InterpolateProgressBar(_currentTurnHealthBar, ClassData.GetPercentage(combatant.Data.Health, combatant.Data.Class.MaxHealth), 1f));
        SetCurrentTurnMovementBar(combatant);
        SetCurrentTurnActionPoints(combatant);

        _combatantName.text = combatant.name;

        _attackButton.style.display = DisplayStyle.Flex;
        _healButton.style.display = DisplayStyle.Flex;
        _endTurnButton.style.display = DisplayStyle.Flex;

        if (combatant.Data.Class.Attack == null)
            _attackButton.style.display = DisplayStyle.None;

        if (combatant.Data.Class.Heal == null)
            _healButton.style.display = DisplayStyle.None;

        if (combatant.Data.Affiliation == Entity.AffiliationState.Enemy)
        {
            _attackButton.style.display = DisplayStyle.None;
            _healButton.style.display = DisplayStyle.None;
            _endTurnButton.style.display = DisplayStyle.None;
        }
    }

    public void SetCurrentTurnMovementBar(Combatant combatant)
    {
        StartCoroutine(InterpolateProgressBar(_currentTurnMovementBar, ClassData.GetPercentage(combatant.Data.MovementRemaining, combatant.Data.Class.MovementSpeed), 0.5f));
    }

    public void SetCurrentTurnActionPoints(Combatant combatant)
    {
        foreach (Button point in _actionPoints)
        {
            point.style.display = DisplayStyle.None;
            point.RemoveFromClassList("enabled");
        }

        for (int i = 0; i < combatant.Data.Class.MaxActions; i++)
        {
            if (i >= _actionPoints.Count)
            {
                Debug.LogWarning("Entity " + combatant.name + " has too many maximum action points to display.");
                Debug.LogWarning("Action points list count: " + _actionPoints.Count);
                Debug.LogWarning("Max action points: " + combatant.Data.Class.MaxActions);
                break;
            }

            _actionPoints[i].style.display = DisplayStyle.Flex;
        }

        for (int i = 0; i < combatant.Data.ActionsLeft; i++)
        {
            if (i >= _actionPoints.Count)
            {
                Debug.LogWarning("Entity " + combatant.name + " has too many remaining action points to display.");
                Debug.LogWarning("Actions left: " + combatant.Data.ActionsLeft);
                break;
            }

            _actionPoints[i].AddToClassList("enabled");
        }
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

    private IEnumerator InterpolateProgressBar(ProgressBar bar, float newValue, float duration)
    {
        float time = 0;
        float oldValue = bar.value;
        while (time <= duration)
        {
            time += Time.deltaTime;
            bar.value = Mathf.Lerp(oldValue, newValue, time / duration);
            yield return null;
        }
    }

    private void SwitchCameraButtonClicked()
    {
        CombatManager.Instance.SwitchCameraToOverhead();
    }

    private void AttackButtonClicked()
    {
        // Debug.Log("attack");
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
