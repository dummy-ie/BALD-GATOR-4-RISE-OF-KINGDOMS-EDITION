using System.Collections.Generic;
using UnityEngine;

public class TEstDialogue : MonoBehaviour
{

    [SerializeField]
    GameObject _textAssetTotoy;

    [SerializeField]
    GameObject _textAssetSenren;

    [SerializeField]
    bool totoy = false;

    [SerializeField]
    bool senren = false;

    [SerializeField] Entity entity1;
    [SerializeField] Entity entity2;
    [SerializeField] Entity entity3;
    [SerializeField] Entity entity4;
    List<Entity> entities;
    public List<Entity> Entities { get { return entities; } }

    // Start is called before the first frame update
    void Start()
    {
        entities.Add(entity1);
        entities.Add(entity2);   
        entities.Add(entity3);
        entities.Add(entity4);
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
