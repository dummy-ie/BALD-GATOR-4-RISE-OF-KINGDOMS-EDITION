using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestView : View {
    List<Quest> _currentQuests;
    UIDocument _document;
    VisualElement _root;
    Label _questName;
    Label _stepName;
    Label _stepDescription;

    private void StartQuest(string id) {
        Quest quest = QuestManager.Instance.GetQuest(id);
        _currentQuests.Add(quest);
        _questName.text = quest.Data.DisplayName;
        _stepName.text = quest.Data.Steps[quest.CurrentStepIndex].GetComponent<QuestStep>().StepName;
        _stepDescription.text = quest.Data.Steps[quest.CurrentStepIndex].GetComponent<QuestStep>().StepDescription;
        //_questDescription.text = quest.Data.Description;
    }
    private void FinishQuest(string id) {
        Quest quest = QuestManager.Instance.GetQuest(id);
        _currentQuests.Remove(quest);
    }
    public override void Initialize()
    {
        _currentQuests = new List<Quest>();
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        _questName = _root.Q<Label>("QuestName");
        _stepName = _root.Q<Label>("StepName");
        _stepDescription = _root.Q<Label>("StepDescription");
    }
    private void OnEnable()
    {
        base.OnEnable();
        QuestManager.Instance.OnStart += StartQuest;
        QuestManager.Instance.OnFinish += FinishQuest;
    }

    private void OnDisable()
    {
        QuestManager.Instance.OnStart -= StartQuest;
        QuestManager.Instance.OnFinish -= FinishQuest;
    }
}
