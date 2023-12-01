using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class Quest {
    private QuestData _data;
    public QuestData Data { 
        get { return _data; }
    }
    private EQuestState _state;
    public EQuestState State { 
        get { return _state; }
        set { _state = value; }
    }
    private int _currentStepIndex;
    public int CurrentStepIndex { 
        get { return _currentStepIndex; }
    }
    private QuestStepState[] _stepStates;
    public QuestStepState[] StepStates  { 
        get { return _stepStates; }
    }
    public Quest(QuestData questData) {
        this._data = questData;
        this._state = EQuestState.REQUIREMENTS_NOT_MET;
        this._currentStepIndex = 0;
        this._stepStates = new QuestStepState[_data.Steps.Length];
        for (int i = 0; i < _stepStates.Length; i++)
        {
            _stepStates[i] = new QuestStepState();
        }
    }

    public void SetStep(int index) { 
        _currentStepIndex = index;
    }

    public bool CurrentStepExists() { 
        return (_currentStepIndex < _data.Steps.Length);
    }

    public void InstantiateCurrentStep(Transform transform) {
        GameObject stepPrefab = GetCurrentStepPrefab();
        if (stepPrefab != null) {
            Object.Instantiate<GameObject>(stepPrefab, transform);
        }
    }

    public GameObject GetCurrentStepPrefab() {
        GameObject stepPrefab = null;
        if (CurrentStepExists()) {
            stepPrefab = _data.Steps[_currentStepIndex];
        }
        else {
            Debug.LogWarning($"Index Out of Range : {_data.ID}, index = {_currentStepIndex}");
        }
        return stepPrefab;
    }

    public void StoreStepState(QuestStepState stepState, int stepIndex) {
        if (stepIndex < _stepStates.Length)
        {
            _stepStates[stepIndex].State = stepState.State;
        }
        else
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                + "Quest Id = " + Data.ID + ", Step Index = " + stepIndex);
        }
    }
}
