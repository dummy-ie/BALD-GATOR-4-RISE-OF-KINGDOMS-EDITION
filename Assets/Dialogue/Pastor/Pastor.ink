INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "pastor"

{pastorCanTalkTo: ->Character.Dialogue1 | ->NoTalk}
===Character===

=Dialogue1
"Oh? A wayward soul. How can I help you?"

    + [Can you tell me about the rumor?]
        ->Dialogue2

    + [Give me your bible!]
        ->Dialogue3
    
=Dialogue2
“Hmm… I sometimes see one of our nuns acting very strange lately. Can you do me a favor and check on her."
    + [Sure]
    ~ pastorTalkedTo = true
    ->NoTalk

=Dialogue3
"I'll go see if I have a spare, son"
    +[You get on that]
    ~Leave(true)
    ->DONE

->END

===NoTalk===
May the moon be with you
    +[...mhm]
    ~ Leave(false)
    ->DONE
->END

===function RollDice(stat)===
Error

===function StartQuest(id)===
Error
