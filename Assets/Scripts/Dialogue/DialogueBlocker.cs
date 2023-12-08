using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBlocker : MonoBehaviour
{

    [SerializeField] private string _targetVariable;
    [SerializeField] private bool _targetState;

    // Start is called before the first frame update
    void Start()
    {
        
    } 


    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.CurrentStory != null)
        {
            if ((bool)DialogueManager.Instance.CurrentStory.variablesState[_targetVariable] == _targetState)
            {
                this.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
