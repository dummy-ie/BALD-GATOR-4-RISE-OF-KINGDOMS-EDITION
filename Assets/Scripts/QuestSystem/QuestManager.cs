using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField]
    private List<string> _labels;
    private AsyncOperationHandle<IList<QuestData>> _handle;
    private int _index = 0;

    private Quest _trackedQuest = null;
    public Quest TrackedQuest {
        get { return _trackedQuest; }
        set { _trackedQuest = value; }
    }

    List<Quest> _currentQuests = new();
    public List<Quest> CurrentQuests {
        get { return _currentQuests; }
    }

    private Dictionary<string, Quest> _questMap = new();
    public UnityAction<string> OnStart;
    public UnityAction<string, int, bool> OnAdvance;
    public UnityAction<string> OnFinish;
    public UnityAction<Quest> OnQuestStateChange;
    public UnityAction<string, int, QuestStepState> OnQuestStepStateChange;

    public Quest GetQuest(string id) {
        Quest quest = _questMap[id];
        if (quest == null)
            Debug.LogError($"ID not found : {id}");
        return quest;
    }
    private void ChangeQuestState(string id, EQuestState state) {
        Quest quest = _questMap[id];
        quest.State = state;
        OnQuestStateChange?.Invoke(quest);
    }
    public void StartQuest(string id) {
        Quest quest = GetQuest(id);
        if (quest == null)
            return;
        if (_trackedQuest == null)
            _trackedQuest = quest;
        if (!_currentQuests.Contains(quest))
            _currentQuests.Add(quest);
        quest.InstantiateCurrentStep(this.transform);
        ChangeQuestState(id, EQuestState.IN_PROGRESS);
        Debug.Log(id + " Started");
    }

    public void AdvanceQuest(string id, int index, bool canFinish = false) {
        Quest quest = _questMap[id];
        if (quest == null)
            return;
        quest.SetStep(index);
        if (quest.CurrentStepExists())
            quest.InstantiateCurrentStep(transform);
        if (canFinish)
            FinishQuest(id);
            //ChangeQuestState(id, EQuestState.CAN_FINISH);
    }

    public void FinishQuest(string id) {
        Debug.Log("Finishing Quest :" + id);
        ChangeQuestState(id, EQuestState.FINISHED);
        _currentQuests.Remove(GetQuest(id));
        if (_trackedQuest == _questMap[id])
            _trackedQuest = null;
    }

    public void FinishCurrentStep(string id, int nextIndex = -1)
    {
        QuestStep[] steps = GetComponentsInChildren<QuestStep>();
        if (nextIndex == -1)
            nextIndex = GetQuest(id).CurrentStepIndex + 1;
        foreach (QuestStep step in steps)
        {
            Debug.Log("Current Step : " + step.StepName + step.gameObject.name);
            if (step.ID == id)
            {
                Debug.Log("Quest Manager : Finishing Step of " + id + ", next Index = " + nextIndex);
                step.FinishStep(nextIndex);
            }
        }

    }

    private bool CheckRequirementsMet(Quest quest) {
        bool meetsRequirements = true;
        foreach (QuestData prerequisiteQuest in quest.Data.Prerequisites) {
                // Debug.Log("yea");
            if (GetQuest(prerequisiteQuest.ID).State != EQuestState.FINISHED) {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }
    private void StepStateChange(string id, int stepIndex, QuestStepState questStepState) {
        Quest quest = GetQuest(id);
        //quest.StoreStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.State);
    }

    private void OnLoadQuests(QuestData asset) {
        Debug.Log("Loaded Quest Data Asset : " + asset.ID);
        _questMap.Add(asset.ID, new Quest(asset));
        _index++;
        
        
    }

    private void OnComplete(AsyncOperationHandle<IList<QuestData>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Completed");
            /*Debug.Log(GetQuest("TestQuest").Data.DisplayName);
            Debug.Log(GetQuest("TestQuest").State);
            Debug.Log(GetQuest("TestQuest").CurrentStepIndex);
            Debug.Log(GetQuest("TestQuest").CurrentStepExists());
            _trackedQuest = GetQuest("TestQuest");*/
            StartQuest("MainQuest1");
            //FinishCurrentStep("MainQuest1");
            //FinishCurrentStep("MainQuest1");
            //FinishCurrentStep("MainQuest1");
        }
        else
            Debug.LogError($"Quest Data not loaded.");
    }
    protected override void OnAwake() {
        _questMap = new Dictionary<string, Quest>();
    }

    private void Start()
    {
        _handle = Addressables.LoadAssetsAsync<QuestData>(_labels, OnLoadQuests, Addressables.MergeMode.Union, false);
        _handle.Completed += OnComplete;
        
    }

    private void OnEnable() {
        OnStart += StartQuest;
        OnAdvance += AdvanceQuest;
        OnFinish += FinishQuest;

        OnQuestStepStateChange += StepStateChange;
    }

    private void OnDisable()
    {
        OnStart -= StartQuest;
        OnAdvance -= AdvanceQuest;
        OnFinish -= FinishQuest;

        OnQuestStepStateChange -= StepStateChange;
    }

    private void Update() {
        foreach (Quest quest in _questMap.Values)
        {
            if (quest.State == EQuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest)) {
                ChangeQuestState(quest.Data.ID, EQuestState.CAN_START);
            }
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(this._handle);
    }
}
