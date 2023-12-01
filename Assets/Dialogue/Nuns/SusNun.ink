INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "susNun"

{susNunCanTalkTo: ->Character.Dialogue1 | ->NoTalk}

===Character===



=Dialogue1
Who goes there!? Ah, it's just a wanderer… What do you need?
    +[Um, Nothing]
        ~Leave(true)
        ->DONE
    * {pastorTalkedTo} [Inspect nun (INTELLIGENCE)]
        ~RollDice("INT")
        ->INTCheck
    * {!pastorTalkedTo} [No. What do YOU need? (CHARISMA)]
        ~RollDice("CHA")
        ->CHACheck
        
=INTCheck
Loading Dice Roll...
->INTResult

=INTResult
{
    -diceRoll: [Success] "Well well well… I never thought I’d get caught. But if you think I’m letting you out alive then you're dead wrong."
    -else: [Fail] "Uh Huh… If you have nothing else to add then I’m busy. Toodeloo!" 
}
        +[Proceed]
{
    -diceRoll: 
        ~Fight()
    - else: 
        ~Leave(false)
        ->DONE
}

=CHACheck
Loading Dice Roll...
->CHAResult

=CHAResult
{
    -diceRoll: [Success] "Hmm… Well you can help me spread these “ashes” around town. Don’t ask, it's a religion thing"
    -else: [Fail] "What I need is to get going. Now out of my way!" 
}
        +[Proceed]
{
    -diceRoll: 
        ~susNunHelp = true
        ~Leave(false)
        ->DONE
    - else: 
        ~Leave(false)
        ->DONE
}


->END

===NoTalk===
...
    +[...]
    ~ Leave(false)
    ->DONE
->END

===function RollDice(stat)===
Error

===function StartQuest(id)===
Error
