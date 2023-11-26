using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool _finished = false;
    protected void FinishStep() { 
        if (!_finished) {
            _finished = true;
            Destroy(gameObject);
        }
    }
}
