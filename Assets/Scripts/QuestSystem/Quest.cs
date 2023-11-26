using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {
    private QuestData _info;
    public QuestData Info { 
        get { return _info; }
    }
    private EQuestState _state;
    public EQuestState State { 
        get { return _state; }
    }
    private int _currentStepIndex;
    private QuestStep[] _steps;
    public Quest(QuestData questInfo) {
        this._info = questInfo;
        this._state = EQuestState.CAN_START;
        this._currentStepIndex = 0;
        //this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        //for (int i = 0; i < questStepStates.Length; i++)
        {
            //QuestStep[i] = new QuestStepState();
        }
    }
}
