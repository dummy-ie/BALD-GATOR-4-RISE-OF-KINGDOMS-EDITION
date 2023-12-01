INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "townGuy"

{susNunHelp: ->Character2.Pre | ->Character.Pre}
===Character===

=Pre
{townGuyCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Hello random stranger! What brings you to our town?”

    + [What's with all the chatter?]
        ->Dialogue2
    + [Shut ur sorry ass up.]
        ~Leave(true)
        ->DONE


=Dialogue2
“There seems to be a rumor going around about how the Moon God Enel will drown the island in its tides. We have no clue who started this rumor but the whole town is petrified”
    + [Then drown ig]
        ~ Leave(true)
        ->DONE
    +[Where else can I ask about this?]
        //~StartQuest(id)
        ->Dialogue3

=Dialogue3
"You can try the chapel up north."
    +[Ayt Bet]
    ~Leave(false)
    ->DONE
    +[Seeya]
    ~Leave(false)
    ->DONE

->END


===Character2===

=Pre
{townGuyCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
You see the villagers around. Time for a little ashfall
    +[Spread the ashes]
    ~townGuyAshed = true
    //FinishQuest(id)
    ->Dialogue2

=Dialogue2
"UGH WHAT IS THIS!?"
    +[Make your escape]
    ~Leave(false)
    ->DONE
->END

===NoTalk===
What's up?
    +[Hey]
    ~ Leave(false)
    ->DONE
->END

===function RollDice(stat)===
Error

===function StartQuest(id)===
Error



