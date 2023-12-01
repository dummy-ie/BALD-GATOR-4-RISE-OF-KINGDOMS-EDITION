using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticUtils;
using static ICameraManipulator;
public class PlayerCombatant : Combatant
{
    public override IEnumerator StartTurn()
    {
        yield return StartCoroutine(base.StartTurn());
        Debug.Log("Player did turn");
    }

    protected override void Update()
    {
        // extremely shitty code that shouldn't be placed here but we're out of time so i'll do it anyway
        // handles changing controlled player
        if (CombatManager.Instance.State == CombatManager.CombatState.None && Data.Affiliation == Entity.AffiliationState.Ally)
        {
            // if this is the selected player
            if (CurrentCameraObject() == _cam.gameObject)
            {
                CombatManager.Instance.NavigationTarget.position = transform.position;
            }
            else if (CombatManager.Instance.CurrentSelected != null)
            {
                if (CombatManager.Instance.CurrentSelected.TryGetComponent(out Entity e))
                {
                    if (e.Affiliation == Entity.AffiliationState.Ally && _nav != null && _nav.enabled && _nav.isOnNavMesh)
                    {
                        _nav.SetDestination(CurrentCameraObject().transform.parent.transform.position);
                    }
                }
                // MoveToPath();
            }
            else if (CombatManager.Instance.CurrentSelected == null)
                Debug.LogWarning(name + "Tried to access CombatManager's CurrentSelected, but is null.", this);
        }

        if (ResetMovement)
        {
            ResetMovement = false;
            Data.MovementRemaining = 10;
            ResetPaths();
        }

        if (CombatManager.Instance.State != CombatManager.CombatState.None)
            if (DestinationReached(_nav, transform.position))
                ResetPaths();
    }
}
