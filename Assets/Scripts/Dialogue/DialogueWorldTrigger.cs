using UnityEngine;

public class DialogueWorldTrigger : MonoBehaviour
{
    [SerializeField] GameObject _targetObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DialogueManager.Instance.EnterDialogue(_targetObject);
        }
    }
}
