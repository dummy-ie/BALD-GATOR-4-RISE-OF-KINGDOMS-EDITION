INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL AdvanceQuest(id, index)
EXTERNAL FinishQuest(id)
EXTERNAL Leave(returnable)

VAR name = "susNun"

->VarCheck
===VarCheck===
->Base


===Base===
{susNunCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
Who goes there!? Ah, it's just a wanderer… What do you need?
    +[Um, Nothing]
        ~Leave(true)
        ->DONE
    * {inquiredPastor} [Inspect nun (INTELLIGENCE)]
        ~RollDice("INT")
        ->INTCheck
    * {!inquiredPastor} [No. What do YOU need? (CHARISMA)]
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
        ~AdvanceQuest("RogueSubquest", 2)
        ~Fight()
        ->Fighting
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
        ~AdvanceQuest("RogueSubquest", 3)
        ~helpTheSusNun = true
        ~townGuyCanTalkTo = true
        ~Leave(false)
        ->DONE
    - else: 
        ~Leave(false)
        ->DONE
}

=Fighting
"Fighting in progress..."
->FightResult

=FightResult
{
    -battleWon: "Ugh... I'm not... finished with the mission..." 
    -else: ded
}
    +[Proceed]
{
    -battleWon: ->FightResult2
    -else: 
        ~Leave(false)
        ->DONE
}


=FightResult2
You notice the nun was holding some sort of relic. You pick it up. [WISDOM INCREASED]
    +[Proceed]
        ~FinishQuest("RogueSubquest")
        ~Leave(false)
        ~IncreaseStat("WIS")
        ~Kill()
        ->DONE


=NoTalk
...
    +[...]
    ~ Leave(false)
    ->DONE
->END

