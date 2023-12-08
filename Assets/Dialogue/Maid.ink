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

VAR name = "maid"

->VarCheck
===VarCheck===
->Base

===Base===
{maidCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
"cough... Please help me..."
    +[It's ok, I am here to help] "Thank you..."
        ++[No problem]
            ~maidRescued = true
            ~Leave(false)
            ->DONE
    +[Sunny told me you'd be here] "Please let me go back to her..."
        ++[Don't worry]
            ~maidRescued = true
            ~Leave(false)
            ->DONE


=NoTalk
"Ouch..."
    +[Damn]
    ~Leave(true)
    ->DONE

->END