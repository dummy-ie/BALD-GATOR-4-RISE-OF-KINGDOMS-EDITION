INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "sunny"

{susNunCanTalkTo: ->Character.Dialogue1 | ->NoTalk}

===Character===

=