using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.IO;
using static UnityEngine.EventSystems.EventTrigger;

//if a "Ink.Parsed" appears here just remove it
public class DialogueManager : Singleton<DialogueManager>
{
    DialogueView _view;
    public DialogueView View { get { return _view; } }

    private Dictionary<string, Ink.Runtime.Object> _variables;
    [SerializeField]InkFile _file;

    private GameObject _characterReference;
    private GameObject _characterReference2;
    private Story _currentStory;

    private string _name;
    public string Name
    {
        get { return _name; }
    }

    private bool _isDialoguePlaying;
    public bool IsDialoguePlaying
    {
        get { return _isDialoguePlaying; }
        set { _isDialoguePlaying = value; }
    }

    private bool _isDiceRolling;
    private bool _fightOngoing;

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
            //Debug.Log("Initialized Variable " + name + " : " + value);
        }
    }


    public void EnterDialogue(GameObject character)
    {
        
        Debug.Log("Entering Dialogue");
        AddButtons();

        _characterReference = character;
        _currentStory = new Story(character.GetComponentInChildren<DialogueHolder>().InkDialogue.text);

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

        _currentStory.BindExternalFunction("FinishQuest", (string questId) =>
        {
            QuestManager.Instance.FinishQuest(questId);
        });

        _currentStory.BindExternalFunction("AdvanceQuest", (string questId) =>
        {
            QuestManager.Instance.FinishQuest(questId);
        });

        _currentStory.BindExternalFunction("IncreaseStat", (string stat) =>
        {
            Entity player = CombatManager.Instance.CurrentSelected.GetComponent<Entity>();
            if (player != null)
            {
                switch (stat)
                {
                    case "CHA": player.Class.Charisma++; break;
                    case "STR": player.Class.Strength++; break;
                    case "INT": player.Class.Intelligence++; break;
                    case "DEX": player.Class.Dexterity++; break;
                    case "CON": player.Class.Constitution++; break;
                    case "WIS": player.Class.Wisdom++; break;
                }
            }

            
        });

        _currentStory.BindExternalFunction("Fight", () =>
        {
            StartCoroutine(StartBattleState());
        });
        _currentStory.BindExternalFunction("SwitchFight", (string target) =>
        {
            StartCoroutine(StartBattleState(target));
        });
        _currentStory.BindExternalFunction("Kill", () =>
        {
            Destroy(_characterReference);
        });
        _currentStory.BindExternalFunction("SwitchKill", (string target) =>
        {
            if (target != null)
            {
                GameObject obj = GameObject.Find(target);
                if (obj != null)
                    Destroy(obj);
                else Debug.LogError("There is no target named:" + target + ". Check INK file!");
            }
                
        });
        _currentStory.BindExternalFunction("Leave", (bool returnable) =>
        {
            Leave(returnable);
        });

        

        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables)
        {
            _currentStory.variablesState.SetGlobal(variable.Key, variable.Value);
        }

        _currentStory.variablesState.variableChangedEvent += VariableChanged;
        ShowView();

        ContinueDialogue();
        
    }

   

    public IEnumerator ExitDialogue(){

        RemoveButtons();

        yield return new WaitForSeconds(0.2f);

        _currentStory.variablesState.variableChangedEvent -= VariableChanged;

        _isDialoguePlaying = false;

        _currentStory.UnbindExternalFunction("RollDice");
        _currentStory.UnbindExternalFunction("StartQuest");
        _currentStory.UnbindExternalFunction("AdvanceQuest");
        _currentStory.UnbindExternalFunction("FinishQuest");
        _currentStory.UnbindExternalFunction("IncreaseStat");
        _currentStory.UnbindExternalFunction("Fight");
        _currentStory.UnbindExternalFunction("SwitchFight");
        _currentStory.UnbindExternalFunction("Kill");
        _currentStory.UnbindExternalFunction("SwitchKill");
        _currentStory.UnbindExternalFunction("Leave");

        _view.Text.text = "";
        HideView();
    }

    public void ContinueDialogue()
    {
        if (_currentStory.canContinue && !_isDiceRolling && !_fightOngoing)
        {
            _view.Text.text = _currentStory.Continue();

            SetButtons();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    

    
    
    public void SetOutcome(int index)
    {
        _currentStory.ChooseChoiceIndex(index);
    }

    public bool GetBoolInkVar(string varName)
    {
        return (bool)_currentStory.variablesState[varName];
    }

    public int GetIntInkVar(string varName)
    {
        return (int)_currentStory.variablesState[varName];
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (_variables.ContainsKey(name))
        {
            _variables[name] = value;
        }
    }

    public void SetDiceRoll(bool diceRoll)
    {
        
        CombatManager.Instance.CurrentSelected = _characterReference2;

        if (InternalDice.Instance.RollType == ERollType.CRITICAL_SUCCESS || InternalDice.Instance.RollType == ERollType.CRITICAL_FAIL)
        {
            _currentStory.variablesState["diceRoll"] = InternalDice.Instance.Roll(out int i);
        }
        else
        {
            _currentStory.variablesState["diceRoll"] = diceRoll;
        }

        _isDiceRolling = false;
        _view.AssignButtons();
        ViewManager.Instance.GetView<GameView>().Show();
        foreach (GameObject player in PartyManager.Instance.PartyMembers)
        {
            player.GetComponentInChildren<PlayerInteract>().AssignButtons();
        }
        ShowView();
        AddButtons();
        ContinueDialogue();
    }

    public IEnumerator EndBattleState(bool battleWon)
    {
        Debug.Log("Ending Battle");
        
        yield return new WaitForSeconds(.5f);

        _currentStory.variablesState["battleWon"] = battleWon;

        _fightOngoing = false;
        ViewManager.Instance.GetView<GameView>().Show();
        foreach (GameObject player in PartyManager.Instance.PartyMembers)
        {
            player.GetComponentInChildren<PlayerInteract>().AssignButtons();
        }
        ShowView();
        AddButtons();
        ContinueDialogue();
    }

    private IEnumerator StartBattleState(string target = null)
    {
        Debug.Log("Starting battle");
        yield return new WaitForSeconds(.5f);
        RemoveButtons();
        HideView();
        _fightOngoing = true;

        List<Entity> combatants = new List<Entity>();
        foreach (GameObject combatant in PartyManager.Instance.PartyMembers)
        {
            combatants.Add(combatant.GetComponent<Entity>());
        }
        if (target != null) {
            GameObject obj = GameObject.Find(target);
            if (obj != null)
                combatants.Add(obj.GetComponent<Entity>());
            else Debug.LogError("There is no target named:" + target + ". Check INK file!");
        }
        else
            combatants.Add(_characterReference.GetComponent<Entity>());

        StartCoroutine(CombatManager.Instance.StartCombat(combatants));
    }

    

    private IEnumerator RollDice(string stat)
    {
        RemoveButtons();
        HideView();
        ViewManager.Instance.GetView<GameView>().Hide();
        _isDiceRolling = true;

        _characterReference2 = CombatManager.Instance.CurrentSelected;

        SceneManager.LoadScene("Dice Roller", LoadSceneMode.Additive);
        
        yield return new WaitForSeconds(.5f);

        _currentStory.variablesState[_name + "HasRolled" + stat] = false;

        Entity player = CombatManager.Instance.CurrentSelected.GetComponent<Entity>();
        int statValue = 20;
        if (player != null)
        {
            switch (stat)
            {
                case "CHA": statValue = player.Class.Charisma; break;
                case "STR": statValue = player.Class.Strength; break;
                case "INT": statValue = player.Class.Intelligence; break;
                case "DEX": statValue = player.Class.Dexterity; break;
                case "CON": statValue = player.Class.Constitution; break;
                case "WIS": statValue = player.Class.Wisdom; break;
            }
        }
        FindObjectOfType<ExternalDice>().DifficultyClass = (int)_currentStory.variablesState[_name + stat];
        
    }

    private void SetButtons()
    {

        foreach (Button button in _view.Choices)
        {
            button.visible = false;
            button.SetEnabled(false);
        }

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

    private void Choice3()
    {
        Debug.Log("Clicked Choice 3");
        SetOutcome(2);
        ContinueDialogue();
    }

    private void Choice4()
    {
        Debug.Log("Clicked Choice 4");
        SetOutcome(3);
        ContinueDialogue();
    }

    private void Leave(bool returnable)
    {
        Debug.Log("Leaving Dialogue");
        if (!returnable)
        {
            _currentStory.variablesState[_name + "CanTalkTo"] = false;
        }

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
        _view = ViewManager.Instance.GetView<DialogueView>();
        //HideView();
        _isDialoguePlaying = false;
    }

    void AddButtons()
    {
        _view.Choices[0].clicked += Choice1;
        _view.Choices[1].clicked += Choice2;
        _view.Choices[2].clicked += Choice3;
        _view.Choices[3].clicked += Choice4;
    }

    void RemoveButtons()
    {
        _view.Choices[0].clicked -= Choice1;
        _view.Choices[1].clicked -= Choice2;
        _view.Choices[2].clicked -= Choice3;
        _view.Choices[3].clicked -= Choice4;
    }
}
