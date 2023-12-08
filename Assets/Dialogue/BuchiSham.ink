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

VAR name = "buchisham"

->VarCheck
===VarCheck===
{canEnterMansion: ->CanEnter | ->Base }

===Base===
{acceptMaidQuest: ->Dialogue1 | ->NoTalk }

=Dialogue1
"Oi! You are not allowed to be in here."
    +[Do you know if a maid works here?] "And who are you to be asking that? Of course not, we have not seen any maid working here!"
        ++{!buchishamCheckCHA}[I am with the IRS, she has some taxes to be paid and my information says she's here. (CHARISMA)]
            ~buchishamHasRolledCHA = true
            ~RollDice("CHA")
            ->CHACheck
        ++{!buchishamCheckDEX}[Make a distraction (DEXTERITY)]
            ~buchishamHasRolledDEX = true
            ~RollDice("DEX")
            ->DEXCheck

    +[Who are you?] "We are the Nyaban Brothers!"
        ++[I see]
            ->Dialogue1

    +["Sorry buds, but I gotta get in"]
        ~AddCombatant("Sham")
        ~Fight()
        ->Fighting
    +[Kayy]
        ~Leave(true)
        ->DONE

=CHACheck
Loading Dice Roll...
->CHAResult

=CHAResult
{
    -diceRoll: [Success] "Oh no it's the IRS! You are free to come and go as you please!" (now u can go to mansion)
    -else: [Fail] "Ha! Do you think we'd believe that lie?" 
}
    +[Proceed]
{
    -diceRoll: 
        ~canEnterMansion = true
        ~Leave(false)
        ->DONE
    - else: 
        ~AddCombatant("Sham")
        ~Fight()
        ->Fighting
}

=DEXCheck
Loading Dice Roll...
->DEXResult

=DEXResult
{
    -diceRoll: [Success] "Did you hear something?" (now u can go to mansion)
    -else: [Fail] Why are you just standing there weirdly, shoo!"
}
    +[Proceed]
{
    -diceRoll: 
        ~canEnterMansion = true
        ~Leave(false)
        ->DONE
    - else: 
        ~Leave(true)
        ->DONE
}



=Fighting
"Fighting in progress..."
->FightResult

=FightResult
{
    -battleWon: Qwie... Ugh...
    -else: ded
}
    +[Proceed]
{
    -battleWon: 
        ~canEnterMansion = true
        ~Leave(false)
        ~Kill()
        ~SwitchKill("Sham")
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}
    

=NoTalk
"You cant enter this mansion"
    +[Bye]
        ~Leave(true)
        ->DONE

->END



===CanEnter===
"Oh don't mind us :3"
    +[Bye]
        ~Leave(true)
        ->DONE
->END
