using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using System.Collections;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] DialogueView view;

    public DialogueView View { get { return view; } }

    DialogueArgs args;
    DialogueClass target;

    void ResetButtons()
    {
        view.Choice1.visible = false;
        view.Choice2.visible = false;

        view.Choice1.clicked -= Option1;
        view.Choice2.clicked -= Option2;
    }

    public void StartDialogue(DialogueClass target, DialogueArgs args)
    {
        // view.Initialize(); // refresh

        if (this.target == null)
            loadTarget(target);

        this.target.CurrentDialogue = args;

        view.BackGround.visible = true;
        view.BackGround.SetEnabled(true);

        ResetButtons();

        loadArgs(args);
        loadTarget(target);

        SetAllText();

        SetButtons();
    
        ViewManager.Instance.Show(view);
    }

    void loadArgs(DialogueArgs args)
    {
        this.args = args;
    }

    void loadTarget(DialogueClass target)
    {
        this.target = target;
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
        view.Choice1.visible = false;
        view.Choice2.visible = false;

        view.BackGround.visible = false;
        view.BackGround.SetEnabled(false);

        ViewManager.Instance.Show(ViewManager.Instance.GetComponentInChildren<GameView>());
    }

    void Option1()
    {
        if (args.Choice1.diceRoll && args.Choice1.otherResultDialogue != null)
        {
            args.Choice1 = DisableButton(args.Choice1);
            if (CheckDiceOutcome())
                StartDialogue(target, args.Choice1.resultDialogue);
            else
                StartDialogue(target, args.Choice1.otherResultDialogue);
        }
        else
        {
            StartDialogue(target, args.Choice1.resultDialogue);
        }
        
        
    }

    void Option2()
    {
        if (args.Choice2.diceRoll && args.Choice2.otherResultDialogue != null)
        {
            args.Choice2 = DisableButton(args.Choice2);
            if (CheckDiceOutcome())
                StartDialogue(target, args.Choice2.resultDialogue);
            else
                StartDialogue(target, args.Choice2.otherResultDialogue);
        }
        else
        {
            StartDialogue(target, args.Choice2.resultDialogue);
        }
        
    }

    bool CheckDiceOutcome()
    {
        if (InternalDice.Instance.RollInternal(10)) //SUCCESS
        {
            Debug.Log("SUCCESS");
            return true; 
        }
        else
        {
            Debug.Log("FAILED");
            return false;
        }
    }

    public void StartCombat()
    {
        EndDialogue();
    }

    public void LeaveDialogue()
    {
        EndDialogue();
    }

    Choice DisableButton(Choice choice)
    {
        Choice newChoice;
        newChoice = choice;
        newChoice.enabled = false;
        return newChoice;
    }

    Choice EnableButton(Choice choice)
    {
        Choice newChoice;
        newChoice = choice;
        newChoice.enabled = true;
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