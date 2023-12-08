INCLUDE Database.ink

EXTERNAL IncreaseStat(stat)
EXTERNAL StartQuest(id)
EXTERNAL FinishQuest(id)
EXTERNAL Leave(returnable)

VAR name = "sunny"

->VarCheck
===VarCheck===
{maidRescued: ->MaidRescued | ->Base}

===Base===
{sunnyCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1
~StartQuest("SyrupSubquest")
“Greetings, it’s rare to see a traveler around these parts. Would you mind hearing a request of mine? My child has not come home in a while. They work at the Mansion close by. Would you please check it out?”

    +[I'd love to!] "Thank you! I will have a reward prepared for you."
        ++[Proceed]
            ~ acceptMaidQuest = true
            ~Leave(false)
            ->DONE
    +[What reward will I get?] "I have some herbs that will be useful in your journey."
        ++[I see]
            ->Dialogue1
    +[Haha no.]
        ~Leave(true)
        ->DONE

=NoTalk
{acceptMaidQuest: "Please find my child!" | "Hello!"}
    +[Bye]
        ~Leave(true)
        ->DONE

->END

===MaidRescued===

=Dialogue1
"Thank you for bringing back my child!"
    *[Reward please?] (INTELLIGENCE INCREASED)
        ++[Yummy herbs]
            ~IncreaseStat("CON")
            ~Leave(true)
            ->DONE
    +["No problem."]
        ~Leave(true)
        ->DONE
            
    
