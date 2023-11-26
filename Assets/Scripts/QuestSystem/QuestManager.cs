using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, Quest> _questMap;
    public UnityAction<string> OnStart;
    public UnityAction<string> OnAdvance;
    public UnityAction<string> OnFinish;

    public void StartQuest(string id) { 
        OnStart?.Invoke(id);
    }

    public void AdvanceQuest(string id) {
        OnAdvance?.Invoke(id);
    }

    public void FinishQuest(string id) {
        OnFinish?.Invoke(id);
    }

    private void Awake() {
        QuestData[] quests = Resources.LoadAll<QuestData>("Quests");
        foreach (QuestData questData in quests) {
            if (_questMap.ContainsKey(questData.ID)) {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questData.ID);
            }
            Debug.Log(questData.ID);
            _questMap.Add(questData.ID, new Quest(questData));
        }
    }
}
