using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : DialogueClass
{
    [SerializeField]
    DialogueView view;

    
    DialogueArgs dialogue1 = new();
    DialogueArgs dialogue2 = new();
    DialogueArgs dialogue3a = new();
    DialogueArgs dialogue3b = new();
    DialogueArgs dialogue4 = new();


    void InitializeDialogue()
    {
        dialogue1.Text = "wassup";
        dialogue1.Choice1 = InstantiateChoice("Powta", true, false, dialogue2);
        dialogue1.Choice2 = InstantiateChoice("Sunna", true, true, dialogue3a, dialogue3b);


        dialogue2.Text = "oh shit";
        dialogue2.Choice1 = InstantiateChoice("Back", true, false, dialogue1);


        dialogue3a.Text = "SUCCED";
        dialogue3a.Choice1 = InstantiateChoice("Next", true, false, dialogue4);

        dialogue3b.Text = "FAILEd";
        dialogue3b.Choice1 = InstantiateChoice("Back", true, false, dialogue1);

        dialogue4.Text = "OOMPH";

        CurrentDialogue = dialogue1;
    }

    private void Awake()
    {
        InitializeDialogue();

        view.Degub.clicked += StartDialogue;
    }

    void Start()
    {
        
    }

    void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(this, CurrentDialogue);
    }

    void Update()
    {
        
    }
}
