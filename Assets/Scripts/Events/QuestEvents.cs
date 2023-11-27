using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestEvents
{
    public UnityAction<string> OnStart;
    public UnityAction<string, int, bool> OnAdvance;
    public UnityAction<string> OnFinish;
    public UnityAction<Quest> OnQuestStateChange;
}
