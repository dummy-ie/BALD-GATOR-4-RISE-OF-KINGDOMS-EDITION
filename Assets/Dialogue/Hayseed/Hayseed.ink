INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "hayseed"

->VarCheck
===VarCheck===
{   
    -!hayseedCanTalkTo: ->NoTalk
    -swarmNestDefeated: ->AfterSwarm.Dialogue1 
    -else: ->Base.Dialogue1
}


===Base===

=Dialogue1
“Traveler! I am in need of dire assistance! Our fields are being ravaged by these godforsaken pests and if this continues, we’ll all die of hunger!”

    + [I’ll help you with these pests.]
        ->Dialogue2
    + [Ok, what’s in it for me?]
        ->Dialogue2
    + [Not my problem.]
        ~Leave(true)
        ->DONE


=Dialogue2
“Excellent, excellent! You go now, I’ll arrange a worthwhile reward for your troubles.”
    + [Okay]
        ~ Leave(false)
        ->DONE


->END

===AfterSwarm===

=Dialogue1
“Thank you thank you thank you! We, the farmers, owe you plenty. With the pests gone, we can hope to survive a few more winters. Here, take this. This is every farmer’s token of gratitude to you.”
    +[… Is that a diamond hoe?]
    ~IncreaseStat("STR")
    ~Leave(false)
    ->DONE

->END

===NoTalk===
Hmm?
    +[Swram]
    ~ Leave(false)
    ->DONE
->END





