INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL Fight()
EXTERNAL SwitchFight(target)
EXTERNAL Kill()
EXTERNAL StartQuest(id)
EXTERNAL AdvanceQuest(id, index)
EXTERNAL FinishQuest(id)
EXTERNAL SwitchKill(target)
EXTERNAL Leave(returnable)

VAR name = "gabriel"

->VarCheck
===VarCheck===
{v1IsDead: ->DeadV1 | ->VarCheck2}
===VarCheck2===
{v1IsOn: ->PowerOnV1 | ->Base}

===Base===
~StartQuest("LobbySubquest")
{gabrielCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Machine... I will cut. You. Down. Break you apart. Splay the gore of your profane form across the stars! I will grind you down until the very sparks cry for mercy!”

    +[What’s wrong with the machine?]
        ~AdvanceQuest("LobbySubquest", -1)
        ->Dialogue2
    +[Stupid bastard.]
        ~Fight()
        ->Fighting
    +[Leave]
        ~Leave(true)
        ->DONE


=Dialogue2
“A mere object dares defy me. I have been commanding it to function, yet it does not falter!”
    +[Have you tried turning it off and on again? (INTELLIGENCE)]
        ~RollDice("INT")
        ->INTCheck
    +[Feed the machine a helping of your blood. (CONSTITUTION)]
        ~RollDice("CON")
        ->CONCheck
    +[You're just too weak.]
        ~Fight()
        ->Fighting
    +[Leave]
        ~Leave(true)
        ->DONE


=INTCheck
Loading Dice Roll...
->INTResult

=INTResult
{
    -diceRoll: [Success] “My attempts at activating it have been fruitless. Could you work your magic on it for me? I shall reward you accordingly for assisting a Holy Knight.” You must find the machine’s power switch and flick it on. 
    -else: [Fail] “Do you take me for a fool? Of course I have. I even set a romantic dinner date for it! Now move aside. I shall beat some sense into this blasphemous machine!” Gabriel starts beating the shit out of the machine.
}
    +[Proceed]
{
    -diceRoll: 
        ~turnOnV1 = true
        ~Leave(false)
        ->DONE
    - else: 
        ~AdvanceQuest("LobbySubquest", 2)
        ~SwitchFight("V1")
        ->FightingV1
}

=CONCheck
Loading Dice Roll...
->CONResult

=CONResult
{
    -diceRoll: [Success] The machine powers on with a satisfied hum. It glows with anticipation.
    -else: [Fail] The machine turns on, but is greedy for more. Kill or be killed.
}
    +[Proceed]
{
    -diceRoll: 
        ~v1IsOn = true
        ->PowerOnV1
    - else: 
        ~AdvanceQuest("LobbySubquest", 2)
        ~SwitchFight("V1")
        ->FightingV1
}


=Fighting
"Fighting in progress..."
->FightResult

=FightResult
{
    -battleWon: "Ugh... May your woes be many... And days few..."
    -else: ded
}
    +[Proceed]
{
    -battleWon: 
        ~Leave(false)
        ~gabrielIsDead = true
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}
    
=FightingV1
"Fighting in progress..."
->FightResultV1

=FightResultV1
{
    -battleWon: "01001000 01101001 00100000 01101101 01100001 00100111 01100001 01101101 00001010"
    -else: ded
}
    +[Proceed]
{
    -battleWon: 
        ~v1IsDead = true
        ~SwitchKill("V1")
        ->DeadV1
    -else: 
        ~Leave(false)
        ->DONE
}
->END

===DeadV1===
~AdvanceQuest("LobbySubquest", 3)
{gabrielCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Looks like the machine has received due justice! I thank you for your support. Please receive this gift in kind.” 
    +[Arigathanks gozaimuch. (CONSTITUTION INCREASED)]
        ~IncreaseStat("CON")
        ~Leave(false)
        ~FinishQuest("LobbySubquest")
        ->DONE



===PowerOnV1===
{gabrielCanTalkTo: ->Dialogue1 | ->NoTalk}

=Dialogue1
“Wonderful! That worthless machine has resumed its function. You are truly blessed in the eyes of God. In that, I thank you. Please receive this gift before we part.”
    +[Selamat pagi! (CONSTITUTION INCREASED)]
        ~IncreaseStat("CON")
        ~Leave(false)
        ~FinishQuest("LobbySubquest")
        ->DONE

===NoTalk===
Machine...
    +[Maybe I should turn back now?]
        ~Leave(true)
        ->DONE
    +[Die!!!]
        ~Fight()
        ->Fighting

=Fighting
"Fighting in progress..."
->FightResult

=FightResult
~FinishQuest("LobbySubquest")
{
    -battleWon: "Ugh... May your woes be many... And days few..."
    -else: ded
}
    +[Proceed]
{
    -battleWon: 
        ~Leave(false)
        ~gabrielIsDead = true
        ~AdvanceQuest("MainQuest2", -1)
        ~Kill()
        ->DONE
    -else: 
        ~Leave(false)
        ->DONE
}