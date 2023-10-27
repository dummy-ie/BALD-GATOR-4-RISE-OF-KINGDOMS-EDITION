using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartenderDialogue : DialogueClass
{
    DialogueView _view;

    DialogueArgs dialogue1 = new();
    DialogueArgs dialogue2 = new();
    DialogueArgs dialogue3 = new();
    DialogueArgs dialogue4 = new();
    DialogueArgs dialogue5a = new();
    DialogueArgs dialogue5b = new();
    DialogueArgs dialogue6a = new();
    DialogueArgs dialogue6b = new();
    DialogueArgs dialogue7 = new();


    void InitializeDialogue()
    {
        dialogue1.Text = "Well-met traveler. You look weary, perhaps you would like some of our blue horse beer?";
        dialogue1.Choice1 = InstantiateChoice("I am here to ask about the One Piece", true, false, dialogue3);
        dialogue1.Choice2 = InstantiateChoice("Sure. Give me a drink, bartender.", true, false, dialogue2);
        //dialogue1.Choice3 = InstantiateChoice("I’m not thirsty", true, false, dialogue7);


        dialogue2.Text = "One blue horse beer coming right up!";
        dialogue2.Choice1 = InstantiateChoice("Blue horse extra cool! Ito ang lamig!", true, false, dialogue7);
        dialogue2.Choice2 = InstantiateChoice("Thanks, I'll be going now.", true, false, dialogue3);

        dialogue3.Text = "Hmm. Yes. The One Piece, legendary treasure yes? Very well then. You lot would be seeking God Enel. They lie behind the Gate of Justice. Unfortunately you would need to unlock it. Head down the road. There lies a tower right before the gate, perhaps you’d find some way of unlocking the gate at the Celestial Ascent tower.";

        CurrentDialogue = dialogue1;
    }

    void Start()
    {
        this._view = DialogueManager.Instance.View;
        InitializeDialogue();
        //view.Degub.clicked += StartDialogue;
    }

    void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(this, CurrentDialogue);
    }

    void Update()
    {

    }
}
