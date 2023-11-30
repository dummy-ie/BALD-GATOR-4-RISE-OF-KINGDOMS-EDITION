INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "swarmNest"

{swarmNestCanTalkTo: ->Character.Dialogue1 | ->NoTalk}
===Character===

=Dialogue1
“Few feet away from you, you spot an odd structure that appears to be made of mud and some other unidentifiable material. You hear sounds of shuffling and squeaking coming from the holes on the structure. You quickly realize that this is the nest of the swarm, the source of the pest problem you’re tasked to deal with.”

    + [Destroy it stealthily. (DEXTERITY)]
        ~ RollDice("DEX")
        ->DEXCheck
    + [Destroy it.]
        ->Dialogue2


=Dialogue2
You approach the nest, intending to smash it into pieces. However, your not-so subtle approach roused the swarm within the nest into action. You have woken the swarm.
    + [Damn]
        ~Fight()
        ->DONE

=DEXCheck
Loading Dice Roll...
->DEXResult

=DEXResult
{
    -diceRoll: [Success] “You slowly approach the nest, being careful to not alert the swarm. Now you are standing right next to it, the sound of the swarm within now more audible than ever. In one swift action, you smash the entire nest. A successful extermination!”
    -else: [Fail] “You slowly approach the nest, being careful to not alert the swarm. But alas! You slipped on a banana peel and fell on the ground. With a loud thud, the swarm is alerted.”
}
    +[Proceed]
{
    -diceRoll: 
        ~hayseedCanTalkTo = true 
        ~swarmNestDefeated = true 
        ~Leave(false)
        
    -else: Fight()
}

->END

===NoTalk===
Bzzz
    +[Bzzz]
    ~Leave(false)
    ->DONE

->END


===function RollDice(stat)===
Error

===function StartQuest(id)===
Error


