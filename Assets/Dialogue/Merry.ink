INCLUDE Database.ink

EXTERNAL Leave(returnable)

VAR name = "merry"

->VarCheck
===VarCheck===
->Base

===Base===
{merryCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
“Oho! Welcome stranger, you seem weary from your travels. Would you like to come in and stay for the night?”
    +[A nice bed to stay the night, I am in!] 
        ~Leave(false)
        ->DONE
    +[That seems like a suspicious request.]
        
    +[I’m just passing through.]
        ~Leave(true)
        ->DONE
=NoTalk
What to do...
+[Bye]
        ~Leave(false)
        ->DONE

->END