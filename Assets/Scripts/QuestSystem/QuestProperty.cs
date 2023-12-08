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
    [SerializeField]
    private QuestStepState[] _stepStates;
    public QuestStepState[] StepStates {
        get { return _stepStates; }
        set { _stepStates = value; }
    }

    public QuestProperty(EQuestState state, int stepIndex, QuestStepState[] stepStates)
    {
        _state = state;
        _stepIndex = stepIndex;
        _stepStates = stepStates;
    }
}
