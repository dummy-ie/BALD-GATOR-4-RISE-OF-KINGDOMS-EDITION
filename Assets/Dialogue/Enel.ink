INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL Betrayal()
EXTERNAL AdvanceQuest(id, index)
EXTERNAL FinishQuest(id)
EXTERNAL MoveScene(targetScene)
EXTERNAL Leave(returnable)

VAR name = "enel"

->VarCheck
===VarCheck===
{markOfJustice: ->Base | ->NoTalk}

===Base===
->Dialogue1

=Dialogue1
“Weakling mortals, state your purpose.”
    +[We want to know where the One Piece is.]  “I will give you the power to obtain the One Piece, but you must give to me your companions’ souls in exchange. After all, true power lies in the self. Your “friends” only serve to hinder your growth. Now, choose.”
        ++[I’ll do it. I will sacrifice them.]
            ->Dialogue2A
        ++[Never! They are my precious nakama. I’ll never abandon them for the likes of you!]
            ->Dialogue2B
        ++[I changed my mind. I don’t need the One Piece after all.]
            ->Dialogue2C

    +[Who are you?] “You arrive here and do not know of me? I am God Enel of Skypiea. I am known to be impatient. Get to the point.”
        ++[Okay Then]
            ->Dialogue1
    +[Where is this?] “This paradise is Fairy Vearth. I have claimed this land of bountiful power for myself, eons ago after I fought with a powerful pirate.”
        ++[I see...]
            ->Dialogue1
    +[We’re here to kill you, and end your tyranny.] “Another challenger, I see. I will show you just how foolish you are.”
        ++[So be it]
            ->Dialogue2B
    +[Try to leave] “So you turn to cowardice. You will not leave here alive, foolish weakling.”
        ++[No wait-]
            ->Dialogue2C




    

=Dialogue2A
With promises of endless power, who needs friends? In exchange for power, you killed your friends, and eventually Enel as well. You are now all powerful, as the new God of Merry Island.
~AdvanceQuest("MainQuest3", 2)
    +[I NEED MORE POWER!!]
        ~Fight()
        ~Betrayal()
        ->FightEnelEvil

=Dialogue2B
Your journey for the One Piece is meaningless without your companions. You chose to fight back and kill the false god Enel, ending his reign of tyranny.
~AdvanceQuest("MainQuest3", 4)
    +[For the One Piece!]
        ~Fight()
        ->FightEnelHeroic


=Dialogue2C
Hesitating between fighting Enel or murdering your companions, Enel grows tired and simply tries to kill you all. Defend yourself or perish like the rest.
~AdvanceQuest("MainQuest3", 6)
    +[To the death?!]
        ~Fight()
        ->FightEnelNormal


=FightEnelHeroic
"Fighting in progress..."
+[Please Wait...]
->FightEnelHeroicResult

=FightEnelHeroicResult
{
    -battleWon: With Enel’s power waning, you manage to return back through the Gate of Justice, forever sealing away the evil that once plagued Merry Island. The citizens rejoice in your heroic act, and history will remember your party as the ones to end the terror of Enel’s judgment.    
    -else: ded
}
~AdvanceQuest("MainQuest3", -1)
    +[Proceed]
{
    -battleWon: 
        ~MoveScene("To enies lobby")
        ~Leave(false)
        heroicEnding = true
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}

=FightEnelNormal
"Fighting in progress..."
+[Please Wait...]
->FightEnelNormalResult

=FightEnelNormalResult
{
    -battleWon: With Enel sufficiently weakened, now is your chance to sever his ties with the Earth below and end his tyranny. Unfortunately with no other portal back, you end up trapped alongside Enel on the moon. Your sacrifice will be forever celebrated by the citizens of Merry Island.
    -else: ded
}
~AdvanceQuest("MainQuest3", -1)
    +[Proceed]
{
    -battleWon: 
        ~Leave(false)
        normalEnding = true
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}

=FightEnelEvil
"Fighting in progress..."
~Betrayal()
+[Please Wait...]
->FightEnelEvilResult

=FightEnelEvilResult
{
    -battleWon: They thought they would be finally free of Enel’s influence. Turns out, the people of Merry Island are simply under new management. Mete out your justice as you see fit, and rule with an iron fist that smites down unbelievers from the heavens.

    -else: ded
}
~AdvanceQuest("MainQuest3", -1)
    +[Proceed]
{
    -battleWon: 
        ~Leave(false)
        evilEnding = true
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}

===NoTalk===
The figure does not move or recognize your prescence. Might you need a mark perhaps?
    +[Slowly back away]
    ~Leave(true)
    ->DONE