INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "townGuy"

{townGuyCanTalkTo: ->Character.Dialogue1 | ->NoTalk}
===Character===

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



