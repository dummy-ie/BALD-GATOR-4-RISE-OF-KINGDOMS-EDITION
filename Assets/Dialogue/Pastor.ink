INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "pastor"

->VarCheck
===VarCheck===
->Base


===Base===
{acceptChapelQuest: ->Dialogue1 | ->NoTalk }

=Dialogue1
"Oh? A wayward soul. How can I help you?"

    + [Can you tell me about the rumor?]
        ->Dialogue2

    + [Give me your bible!]
        ->Dialogue3
    
=Dialogue2
“Hmm… I sometimes see one of our nuns acting very strange lately. Can you do me a favor and check on her."
    + [Sure]
        ~ inquiredPastor = true
        ->NoTalk
        ->DONE

=Dialogue3
"I'll go see if I have a spare, son"
    +[You get on that]
    ~Leave(true)
    ->DONE

=NoTalk
May the moon be with you
    +[...mhm]
    ~ Leave(true)
    ->DONE
->END



