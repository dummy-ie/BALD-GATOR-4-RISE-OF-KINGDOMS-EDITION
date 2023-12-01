INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "nonSusNun"

{nonSusNunCanTalkTo: ->Character.Dialogue1 | ->NoTalk}
===Character===

=Dialogue1
“Oh my moons, it’s been a while since we’ve had a newcomer to our chapel. How may I help you, o’ young one?”
    + [Can you tell me about the rumor?]
        ->Dialogue2

    + [Spill the beans!]
        ->Dialogue4
    
=Dialogue2
“Oh dear me, I happen to have no idea where it came from. I was so distraught to hear that people think it originated from within these sacred grounds. For shame!”
    + [Yeah yeah now where else can I ask?]
        ->Dialogue3

=Dialogue3
"You can try asking the pastor. He is most aware when it comes to the church grounds."
    +[Alright]
    ->NoTalk

=Dialogue4
"...What?"
    +[...]
    ~Leave(true)
    ->DONE

->END

===NoTalk===
May the moon be with you always
    +[...ok]
    ~ Leave(false)
    ->DONE
->END

===function RollDice(stat)===
Error

===function StartQuest(id)===
Error



