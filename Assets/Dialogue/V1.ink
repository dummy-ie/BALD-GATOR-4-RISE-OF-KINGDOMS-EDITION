INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL AdvanceQuest(id, index)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL Leave(returnable)

VAR name = "v1"

->VarCheck
===VarCheck===
{turnOnV1: ->Base | ->NoTalk}

===Base===
{v1CanTalkTo: ->Dialogue1| ->NoTalk}

=Dialogue1
You look at the machine. Fortunately, there's a big power button at it's back.
    +[Press the power button]
        ->Dialogue2

=Dialogue2
~AdvanceQuest("LobbySubquest", 3)
The machine turns on and starts spewing out 1s and 0s. You don't know what they mean
    +[Proceed]
        ~turnOnV1 = false
        ~v1IsOn = true
        ~gabrielCanTalkTo = true
        ~Leave(false)
        ->DONE


===NoTalk===
...
    +[...]
    ~Leave(true)
    ->DONE

->END