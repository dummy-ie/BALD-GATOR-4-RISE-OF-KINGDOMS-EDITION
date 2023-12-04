INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Leave(returnable)

VAR name = "zrell"

->VarCheck
===VarCheck===
->Base

===Base===
{inquireBartender: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Halt! Identify yourself, traveler. God Enel has informed me of your presence. You don’t seem to be one of the townspeople.”
    +[I’m here to fucking destroy Enel. (STRENGTH)]



    +[We’ve come in search of arcane knowledge from God Enel. (WISDOM)]
    +[I’m here to kill you.]

    +[Leave]
        ~Leave(true)
        ->DONE

=CHACheck
Loading Dice Roll...
->CHAResult

=CHAResult
{
    -diceRoll: [Success] “You wink seductively at the ship hand. You grab his hand and pull him closer and then you whisper sweet endings into his ear. His face blushes a wonderful shade of red. You give him a quick peck on their neck, emotionally stunning them frozen. Now they’re distracted. You make your way down the dock. The ship hand’s stuttering was music to your ears as you made your escape from the ship.”
    -else: [Fail] “You wink seductively at the ship hand. You see yourself as a beautiful and delicate flower dancing in the sunlight while giggling with joy. The ship hand’s heart beats as they walk closer. You reach out a hand to them and they graciously accept. With a graceful turn you land on their arms, your eyes locked with theirs. The ship hand pulls you closer, hearts fluttering. A kiss, and another one for good measure. Claps ring around the crowd, your public display of affection has touched many hearts. That is what would have happened if you were more charismatic than a pile of poo. No, they just simply responded with ‘I’m married.’”
}
    +[Proceed]
{
    -diceRoll: 
        ~Leave(false)
        ->DONE
    - else: ->Dialogue3
}


=NoTalk
Need something?
    +[Nope]
    ~Leave(true)
    ->DONE

