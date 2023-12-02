INCLUDE ../Database.ink

EXTERNAL Leave(returnable)

VAR name = "bartender"

->VarCheck

===VarCheck===
{
    -!bartenderCanTalkTo: ->NoTalk
    -else: ->Base.Dialogue1
}

===Base===

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
~inquireBartender = true
~Leave(true)
->DONE

=Dialogue3
“One blue horse beer coming right up!”
    + [Blue horse extra cool! Ito ang lamig!]
~Leave(true)
->DONE

->END

===NoTalk===
Want a drink?
    +[sure]
    ~ Leave(false)
    ->DONE
->END





