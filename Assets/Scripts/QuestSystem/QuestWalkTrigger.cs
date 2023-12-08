using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestWalkTrigger : MonoBehaviour
{
    [SerializeField] private string _questId;
    [SerializeField] private int _index = -1;
    private bool _active = true;

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
        if (_active)
        {
            Debug.Log("Advancing Quest");
            QuestManager.Instance.FinishCurrentStep(_questId, _index);
            _active = false;
        }
        //Destroy(gameObject);
    }
}
