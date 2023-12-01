INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "bartender"

->Character.Dialogue1
===Character===

=Dialogue1
“Well-met traveler. You look weary, perhaps you would like some of our blue horse beer?”
    + [I am here to ask about the One Piece.]
        ->Dialogue2
    + [Sure. Give me a drink, bartender.]
        ->Dialogue3
    + [I’m not thirsty.]
        ~Leave(true)
        ->DONE


=Dialogue2
“Hmm. Yes. The One Piece, legendary treasure yes? Very well then. You lot would be seeking God Enel. They lie behind the Gate of Justice. Unfortunately you would need to unlock it. Head down the road. There lies a tower right before the gate, perhaps you’d find some way of unlocking the gate at the Celestial Ascent tower.”
    + ['Kay]
~StartQuest("MainQuest1")
~Leave(true)
->DONE

=Dialogue3
“One blue horse beer coming right up!”
    + [Blue horse extra cool! Ito ang lamig!]
~Leave(true)
->DONE

->END


===function RollDice(stat)===
Error

===function StartQuest(id)===
Error

===function Fight()===
Error

===function Leave(returnable)===
Error




