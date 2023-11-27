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

    public Quest GetQuest(string id) {
        Quest quest = _questMap[id];
        if (quest == null)
            Debug.LogError($"ID not found : {id}");
        return quest;
    }
    private void ChangeQuestState(string id, EQuestState state) {
        Quest quest = _questMap[id];
        quest.State = state;
        EventsManager.Instance.QuestEvents.OnQuestStateChange?.Invoke(quest);
    }
    public void StartQuest(string id) {
        Quest quest = GetQuest(id);
        if (quest == null)
            return;
        quest.InstantiateCurrentStep(this.transform);
        quest.State = EQuestState.IN_PROGRESS;
        Debug.Log(quest.Info.ID + " Started");
    }

    public void AdvanceQuest(string id, int index, bool canFinish) {
        Quest quest = _questMap[id];
        if (quest == null)
            return;
        quest.SetStep(index);
        if (quest.CurrentStepExists())
            quest.InstantiateCurrentStep(transform);
        if (canFinish)
            quest.State = EQuestState.CAN_FINISH;
    }

    public void FinishQuest(string id) {
        _questMap[id].State = EQuestState.FINISHED;
    }
    private bool CheckRequirementsMet(Quest quest) {
        bool meetsRequirements = true;
        foreach (QuestData prerequisiteQuest in quest.Info.Prerequisites) {
                Debug.Log("yea");
            if (GetQuest(prerequisiteQuest.ID).State != EQuestState.FINISHED) {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    protected override void OnAwake() {
        _questMap = new Dictionary<string, Quest>();
        QuestData[] quests = Resources.LoadAll<QuestData>("Quests");
        foreach (QuestData questData in quests) {
            _questMap.Add(questData.ID, new Quest(questData));
        }
        Quest quest = GetQuest("TestQuest");
        Debug.Log(quest.Info.DisplayName);
        Debug.Log(quest.Info.Description);
        Debug.Log(quest.State);
        Debug.Log(quest.CurrentStepIndex);
        Debug.Log(quest.CurrentStepExists());
    }

    private void OnEnable() {
        EventsManager.Instance.QuestEvents.OnStart += StartQuest;
        EventsManager.Instance.QuestEvents.OnAdvance += AdvanceQuest;
        EventsManager.Instance.QuestEvents.OnFinish += FinishQuest;
        //Debug.Log(OnStart);

        //GameEventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }

    private void OnDisable()
    {
        EventsManager.Instance.QuestEvents.OnStart -= StartQuest;
        EventsManager.Instance.QuestEvents.OnAdvance -= AdvanceQuest;
        EventsManager.Instance.QuestEvents.OnFinish -= FinishQuest;
    }

    private void Update() {
        foreach (Quest quest in _questMap.Values)
        {
            if (quest.State == EQuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest)) {
                ChangeQuestState(quest.Info.ID, EQuestState.CAN_START);
            }
        }
    }
}
