using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : ChoiceClass
{
    DialogueArgs dialogue1 = new DialogueArgs();
    DialogueArgs dialogue2 = new DialogueArgs();
    DialogueArgs dialogue3 = new DialogueArgs();

    void Start()
    {
        dialogue1.Text = "Hey wassup";
        dialogue1.Choice1 = InstantiateChoice(dialogue2, "Powta", true, false);
        dialogue1.Choice2 = InstantiateChoice(dialogue3, "Sunna", true, false);


        dialogue2.Text = "oh shit";
        dialogue2.Choice1 = InstantiateChoice(dialogue1, "Back", true, false);

        dialogue3.Text = "Gabeh";
        dialogue3.Choice1 = InstantiateChoice(dialogue1, "Back", true, true);

        DialogueManager.Instance.StartDialogue(dialogue1);
    }

    void Update()
    {
        
    }
}
