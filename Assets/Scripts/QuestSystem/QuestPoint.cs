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


    public void SubmitPressed() {
        // start or finish a quest
        Debug.Log(_currentState);
        if (_currentState.Equals(EQuestState.CAN_START) && _startPoint) {
        Debug.Log("subtmitiiedd");
            EventsManager.Instance.QuestEvents.OnStart(_id);
        }
        else if (_currentState.Equals(EQuestState.CAN_FINISH) && _finishPoint) {
            EventsManager.Instance.QuestEvents.OnFinish(_id);
        }
    }

    private void StateChange(Quest quest) {
        if (quest.Info.ID.Equals(_id))
        {
            Debug.Log("yea");
            _currentState = quest.State;
            //questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }
    private void Awake() {
        _id = _questData.ID;
    }
    private void OnEnable()
    {
        EventsManager.Instance.QuestEvents.OnQuestStateChange += StateChange;
    }

    private void OnDisable()
    {
        EventsManager.Instance.QuestEvents.OnQuestStateChange -= StateChange;
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