using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] DialogueView _view;

    public DialogueView View { get { return _view; } }

    private DialogueArgs _args;

    private DialogueArgs _nextArgsOther;
    public DialogueArgs NextArgsOther { get { return _nextArgsOther; } }
    private DialogueArgs _nextArgs;
    public DialogueArgs NextArgs { get { return _nextArgs; } }

    private DialogueClass _target;
    public DialogueClass Target { get { return _target; } }

    private ExternalDice _externalDice;

    void ResetButtons()
    {
        _view.Choice1.visible = false;
        _view.Choice2.visible = false;

        _view.Choice1.clicked -= Option1;
        _view.Choice2.clicked -= Option2;
        // _view.Choice1.clickable = new Clickable(() => { });
        // _view.Choice2.clickable = new Clickable(() => { });
    }

    public void StartDialogue(DialogueClass target, DialogueArgs args)
    {
        // view.Initialize(); // refresh

        if (this._target == null)
            loadTarget(target);

        this._target.CurrentDialogue = args;

        _view.BackGround.visible = true;
        _view.BackGround.SetEnabled(true);

        ResetButtons();

        loadArgs(args);
        loadTarget(target);

        SetAllText();

        SetButtons();

        ViewManager.Instance.Show(_view);
    }

    void loadArgs(DialogueArgs args)
    {
        this._args = args;
    }

    void loadTarget(DialogueClass target)
    {
        this._target = target;
    }


    void SetAllText()
    {
        _view.Text.text = _args.Text;

        _view.Choice1.text = _args.Choice1.buttonText;
        _view.Choice2.text = _args.Choice2.buttonText;
    }

    void SetButtons()
    {
        if (_args.Choice1.enabled)
        {
            _view.Choice1.visible = true;
            _view.Choice1.clicked += Option1;
        }

        if (_args.Choice2.enabled)
        {
            _view.Choice2.visible = true;
            _view.Choice2.clicked += Option2;
        }
    }

    void EndDialogue()
    {
        _view.Choice1.visible = false;
        _view.Choice2.visible = false;

        _view.BackGround.visible = false;
        _view.BackGround.SetEnabled(false);

        ViewManager.Instance.Show(ViewManager.Instance.GetComponentInChildren<GameView>());
    }

    void Option1()
    {
        if (_args.Choice1.diceRoll && _args.Choice1.otherResultDialogue != null)
        {
            if (InternalDice.Instance.RollType == ERollType.CRITICAL_SUCCESS)
                StartDialogue(_target, _args.Choice1.resultDialogue);
            else if (InternalDice.Instance.RollType == ERollType.CRITICAL_FAIL)
                StartDialogue(_target, _args.Choice1.otherResultDialogue);

            SceneManager.LoadScene("Dice Roller", LoadSceneMode.Additive);

            _args.Choice1 = DisableButton(_args.Choice1);
            _nextArgs = _args.Choice1.resultDialogue;
            _nextArgsOther = _args.Choice1.otherResultDialogue;
        }
        else
        {
            StartDialogue(_target, _args.Choice1.resultDialogue);
        }
    }

    void Option2()
    {
        if (_args.Choice2.diceRoll && _args.Choice2.otherResultDialogue != null)
        {
            if (InternalDice.Instance.RollType == ERollType.CRITICAL_SUCCESS)
                StartDialogue(_target, _args.Choice2.resultDialogue);
            else if (InternalDice.Instance.RollType == ERollType.CRITICAL_FAIL)
                StartDialogue(_target, _args.Choice2.otherResultDialogue);

            SceneManager.LoadScene("Dice Roller", LoadSceneMode.Additive);

            _args.Choice2 = DisableButton(_args.Choice2);
            _nextArgs = _args.Choice2.resultDialogue;
            _nextArgsOther = _args.Choice2.otherResultDialogue;
        }
        else
        {
            StartDialogue(_target, _args.Choice2.resultDialogue);
        }
    }

    // public bool CheckDiceOutcome()
    // {
    //     Debug.Log("Check dice outcome");
    //     if (InternalDice.Instance.RollType == ERollType.CRITICAL_SUCCESS)
    //         return true;
    //     else if (InternalDice.Instance.RollType == ERollType.CRITICAL_FAIL)
    //         return false;

    //     // Debug.Log("Result: " + ExternalDice.ResultExternal(10));
    //     if (ExternalDice.ResultExternal(10))
    //         return true;
    //     else
    //         return false;
    // }

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
        _view.Choice3.clicked += StartCombat;
        _view.Choice4.clicked += LeaveDialogue;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if (scene.name == "Dice Roller")
        {
            Debug.Log("Loaded Dice Roller");

            ViewManager.Instance.Show(ViewManager.Instance.GetComponentInChildren<DiceRollView>());

            _externalDice = scene.GetRootGameObjects()[0].GetComponentInChildren<ExternalDice>();
            if (_externalDice == null)
                Debug.LogError("External dice null!");
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        ViewManager.Instance.Show(_view);
    }
}