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
            if (DialogueManager.Instance.CurrentStory.variablesState[_targetVariable] != null &&
               (bool)DialogueManager.Instance.CurrentStory.variablesState[_targetVariable] == _targetState)
            {
                this.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
