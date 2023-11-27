using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : Singleton<EventsManager>
{
    public QuestEvents QuestEvents;
    protected override void OnAwake()
    {
        QuestEvents = new QuestEvents();
    }
}
