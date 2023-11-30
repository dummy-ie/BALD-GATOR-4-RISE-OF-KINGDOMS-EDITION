using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : Combatant
{   
    public override IEnumerator StartTurn()
    {
        yield return StartCoroutine(base.StartTurn());
        Debug.Log("Player did turn");
    }
}
