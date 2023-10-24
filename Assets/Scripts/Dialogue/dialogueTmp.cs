using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using System.Collections;

public class dialogueTmp : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialoguePanel;
    public string[] dialogues;
    private int currentDialogueIndex = 0;
    public bool canContinue = false;
    public float textSpeed = 0.05f; // Adjust the speed as you prefer

    IEnumerator currentSentence;
    IEnumerator inputDelay;
    //IInteractable interact;

    void Start()
    {
        dialoguePanel.SetActive(false); // Initially hide dialogue
    }

    public void StartDialogue(string[] newDialogues) //IInteractable interact = null)
    {

        //this.interact = interact;

        dialogues = newDialogues;
        dialoguePanel.SetActive(true);
        currentDialogueIndex = 0;
        if (currentSentence != null)
        {
            StopCoroutine(this.currentSentence);
        }
        this.currentSentence = TypeSentence(dialogues[currentDialogueIndex]);

        inputDelay = delay();
        StartCoroutine(inputDelay);

        StartCoroutine(this.currentSentence); // Start coroutine
    }

    IEnumerator delay()
    {
        canContinue = false;
        yield return new WaitForSeconds(0.1f);
        canContinue = true;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = ""; // Clear current text
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Add letters one by one
            yield return new WaitForSeconds(textSpeed); // Wait for the defined time
        }
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E)) && canContinue) // Use Return or another key you prefer
        {
            ProceedDialogue();
        }
    }

    public void ProceedDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            if (currentSentence != null)
            {
                StopCoroutine(this.currentSentence);
            }
            this.currentSentence = TypeSentence(dialogues[currentDialogueIndex]);
            StartCoroutine(this.currentSentence); // Start coroutine for next sentence
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        canContinue = false;
        //PlayerManager.isInteracting = false;
        dialoguePanel.SetActive(false);
        // interact.endInteract();
        // interact = null;

        inputDelay = delay();
        StartCoroutine(inputDelay);
    }
}