INCLUDE Database.ink


EXTERNAL Leave(returnable)

VAR name = "sunny"

->VarCheck
===VarCheck===
->Base

===Base===
{sunnyCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
“Greetings, it’s rare to see a traveler around these parts. Would you mind hearing a request of mine? My child has not come home in a while. They work at the Mansion close by. Would you please check it out?”

    +[I'd love to!]
        ~ acceptMaidQuest = true
        ->Dialogue2
    +[Haha no.]
        ~Leave(true)
        ->DONE

=Dialogue2
"Oh thankyou so much! I promise I'll make it up for the trouble!"
    +[You better]
    ~Leave(false)
    ->DONE

=NoTalk
{acceptMaidQuest: "Please find my child!" | "Hello!"}
    +[Bye]
        ~Leave(false)
        ->DONE

->END