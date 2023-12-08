using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class QuestView : View {
    
    UIDocument _document;
    VisualElement _root;
    ScrollView _questList;
    Button _closeButton;
    Label _stepName;
    Label _description;

    private void StartQuest(string id) {
        Quest quest = QuestManager.Instance.GetQuest(id);
        if (QuestManager.Instance.CurrentQuests.Count == 0)
            QuestManager.Instance.TrackedQuest = quest;
        UpdateQuestList();
    }
    private void FinishQuest(string id) {
        Quest quest = QuestManager.Instance.GetQuest(id);
        UpdateQuestList();
    }
    public override void Initialize()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        _questList = _root.Q<ScrollView>("QuestList");
        _stepName = _root.Q<Label>("StepName");
        _description = _root.Q<Label>("Description");
        _closeButton = _root.Q<Button>("CloseButton");
        _closeButton.clicked += () =>
        {
            ViewManager.Instance.GetView<GameView>().Show();
            Camera.main.gameObject.GetComponent<PostProcessVolume>().enabled = false;
            BaldGatorManager.Instance.ResumeGame();
            Hide();
        };
    }

    void UpdateQuestList()
    {
        foreach (Quest quest in QuestManager.Instance.CurrentQuests)
        {
            Debug.Log(quest.Data.ID);
            if (QuestManager.Instance.TrackedQuest != null)
            {
                _stepName.text = QuestManager.Instance.TrackedQuest.GetCurrentStepPrefab().GetComponent<QuestStep>().StepName;
               _description.text = QuestManager.Instance.TrackedQuest.GetCurrentStepPrefab().GetComponent<QuestStep>().StepDescription;
            }
            Button newButton = new Button();
            newButton.text = quest.Data.DisplayName;
            newButton.AddToClassList("quest-button");
            newButton.clicked += () =>
            {
                _stepName.text = quest.GetCurrentStepPrefab().GetComponent<QuestStep>().StepName;
                _description.text = quest.GetCurrentStepPrefab().GetComponent<QuestStep>().StepDescription;
                QuestManager.Instance.TrackedQuest = quest;
            };
            _root.Add(newButton);
            _questList.contentContainer.Add(newButton);
        }
    }

    private void OnEnable()
    {
        base.OnEnable();
        UpdateQuestList();
        QuestManager.Instance.OnStart += StartQuest;
        QuestManager.Instance.OnFinish += FinishQuest;
    }

    private void OnDisable()
    {
        QuestManager.Instance.OnStart -= StartQuest;
        QuestManager.Instance.OnFinish -= FinishQuest;
    }
}
