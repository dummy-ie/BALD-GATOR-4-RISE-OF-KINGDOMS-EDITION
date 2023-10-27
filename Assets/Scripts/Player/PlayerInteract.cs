using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour
{
    private GameView _gameView;
    private List<Button> _buttons = new(3);
    // private List<DialogueClass> _nearbyDialogues = new();

    // Start is called before the first frame update
    void Start()
    {
        _gameView = ViewManager.Instance.GetComponentInChildren<GameView>();
        _buttons = _gameView.Root.Q("InteractButtons").Query<Button>().ToList();

        if (_buttons.Count == 0)
            Debug.LogError("No Buttons Found! " + name);

        if (_gameView == null)
            Debug.LogError("No GameView in ViewManager! " + name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        // _buttons = _gameView.Root.Q("InteractButtons").Query<Button>().ToList(); // refresh the list

        GameObject collided = other.gameObject;
        DialogueClass dialogue = collided.GetComponentInChildren<DialogueClass>();

        if (dialogue == null)
            Debug.LogError("Dialogue " + collided.name + " is null!");

        foreach (Button button in _buttons)
        {
            // Debug.Log("Evaluating " + button.name + ", Text: " + button.text + ", Display: " + button.style.display);
            if (button.text == "Button") // for some reason UNITY CANT FUCKING COMPARE DISPLAY STYLES
            {
                // Debug.Log("Displaying " + button.name);
                button.style.display = DisplayStyle.Flex;
                button.text = collided.name;
                button.clicked += () => DialogueManager.Instance.StartDialogue(dialogue, dialogue.CurrentDialogue); 
                break;
            }
        }

        // _nearbyDialogues.Add(dialogue);
        // Debug.Log("Added Dialogue " + collided.name);
        // Debug.Log("Count: " + _nearbyDialogues.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        // _buttons = _gameView.Root.Q("InteractButtons").Query<Button>().ToList(); // refresh list

        GameObject collided = other.gameObject;
        DialogueClass dialogue = collided.GetComponentInChildren<DialogueClass>();

        if (dialogue == null)
            Debug.LogError("Dialogue " + collided.name + " is null!");

        foreach (Button button in _buttons)
        {
            if (button.text == collided.name)
            {
                button.style.display = DisplayStyle.None;
                button.text = "Button";
                // button.clicked -= () => DialogueManager.Instance.StartDialogue(dialogue, dialogue.CurrentDialogue); 
                button.clickable = new Clickable(() => {}); // RESET ITS CLICKABLE FUNCTION
                break;
            }
        }

        // _nearbyDialogues.Remove(dialogue);
        // Debug.Log("Removed Dialogue " + collided.name);
        // Debug.Log("Count: " + _nearbyDialogues.Count);
    }
}
