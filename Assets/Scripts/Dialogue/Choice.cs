using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Choice
{
    
    public DialogueArgs resultDialogue;
    public DialogueArgs otherResultDialogue; //THIS IS FOR DICE ROLL OPTIONS WITH FAIL OUTCOMES
    public string buttonText;
    public bool enabled;
    public bool diceRoll;
    //stats
}

public class DialogueClass : MonoBehaviour
{
    DialogueArgs currentDialogue;
    public DialogueArgs CurrentDialogue
    {
        get { return currentDialogue; }
        set { currentDialogue = value; }
    }

    protected Choice InstantiateChoice(string buttonText, // BUTTON TEXT
                                       bool enabled, // OPTION IS PRESSABLE
                                       bool diceRoll, // OPTION INITIATES DICE ROLL
                                       DialogueArgs nextDialogue, // NEXT DIALOGUE
                                       DialogueArgs otherDialogue = null)//NORMALLY EMPTY IF NOT DICE ROLL OPTION
    {
        Choice choice;
        
        choice.buttonText = buttonText;
        choice.enabled = enabled;
        choice.diceRoll = diceRoll;
        choice.resultDialogue = nextDialogue;
        choice.otherResultDialogue = otherDialogue;

        return choice;
    }

}