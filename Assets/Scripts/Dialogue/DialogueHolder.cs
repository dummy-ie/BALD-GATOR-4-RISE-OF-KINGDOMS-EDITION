using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField] private TextAsset _inkDialogue;
    public TextAsset InkDialogue
    {
        get { return _inkDialogue; }
    }
}
