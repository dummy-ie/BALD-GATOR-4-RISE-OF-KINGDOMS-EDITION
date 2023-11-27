using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {
    private QuestData _info;
    public QuestData Info { 
        get { return _info; }
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
    //private QuestStep[] _steps;
    public Quest(QuestData questData) {
        this._info = questData;
        this._state = EQuestState.REQUIREMENTS_NOT_MET;
        this._currentStepIndex = 0;
        //this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        //for (int i = 0; i < questStepStates.Length; i++)
        {
            //QuestStep[i] = new QuestStepState();
        }
    }

    public void SetStep(int index) { 
        _currentStepIndex = index;
    }

    public bool CurrentStepExists() { 
        return (_currentStepIndex < _info.Steps.Length);
    }

    public void InstantiateCurrentStep(Transform transform) {
        GameObject stepPrefab = GetCurrentStepPrefab();
        if (stepPrefab != null) {
            Object.Instantiate<GameObject>(stepPrefab, transform);
        }
    }

    private GameObject GetCurrentStepPrefab() {
        GameObject stepPrefab = null;
        if (CurrentStepExists()) {
            stepPrefab = _info.Steps[_currentStepIndex];
        }
        else {
            Debug.LogWarning($"Index Out of Range : {_info.ID}, index = {_currentStepIndex}");
        }
        return stepPrefab;
    }
}
