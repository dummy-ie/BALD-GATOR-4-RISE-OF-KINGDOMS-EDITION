using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.IO;

//if a "Ink.Parsed" appears here just remove it
public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] DialogueView _view;
    public DialogueView View { get { return _view; } }

    private Dictionary<string, Ink.Runtime.Object> _variables;
    [SerializeField]InkFile _file;

    private Story _currentStory;

    private string _name;
    public string Name
    {
        get { return _name; }
    }

    private bool _isDialoguePlaying;
    private bool _isDiceRolling;
    public bool IsDialoguePlaying
    {
        get { return _isDialoguePlaying;}
        set { _isDialoguePlaying = value;}
    }


    void InitializeVariables()
    {
        
        string inkFileContents = File.ReadAllText(_file.filePath);
        Ink.Compiler compiler = new(inkFileContents);
        Story tempStory = compiler.Compile();

        _variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in tempStory.variablesState)
        {

            Ink.Runtime.Object value = tempStory.variablesState.GetVariableWithName(name);
            _variables.Add(name, value);
            Debug.Log("Initialized Variable " + name + " : " + value);
        }
    }


    public void EnterDialogue(TextAsset inkJSON)
    {
        Debug.Log("Entering Dialogue");
        _currentStory = new Story(inkJSON.text);

        _name = (string)_currentStory.variablesState["name"];

        _isDialoguePlaying = true;

        _currentStory.BindExternalFunction("RollDice", (string stat) =>
        {
            StartCoroutine(RollDice(stat));
        });

        _currentStory.BindExternalFunction("StartQuest", (string questId) =>
        {
            QuestManager.Instance.OnStart(questId);
        });


        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables)
        {
            _currentStory.variablesState.SetGlobal(variable.Key, variable.Value);
        }

        _currentStory.variablesState.variableChangedEvent += VariableChanged;
        ShowView();

        ContinueDialogue();
        
    }

    IEnumerator RollDice(string stat)
    {
        SceneManager.LoadScene("Dice Roller", LoadSceneMode.Additive);

        _isDiceRolling = true;
        yield return new WaitForSeconds(.5f);

        FindObjectOfType<ExternalDice>().DifficultyClass = (int)_currentStory.variablesState[_name + "Check" + stat];
    }

    public IEnumerator ExitDialogue(){
        yield return new WaitForSeconds(0.2f);

        _currentStory.variablesState.variableChangedEvent -= VariableChanged;

        _isDialoguePlaying = false;

        _currentStory.UnbindExternalFunction("RollDice");
        _currentStory.UnbindExternalFunction("StartQuest");

        _view.Text.text = "";
        HideView();
    }

    public void ContinueDialogue()
    {
        if (_currentStory.canContinue && !_isDiceRolling)
        {
            _view.Text.text = _currentStory.Continue();

            SetButtons();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (_variables.ContainsKey(name))
        {
            _variables[name] = value;
        }
    }

    public void SetDiceRoll(bool diceRoll)
    {
        AddButtons();

        _view.AssignButtons();
        ShowView();
        _isDiceRolling = false;
        _currentStory.variablesState["diceRoll"] = diceRoll;
        ContinueDialogue();
    }

    private void SetButtons()
    {

        _view.Choices[0].visible = false;
        _view.Choices[0].SetEnabled(false);
        _view.Choices[1].visible = false;
        _view.Choices[1].SetEnabled(false);

        List<Choice> currentChoices = _currentStory.currentChoices;
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            
            _view.Choices[index].SetEnabled(true);
            _view.Choices[index].visible = true;
            _view.Choices[index].text = choice.text;
            index++;
        }

        
    }
    
    public void SetOutcome(int index)
    {
        _currentStory.ChooseChoiceIndex(index);
    }

    private void Choice1()
    {
        Debug.Log("Clicked Choice 1");
        SetOutcome(0);
        ContinueDialogue();
    }

    private void Choice2()
    {
        Debug.Log("Clicked Choice 2");
        SetOutcome(1);
        ContinueDialogue();
    }

    private void Fight()
    {
        StartCoroutine(ExitDialogue());
        //CombatManager.Instance.StartCombat();
    }

    private void Leave()
    {
        StartCoroutine(ExitDialogue());
    }

    


    private void HideView()
    {
        foreach (Button button in _view.Choices)
        {
            button.visible = false;
            button.SetEnabled(false);
        }

        _view.BackGround.visible = false;
        _view.BackGround.SetEnabled(false);

    }

    private void ShowView()
    {
        foreach (Button button in _view.Choices)
        {
            button.visible = true;
            button.SetEnabled(true);
        }

        _view.BackGround.visible = true;
        _view.BackGround.SetEnabled(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();

        //HideView();
        _isDialoguePlaying = false;
    }

    void AddButtons()
    {
        _view.Choices[0].clicked += Choice1;
        _view.Choices[1].clicked += Choice2;
        _view.Choices[2].clicked += Fight;
        _view.Choices[3].clicked += Leave;
    }

    private void OnEnable()
    {
        AddButtons();
    }

    private void OnDisable()
    {
        _view.Choices[0].clicked -= Choice1;
        _view.Choices[1].clicked -= Choice2;
        _view.Choices[2].clicked -= Fight;
        _view.Choices[3].clicked -= Leave;
    }
}
