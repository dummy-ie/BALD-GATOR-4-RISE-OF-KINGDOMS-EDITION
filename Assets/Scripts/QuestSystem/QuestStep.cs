using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool _finished = false;
    private string _id;
    private int _stepIndex;

    [SerializeField]
    private string _stepName;
    public string StepName {
        get { return _stepName; }
    }
    [SerializeField]
    private string _stepDescription;
    public string StepDescription {
        get { return _stepDescription; }
    }
    [SerializeField]
    private bool _canFinish = false;
    public void InitializeQuestStep(string id, int stepIndex, string stepState) {
        _id = id;
        _stepIndex = stepIndex;
        if (stepState != null && stepState != "")
        {
            SetStepState(stepState);
        }
    }
    public void FinishStep(int nextStepIndex) { 
        if (!_finished) {
            _finished = true;
            QuestManager.Instance.AdvanceQuest(_id, nextStepIndex, _canFinish);
            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState) {
        QuestManager.Instance.OnQuestStepStateChange(_id, _stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetStepState(string state = "");
}
