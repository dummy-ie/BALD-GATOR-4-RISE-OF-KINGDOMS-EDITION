INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL StartQuest(id)
EXTERNAL AdvanceQuest(id, index)
EXTERNAL FinishQuest(id)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "hayseed"

->VarCheck
===VarCheck===
{   
    -swarmNestDefeated: ->AfterSwarm
    -else: ->Base
}


===Base===
{hayseedCanTalkTo: ->Dialogue1 | ->NoTalk}


=Dialogue1
“Traveler! I am in need of dire assistance! Our fields are being ravaged by these godforsaken pests and if this continues, we’ll all die of hunger!”
~StartQuest("FooshaSubquest")
    + [I’ll help you with these pests.]
        ~AdvanceQuest("FooshaSubquest", -1)
        ~acceptHayseedQuest = true
        ->Dialogue2
    + [Ok, what’s in it for me?]
        ~AdvanceQuest("FooshaSubquest", -1)
        ~acceptHayseedQuest = true
        ->Dialogue2
    + [Not my problem.]
        ~Leave(true)
        ->DONE


=Dialogue2
“Excellent, excellent! You go now, I’ll arrange a worthwhile reward for your troubles.”
    + [Okay]
        ~ Leave(false)
        ->DONE


=NoTalk
"These pests are so annoying!"
    +[Bzzz]
    ~ Leave(false)
    ->DONE

->END

===AfterSwarm===
{hayseedCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Thank you thank you thank you! We, the farmers, owe you plenty. With the pests gone, we can hope to survive a few more winters. Here, take this. This is every farmer’s token of gratitude to you.” [STRENGTH INCREASED]
    +[… Is that a diamond hoe?]
    ~FinishQuest("FooshaSubquest")
    ~IncreaseStat("STR")
    ~Leave(false)
    ->DONE

=NoTalk
"Thanks for dealing with those pests!"
    +[Bzzz!]
    ~ Leave(false)
    ->DONE


->END


->END





