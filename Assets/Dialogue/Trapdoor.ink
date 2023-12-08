INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL StartQuest(id)
EXTERNAL AdvanceQuest(id, index)
EXTERNAL AddCombatant(name)
EXTERNAL FinishQuest(id)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL MoveScene(targetScene)
EXTERNAL SwitchKill(target)
EXTERNAL Leave(returnable)

VAR name = "trapdoor"

->VarCheck
===VarCheck===
->Base

===Base===
~AdvanceQuest("SyrupSubquest", -1)
{trapdoorCanTalkTo: ->Dialogue1 | ->NoTalk }

=Dialogue1

This trapdoor seems to lead somewhere deeper
    +{!trapdoorIsOpen}[Break the trapdoor (STRENGTH)] 
        ~RollDice("STR")
        ->STRCheck
    
    +{keyObtained}[UseKey]
        ->Dialogue2
    +{trapdoorIsOpen}[Enter trapdoor]
        ~AdvanceQuest("SyrupSubquest", -1)
        ~MoveScene("connection")
        ->DONE
    +[Leave]
        ~Leave(true)
        ->DONE

=Dialogue2
You unlock the trapdoor with the key   
    +[Cool]
        ~trapdoorIsOpen = true
        ~keyObtained = false
        ->Dialogue1

=STRCheck
Loading Dice Roll...
+[Please Wait...]
->STRResult

=STRResult
{
    -diceRoll: [Success] “You break the trapdoor successfully, and somehow quietly!"
    -else: [Fail] “You try to break the trapdoor, but it’s very sturdy. It seems you have to find another way to open the trapdoor”
}
    +[Proceed]
{
    -diceRoll: 
        ~trapdoorIsOpen = true
        ~keyObtained = false
        ->Dialogue1
    - else: 
        ->Dialogue1
}


=NoTalk
"Heh."
    +[Weirdo]
    ~Leave(true)
    ->DONE

->END