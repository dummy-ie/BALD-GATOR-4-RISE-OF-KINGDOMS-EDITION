INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL StartQuest(id)
EXTERNAL AddCombatant(name)
EXTERNAL FinishQuest(id)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL SwitchKill(target)
EXTERNAL Leave(returnable)

VAR name = "susmerry"

->VarCheck
===VarCheck===
->Base

===Base===
{susmerryCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
“Oho! Welcome stranger, what brings you to this wonderful mansion? Hopefully my colleagues outside did not give you any sort of trouble?"
    *[They were a wonderful bunch!] "I hope they entertained you as much as you will entertain me oho!"
        ++[Haha yeah...]
            ->Dialogue1
    
    +[Steal Key (DEXTERITY)] 
        ~RollDice("DEX")
        ->DEXCheck
    
    +[I feel like fighting!]
        ~Fight()
        ->Fighting
    +[Leave]
        ~Leave(true)
        ->DONE

=DEXCheck
Loading Dice Roll...
+[Please Wait...]
->DEXResult

=DEXResult
{
    -diceRoll: [Success] You now have a sturdy looking key.
    -else: [Fail] "We just met and you already plan to pickpocket me? I will make you regret that decision"
}
    +[Proceed]
{
    -diceRoll: 
        ~keyObtained = true
        ~Leave(false)
        ->DONE
    - else: 
        ~Fight()
        ->Fighting
}

=Fighting
"Fighting in progress..."
+[Please Wait...]
->FightResult

=FightResult
{
    -battleWon: "Ugh! Curses!"
    -else: ded
}
    +[Proceed]
{
    -battleWon: 
        ~keyObtained = true
        ~Leave(false)
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}
    


=NoTalk
"Heh."
    +[Weirdo]
    ~Leave(true)
    ->DONE

->END