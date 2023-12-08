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

VAR name = "merry"

->VarCheck
===VarCheck===
->Base

===Base===
{merryCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
“Who are you? Are you another one of his goons, here to beat me up again?”
    *[You look like the guy upstairs.] "He's a fake! He kept me locked down here for ages!"
        ++[Oh damn]
            ->Dialogue1
    +["Do you know where the maid is?"] "She's at the cell, please help her escape this damned mansion!"
        ++[Noted]
            ~Leave(false)
            ->DONE


=NoTalk
"Pain..."
    +[Sucks to suck]
    ~Leave(true)
    ->DONE

->END