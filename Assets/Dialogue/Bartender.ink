﻿INCLUDE Database.ink

EXTERNAL Leave(returnable)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)

VAR name = "bartender"

->VarCheck

===VarCheck===
->Base

===Base===

{bartenderCanTalkTo: ->Dialogue1 | ->NoTalk}

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
~FinishQuest("MainQuest1")
~StartQuest("MainQuest2")
~inquireBartender = true
~Leave(true)
->DONE

=Dialogue3
“One blue horse beer coming right up!”
    + [Blue horse extra cool! Ito ang lamig!]
~Leave(true)
->DONE

=NoTalk
"Want a drink?"
    +[sure]
    ~ Leave(false)
    ->DONE

->END




