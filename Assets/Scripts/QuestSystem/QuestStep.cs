using UnityEngine;

public class QuestStep : MonoBehaviour
{
    private bool _finished = false;
    private string _id;
    public string ID
    {
        get { return _id; }
    }
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
        _finished = false;
        Debug.Log($"Initializing Step {stepIndex} of {id}");
        if (stepState != null && stepState != "")
        {
            //SetStepState(stepState);
        }
    }
    public void FinishStep(int nextStepIndex) {
        Debug.Log(_stepName + " : " + _finished);
        if (!_finished) {
            _finished = true;
            Debug.Log("Finishing Step : " +  _stepName);
            QuestManager.Instance.AdvanceQuest(_id, nextStepIndex, _canFinish);
            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState) {
        QuestManager.Instance.OnQuestStepStateChange(_id, _stepIndex, new QuestStepState(newState));
    }

    //protected abstract void SetStepState(string state = "");
}
