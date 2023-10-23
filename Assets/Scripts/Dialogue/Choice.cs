using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Choice
{
    public string buttonText;
    public DialogueArgs resultDialogue;
    public bool enabled;
    public bool diceRoll;
    //stats
}

public class ChoiceClass : MonoBehaviour
{
    protected Choice InstantiateChoice(DialogueArgs nextDialogue,
                                       string buttonText,
                                       bool enabled,
                                       bool diceRoll)
    {
        Choice choice;
        choice.buttonText = buttonText;
        choice.resultDialogue = nextDialogue;
        choice.enabled = enabled;
        choice.diceRoll = diceRoll;
        return choice;
    }
}