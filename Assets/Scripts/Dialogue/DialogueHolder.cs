using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField] private TextAsset _inkDialogue;
    public TextAsset InkDialogue
    {
        get { return _inkDialogue; }
    }
}
