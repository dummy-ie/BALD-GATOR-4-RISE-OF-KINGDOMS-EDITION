using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;

public class TEstDialogue : MonoBehaviour
{

    [SerializeField]
    TextAsset _textAssetTotoy;

    [SerializeField]
    TextAsset _textAssetSenren;

    [SerializeField]
    bool totoy = false;

    [SerializeField]
    bool senren = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (totoy)
        {
            Debug.Log("Selecting Totoy");
            totoy = false;
            DialogueManager.Instance.EnterDialogue(_textAssetTotoy);
           
        }

        if (senren)
        {
            Debug.Log("Selecting Senren");
            senren = false;
            DialogueManager.Instance.EnterDialogue(_textAssetSenren);
        }
    }
}
