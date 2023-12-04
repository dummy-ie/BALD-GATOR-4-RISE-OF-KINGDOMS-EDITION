INCLUDE Database.ink

EXTERNAL Leave(returnable)

VAR name = "townGuy"

->VarCheck

===VarCheck===
{
    -helpTheSusNun: ->HelpingTheNun
    -else: ->Base 
}

===Base===
{townGuyCanTalkTo: ->Dialogue1 | ->NoTalk }

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
    ~acceptChapelQuest = true
    ~susNunCanTalkTo = true
    ~Leave(false)
    ->DONE

=NoTalk
"The prophecy is scary!""
    +[Coward]
    ~ Leave(false)
    ->DONE

->END


===HelpingTheNun===
{townGuyCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
You see the villagers around. Time for a little ashfall
    +[Spread the ashes]
    ~ashesHasBeenSpread = true
    //FinishQuest(id)
    ->Dialogue2

=Dialogue2
"UGH WHAT IS THIS!?"
    +[Make your escape]
    ~Leave(false)
    ->DONE

=NoTalk
"DUDE WTH??"
    +[Heh]
    ~Leave(false)
    ->DONE
->END






