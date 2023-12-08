[System.Serializable]
public class QuestStepState {
    private string _state;
    public string State { 
        get { return _state; }
        set { _state = value; }
    }
    private string _description;
    public string Description {
        get { return _description; }
        set { _description = value; }
    }

    public QuestStepState(string state = "", string description = "")
    {
        this._state = state;
        this._description = description;
    }
}
