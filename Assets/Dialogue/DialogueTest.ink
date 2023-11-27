INCLUDE Database.ink

EXTERNAL RollDice(stat)

VAR name = "totoy"

->Character.Dialogue1
===Character===

=Dialogue1
Hello, this is dialogue 1!
    + [Move to dialogue 2]
        ->Dialogue2
    + [Move to dialogue 3]
        ->Dialogue3


=Dialogue2
Heyo, this is dialogue 2!
    + [Okay]
        ->Dialogue1



=Dialogue3
Wassup, this is dialogue 3!
    + [Okay]
    * {!totoyHasRolled}[Roll]
        ~ RollDice("CHA")
        ~ totoyHasRolled = true
        ->Dialogue3Check
        
-->Dialogue1

=Dialogue3Check
Loading Dice Roll...
->Dialogue3Result

=Dialogue3Result
{diceRoll: Sucess!! You will now be moved to Dialogue 4|Fail :< You will now be moved back to Dialogue 1}
        +[Okay]
{diceRoll: ->Dialogue4|->Dialogue1}


=Dialogue4
Oya, this is dialogue 4!
    + [Cool]
        ->Dialogue1

->END


===function RollDice(stat)===
Error




