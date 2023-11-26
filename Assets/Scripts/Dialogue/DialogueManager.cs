using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] DialogueView _view;
    public DialogueView View { get { return _view; } }

    [SerializeField]
    TextAsset _inkJSON;

    private Story _currentStory;

    private bool _isDialoguePlaying;
    public bool IsDialoguePlaying
    {
        get { return _isDialoguePlaying;}
        set { _isDialoguePlaying = value;}
    }


    public void EnterDialogue(TextAsset inkJSON)
    {
        Debug.Log("Entering Dialogue");
        _currentStory = new Story(inkJSON.text);
        _isDialoguePlaying = true;

        _currentStory.BindExternalFunction("test", (string text) =>
        {
            Debug.Log("Chose " + text);
        });

        _currentStory.BindExternalFunction("setroll", () =>
        {
            _currentStory.variablesState["roll"] = InternalDice.Instance.Roll(out _, 20, 0, 10);
        });

        ShowView();

        ContinueDialogue();
        
    }



    public IEnumerator ExitDialogue(){
        yield return new WaitForSeconds(0.2f);
        _isDialoguePlaying = false;

        _currentStory.UnbindExternalFunction("test");

        _view.Text.text = "";
        HideView();
    }

    public void ContinueDialogue()
    {
        if (_currentStory.canContinue)
        {
            _view.Text.text = _currentStory.Continue();

            SetButtons();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
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
        SetOutcome(0);
        ContinueDialogue();
    }

    private void Choice2()
    {
        SetOutcome(1);
        ContinueDialogue();
    }

    private void Fight()
    {
        StartCoroutine(ExitDialogue());
    }

    private void Leave()
    {
        StartCoroutine(ExitDialogue());
    }

    

    private void HideView()
    {
        _view.BackGround.visible = false;
        _view.BackGround.SetEnabled(false);

    }

    private void ShowView()
    {
        _view.BackGround.visible = true;
        _view.BackGround.SetEnabled(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        

        _view.Choices[0].clicked += Choice1;
        _view.Choices[1].clicked += Choice2;
        _view.Choices[2].clicked += Fight;
        _view.Choices[3].clicked += Leave;

        HideView();
        _isDialoguePlaying = false;

        EnterDialogue(_inkJSON);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDialoguePlaying)
        {
            return;
        }
    }
}
