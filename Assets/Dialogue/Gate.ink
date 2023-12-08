INCLUDE Database.ink

EXTERNAL RollDice(stat)
EXTERNAL IncreaseStat(stat)
EXTERNAL StartQuest(id)
EXTERNAL AdvanceQuest(id, index)
EXTERNAL FinishQuest(id)
EXTERNAL Fight()
EXTERNAL Kill()
EXTERNAL MoveScene(targetScene)
EXTERNAL Leave(returnable)

VAR name = "gate"

->VarCheck
===VarCheck===
->Base

===Base===
->Dialogue1

=Dialogue1
The gate shines brightly for you. You sense that the Moon God Enel is awaiting your visit. (Mark of Justice Required)
    +{markOfJustice}[Enter the Gate.]
        ~MoveScene("SceneChanger name here")
        ~FinishQuest("MainQuest2")
        ~StartQuest("MainQuest3")
        ~AdvanceQuest("MainQuest3", -1)
        ~Leave(true)
        ->DONE
    +[Turn back.]
        ~Leave(true)
        ->DONE


===NoTalk===
The gate does not let you pass
    +[...]
    ~Leave(true)
    ->DONE

->END