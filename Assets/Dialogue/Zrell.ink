INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL AdvanceQuest(id, index)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL Leave(returnable)

VAR name = "zrell"

->VarCheck
===VarCheck===
->Base

===Base===
{inquireBartender: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Halt! Identify yourself, traveler. God Enel has informed me of your presence. You don’t seem to be one of the townspeople.”
    +[I’m here to fucking destroy Enel. (STRENGTH)]
        ~ RollDice("STR")
        ->STRCheck
    +[We’ve come in search of arcane knowledge from God Enel. (WISDOM)]
        ~ RollDice("WIS")
        ->WISCheck
    +[I’m here to kill you.]
        ~Fight()
        ->Fighting
    +[Leave]
        ~Leave(true)
        ->DONE

=STRCheck
Loading Dice Roll...
->STRResult

=STRResult
{
    -diceRoll: [Success] “Hah. What you lack in caution you look to compensate with power. God Enel will happily display his immense might over a mere mortal.” Receive “Mark of Justice”, allowing passage to the gate.
    -else: [Fail] “Hah. Your blasphemous insult won’t get you far. You are not worthy to be God Enel’s opponent. I will strike you down myself.”
}
    +[Proceed]
{
    -diceRoll: 
        ~markOfJustice = true
        ~AdvanceQuest("MainQuest2", 2)
        ~Leave(false)
        ->DONE
    - else: 
        ~Fight()
        ->Fighting
}

=WISCheck
Loading Dice Roll...
->STRResult

=WISResult
{
    -diceRoll: [Success] “Very well. God Enel has expressed intrigue for your party. Be on your way to the gate.” Receive “Mark of Justice”, allowing passage to the gate.

    -else: [Fail] “Your lot don’t look like the scholarly type. God Enel doesn’t grant knowledge to filthy liars. Die!”

}
    +[Proceed]
{
    -diceRoll: 
        ~markOfJustice = true
        ~AdvanceQuest("MainQuest2", 2)
        ~Leave(false)
        ->DONE
    - else: 
        ~Fight()
        ->Fighting
}


=Fighting
"Fighting in progress..."
->FightResult

=FightResult
{
    -battleWon: "I-Impossible..."
    -else: ded
}
    +[Proceed]
{
    -battleWon: 
        ~Leave(false)
        ~AdvanceQuest("MainQuest2", -1)
        ~zrellIsDead = true
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}
    
=NoTalk
Need something?
    +[Nope]
    ~Leave(true)
    ->DONE

