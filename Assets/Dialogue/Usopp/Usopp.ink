INCLUDE ../Database.ink

EXTERNAL RollDice(stat)
EXTERNAL StartQuest(id)
EXTERNAL Fight()
EXTERNAL Leave()


VAR name = "usopp"

->Character.Dialogue1
===Character===

=Dialogue1
“Welcome to Merry Island, traveler.”
    + [What’s going on around here?]
        ->Dialogue2
    + [Thanks! I’m going now.]
        ->Dialogue3
    + [I'll kill you!]
        ~Fight()
        ->DONE


=Dialogue2
“Some ‘God’ decided to… well, play god with the inhabitants of the island. Not in a good way at that. The people are suffering under their rule.”

    + [Thanks! I'll going now.]
        ->Dialogue3
    + [Thanks. I’ll kill you.]
        ~Fight()
        ->DONE



=Dialogue3
“Nuh-uh-uh! Hold on a minute. You gotta pay.”

    + [*Give them a bag of rocks*]
        ->Dialogue4
    * {!usoppHasRolledCHA}[Seduce (CHARISMA)]
        ~ RollDice("CHA")
        ->CHACheck
    * {!usoppHasRolledDEX}[Run for it (DEXTERITY)]
        ~ RollDice("DEX")
        ->CHACheck
        
-->Dialogue1

=CHACheck
Loading Dice Roll...
->CHAResult

=CHAResult
{
    -diceRoll: [Success] “You wink seductively at the ship hand. You grab his hand and pull him closer and then you whisper sweet endings into his ear. His face blushes a wonderful shade of red. You give him a quick peck on their neck, emotionally stunning them frozen. Now they’re distracted. You make your way down the dock. The ship hand’s stuttering was music to your ears as you made your escape from the ship.”
    -else: [Fail] “You wink seductively at the ship hand. You see yourself as a beautiful and delicate flower dancing in the sunlight while giggling with joy. The ship hand’s heart beats as they walk closer. You reach out a hand to them and they graciously accept. With a graceful turn you land on their arms, your eyes locked with theirs. The ship hand pulls you closer, hearts fluttering. A kiss, and another one for good measure. Claps ring around the crowd, your public display of affection has touched many hearts. That is what would have happened if you were more charismatic than a pile of poo. No, they just simply responded with ‘I’m married.’”
}
        +[Proceed]
{diceRoll: Leave()|->Dialogue3}

=DEXCheck
Loading Dice Roll...
->DEXResult

=DEXResult
{
    -diceRoll: [Success] “You break into an all-out sprint. Pushing through angry and surprised crowds, you weave your escape route from the ship through the docks. Performing impressive and timely dodges over and under the cargos on the ports. The angry yells of the ship hand slowly died down as you gain significant distance from the ship you ran away from. You have successfully got off the ship without paying.”
    -else: [Fail] “You stared blankly at the ship hand before you shifted into a mighty sprint. The victorious feeling of getting away without paying surges through you. You are ecstatic, energized even! That is what you would have felt like if you didn’t slip on the humorously placed banana peel the moment you tried to run.”
}
    +[Proceed]
{diceRoll: Leave()|->Dialogue3}

=Dialogue4
“Heh, right you are. Run along now… hey wait a minute~...”
    + [*Sucker*]
        ~Leave()
        ->DONE
->END


===function RollDice(stat)===
Error

===function StartQuest(id)===
Error

===function Fight()===
Error

===function Leave()===
Error


