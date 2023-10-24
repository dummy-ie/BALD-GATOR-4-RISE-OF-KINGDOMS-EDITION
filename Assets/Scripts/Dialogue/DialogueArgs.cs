using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueArgs
{


    private string text;
    public string Text
    {
        get { return text; }
        set { text = value; }
    }


    private Choice choice1;
    public Choice Choice1
    {
        get { return choice1; }
        set { choice1 = value; }
    }

    private Choice choice2;
    public Choice Choice2
    {
        get { return choice2; }
        set { choice2 = value; }
    }
}
