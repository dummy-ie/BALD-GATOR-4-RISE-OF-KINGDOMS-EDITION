using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProperty {
    [SerializeField]
    private EQuestState _state;
    public EQuestState State {
        get { return _state; }
        set { _state = value; }
    }
    [SerializeField]
    private int _stepIndex;
    public int StepIndex {
        get { return _stepIndex; }
        set { _stepIndex = value; }
    }
    //public QuestStepState[] questStepStates;

    /*public QuestProperty(EQuestState state, int stepIndex/*, QuestStepState[] questStepStates*//*)
    {
        _state = state;
        _stepIndex = stepIndex;
        //this.questStepStates = questStepStates;
    }*/
}