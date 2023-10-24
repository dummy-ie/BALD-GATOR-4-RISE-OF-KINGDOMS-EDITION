using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using System.Collections;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] DialogueView view;

    DialogueArgs args;

    void ResetButtons()
    {
        view.Choice1.visible = false;
        view.Choice2.visible = false;

        view.Choice1.clicked -= Option1;
        view.Choice2.clicked -= Option2;
        //view.Choice3.clickable = null;
        //view.Choice4.clickable = null;
    }

    public void StartDialogue(DialogueArgs args)
    {
        view.BackGround.visible = true;
        view.BackGround.SetEnabled(true);

        ResetButtons();

        loadArgs(args);

        SetAllText();

        SetButtons();
    }

    void loadArgs(DialogueArgs args)
    {
        this.args = args;
    }


    void SetAllText()
    {
        view.Text.text = args.Text;

        view.Choice1.text = args.Choice1.buttonText;
        view.Choice2.text = args.Choice2.buttonText;
    }

    void SetButtons()
    {
        if (args.Choice1.enabled)
        {
            view.Choice1.visible = true;
            view.Choice1.clicked += Option1;
        }

        if (args.Choice2.enabled)
        {
            view.Choice2.visible = true;
            view.Choice2.clicked += Option2;
        }
    }

    void EndDialogue()
    {
        view.BackGround.visible = false;
        view.BackGround.SetEnabled(false);
    }

    void Option1()
    {
        if (args.Choice1.diceRoll)
        {
            args.Choice1 = disableButton(args.Choice1);
        }
        StartDialogue(args.Choice1.resultDialogue);
        
    }

    void Option2()
    {
        if (args.Choice2.diceRoll)
        {
            args.Choice2 = disableButton(args.Choice2);
        }
        StartDialogue(args.Choice2.resultDialogue);
    }

    public void StartCombat()
    {
        EndDialogue();
    }

    public void LeaveDialogue()
    {
        EndDialogue();
    }

    Choice disableButton(Choice choice)
    {
        Choice newChoice;
        newChoice = choice;
        newChoice.enabled = false;
        return newChoice;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        view.Choice3.clicked += StartCombat;
        view.Choice4.clicked += LeaveDialogue;
    }



}