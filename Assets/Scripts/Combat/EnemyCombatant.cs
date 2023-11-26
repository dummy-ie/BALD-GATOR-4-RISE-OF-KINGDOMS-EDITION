using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatant : Combatant
{   
    public override IEnumerator StartTurn()
    {
        yield return StartCoroutine(base.StartTurn());
        Debug.Log("Enemy did turn");
    }
}
