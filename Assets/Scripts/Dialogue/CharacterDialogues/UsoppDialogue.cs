using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsoppDialogue : DialogueClass {
    DialogueView _view;

    DialogueArgs dialogue1 = new();
    DialogueArgs dialogue2 = new();
    DialogueArgs dialogue3 = new();
    DialogueArgs dialogue4 = new();
    DialogueArgs dialogue5a = new();
    DialogueArgs dialogue5b = new();
    DialogueArgs dialogue6a = new();
    DialogueArgs dialogue6b = new();
    DialogueArgs dialogue7 = new();


    void InitializeDialogue() {
        dialogue1.Text = "Welcome to Merry Island, Traveler";
        dialogue1.Choice1 = InstantiateChoice("I'll kill you!", true, false, dialogue7);
        dialogue1.Choice2 = InstantiateChoice("Thanks! I'm going now.", true, false, dialogue3);
        dialogue1.Choice3 = InstantiateChoice("What's going on around here?", true, false, dialogue2);


        dialogue2.Text = "Some 'God' decided to… well, play god with the inhabitants of the island. Not in a good way at that.The people are suffering under their rule.";
        dialogue2.Choice1 = InstantiateChoice("Thanks, I'll kill you.", true, false, dialogue7);
        dialogue2.Choice2 = InstantiateChoice("Thanks, I'll be going now.", true, false, dialogue3);


        dialogue3.Text = "Nuh-uh-uh! Hold on a minute. You gotta pay.";
        dialogue3.Choice1 = InstantiateChoice("Give them a bag of rocks.", true, false, dialogue4);
        dialogue3.Choice2 = InstantiateChoice("Seduce.", true, true, dialogue5a, dialogue5b);
        dialogue3.Choice3 = InstantiateChoice("Run for it.", true, true, dialogue6a, dialogue6b);

        dialogue4.Text = "Heh, right you are. Run along now… hey wait a minute~...";

        dialogue5a.Text = "You wink seductively at the ship hand. You grab his hand and pull him closer and then you whisper sweet endings into his ear. His face blushes a wonderful shade of red. You give him a quick peck on their neck, emotionally stunning them frozen. Now they’re distracted. You make your way down the dock. The ship hand’s stuttering was music to your ears as you made your escape from the ship.";
        dialogue5b.Text = "You wink seductively at the ship hand. You see yourself as a beautiful and delicate flower dancing in the sunlight while giggling with joy. The ship hand’s heart beats as they walk closer. You reach out a hand to them and they graciously accept. With a graceful turn you land on their arms, your eyes locked with theirs. The ship hand pulls you closer, hearts fluttering. A kiss, and another one for good measure. Claps ring around the crowd, your public display of affection has touched many hearts. That is what would have happened if you were more charismatic than a pile of poo. No, they just simply responded with 'I'm married.'";

        dialogue6a.Text = "You break into an all-out sprint. Pushing through angry and surprised crowds, you weave your escape route from the ship through the docks. Performing impressive and timely dodges over and under the cargos on the ports. The angry yells of the ship hand slowly died down as you gain significant distance from the ship you ran away from. You have successfully got off the ship without paying.";
        dialogue6b.Text = "You stared blankly at the ship hand before you shifted into a mighty sprint. The victorious feeling of getting away without paying surges through you. You are ecstatic, energized even! That is what you would have felt like if you didn’t slip on the humorously placed banana peel the moment you tried to run.";

        dialogue7.Text = "bruh";

        CurrentDialogue = dialogue1;
    }

    void Start() {
        this._view = DialogueManager.Instance.View;
        InitializeDialogue();
        //view.Degub.clicked += StartDialogue;
    }

    void StartDialogue() {
        DialogueManager.Instance.StartDialogue(this, CurrentDialogue);
    }

    void Update()
    {

    }
}
