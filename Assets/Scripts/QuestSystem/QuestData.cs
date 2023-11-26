using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/QuestData")]
public class QuestData : ScriptableObject {
    [SerializeField]
    private string _id;
    public string ID {
        get { return _id; }
    }

    [SerializeField]
    private string _displayName;
    public string DisplayName {
        get { return _displayName; }
    }
    [SerializeField]
    private string _description;
    public string Description {
        get { return _description; }
    }

    private QuestData[] _prerequisites;
    public QuestData[] Prerequisites {
        get { return _prerequisites; }
    }

    private GameObject[] _steps;
    public GameObject[] Steps {
        get { return _steps; }
    }

    private void OnValidate() {
#if UNITY_EDITOR
        _id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
