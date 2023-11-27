using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, Quest> _questMap;
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
            ChangeQuestState(id, EQuestState.CAN_FINISH);
    }

    public void FinishQuest(string id) {
        ChangeQuestState(id, EQuestState.FINISHED);
    }
    private bool CheckRequirementsMet(Quest quest) {
        bool meetsRequirements = true;
        foreach (QuestData prerequisiteQuest in quest.Data.Prerequisites) {
                Debug.Log("yea");
            if (GetQuest(prerequisiteQuest.ID).State != EQuestState.FINISHED) {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }
    private void StepStateChange(string id, int stepIndex, QuestStepState questStepState) {
        Quest quest = GetQuest(id);
        quest.StoreStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.State);
    }

    protected override void OnAwake() {
        _questMap = new Dictionary<string, Quest>();
        QuestData[] quests = Resources.LoadAll<QuestData>("Quests");
        foreach (QuestData questData in quests) {
            _questMap.Add(questData.ID, new Quest(questData));
        }
        Quest quest = GetQuest("TestQuest");
        Debug.Log(quest.Data.DisplayName);
        Debug.Log(quest.State);
        Debug.Log(quest.CurrentStepIndex);
        Debug.Log(quest.CurrentStepExists());
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
}
