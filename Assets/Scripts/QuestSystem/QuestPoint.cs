using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [SerializeField] private QuestData _questData;
    [SerializeField] private bool _startPoint = true;
    [SerializeField] private bool _finishPoint = true;

    private bool _nearPlayer = false;
    private string _id;
    private EQuestState _currentState;

    //private QuestIcon questIcon;

    private void Awake() {
        _id = _questData.ID;
        //questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        //GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        //GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        //GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        //GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed() {
        if (!_nearPlayer) {
            return;
        }

        // start or finish a quest
        if (_currentState.Equals(EQuestState.CAN_START) && _startPoint) {
            QuestManager.Instance.StartQuest(_id);
        }
        else if (_currentState.Equals(EQuestState.CAN_FINISH) && _finishPoint) {
            QuestManager.Instance.FinishQuest(_id);
        }
    }

    private void StateChange(Quest quest) {
        if (quest.Info.ID.Equals(_id))
        {
            _currentState = quest.State;
            //questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nearPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nearPlayer = false;
        }
    }
}