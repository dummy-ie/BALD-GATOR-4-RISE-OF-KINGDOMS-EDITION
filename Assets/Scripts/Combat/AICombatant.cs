using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static StaticUtils;
using static ICameraManipulator;
public class AICombatant : Combatant
{
    [Tooltip("How long will this combatant stay idle (in SECONDS) if they can't do any actions or their actions have failed because I am cramming?")]
    [SerializeField]
    private float _actionTimeOutSeconds = 5f;

    [Tooltip("How long will this combatant stay idle if they can't do anything on their turn? ")]
    [SerializeField]
    private float _turnTimeOutSeconds = 1f;

    private bool _isDoingAction = false;

    private IEnumerator Attack(Combatant enemy)
    {
        // bool isAttacking = true;
        float timer = _actionTimeOutSeconds;

        // Move Camera to enemy
        enemy.OnTap(new TapEventArgs(Vector2.zero));
        enemy.GetComponent<FocusCameraOnTap>().OnTap(new TapEventArgs(Vector2.zero)); // whatever! fuck you

        while (_isDoingAction && timer > 0)
        {
            timer -= Time.deltaTime;

            // move to target
            CombatManager.Instance.NavigationTarget.gameObject.SetActive(true);
            CombatManager.Instance.NavigationTarget.position = enemy.transform.position + Vector3.down + (Vector3.up * 0.1f);
            StartMove();

            yield return null;
        }

        // attack after the timer ends
        CombatManager.Instance.AttackSelectedTarget();

        if (timer <= 0)
            Debug.Log("Timer finished");

        Debug.Log("Attack finished, moving on");
        _isDoingAction = false;
    }

    private IEnumerator HealSelf()
    {
        // bool isHealingSelf = true;
        float timer = _actionTimeOutSeconds;

        while (_isDoingAction && timer > 0)
        {
            timer -= Time.deltaTime;

            // Move Camera back to self
            OnTap(new TapEventArgs(Vector2.zero));
            GetComponent<FocusCameraOnTap>().OnTap(new TapEventArgs(Vector2.zero)); // whatever! fuck you

            CombatManager.Instance.HealSelectedTarget();

            yield return null;
        }

        if (timer <= 0)
            Debug.Log("Timer finished");

        Debug.Log("Heal self finished, moving on");
        _isDoingAction = false;
    }

    private IEnumerator HealAlly(Combatant ally)
    {
        // bool isHealingAlly = true;
        float timer = _actionTimeOutSeconds;

        // Move Camera to ally
        ally.OnTap(new TapEventArgs(Vector2.zero));
        ally.GetComponent<FocusCameraOnTap>().OnTap(new TapEventArgs(Vector2.zero)); // whatever! fuck you

        while (_isDoingAction && timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // heal after timer
        CombatManager.Instance.HealSelectedTarget();

        if (timer <= 0)
            Debug.Log("Timer finished");

        Debug.Log("Heal ally finished, moving on");
        _isDoingAction = false;
    }

    private IEnumerator RunToAlly(Combatant ally)
    {
        // bool isRunningToAlly = true;
        float timer = _actionTimeOutSeconds;

        // Move Camera to ally
        // ally.OnTap(new TapEventArgs(Vector2.zero));
        // ally.GetComponent<FocusCameraOnTap>().OnTap(new TapEventArgs(Vector2.zero)); // whatever! fuck you

        while (_isDoingAction && timer > 0)
        {
            timer -= Time.deltaTime;

            // move to target
            CombatManager.Instance.NavigationTarget.gameObject.SetActive(true);
            CombatManager.Instance.NavigationTarget.position = ally.transform.position + Vector3.down + Vector3.up * 0.1f;
            StartMove();

            yield return null;
        }

        if (timer <= 0)
            Debug.Log("Timer finished");

        Debug.Log("Run to ally finished, moving on");
        _isDoingAction = false;
    }

    private IEnumerator MoveRandomly()
    {
        // bool isMovingRandomly = true;
        float timer = _actionTimeOutSeconds;

        while (_isDoingAction && timer > 0)
        {
            timer -= Time.deltaTime;

            // move to randomly placed target
            CombatManager.Instance.NavigationTarget.gameObject.SetActive(true);
            Vector3 randomDirection = Random.insideUnitSphere * Data.Class.MovementSpeed;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, Data.Class.MovementSpeed, 1);
            Vector3 finalPosition = hit.position;
            CombatManager.Instance.NavigationTarget.position = finalPosition + Vector3.up * 0.1f;

            StartMove();

            yield return null;
        }

        if (timer <= 0)
            Debug.Log("Timer finished");

        Debug.Log("Move randomly finished, moving on");
        _isDoingAction = false;
    }

    public override IEnumerator StartTurn()
    {

        if (Data.Affiliation == Entity.AffiliationState.Ally)
        {
            yield return StartCoroutine(base.StartTurn());
            yield break;
        }

        float turnTimer = _turnTimeOutSeconds;

        if (Data.Health <= 0)
        {
            EndTurn = true;
            Debug.Log("Dead Enemy, skipping");
        }

        Debug.Log("Replenishing movement and actions");
        Data.MovementRemaining = Data.Class.MovementSpeed;
        Data.ActionsLeft = Data.Class.MaxActions;

        while (!EndTurn && turnTimer > 0)
        {
            turnTimer -= 1; // routine waits 1 second to repeat this while loop
            Debug.Log("turnTimer: " + turnTimer);

            // Move Camera back to self
            OnTap(new TapEventArgs(Vector2.zero));
            GetComponent<FocusCameraOnTap>().OnTap(new TapEventArgs(Vector2.zero)); // whatever! fuck you
            yield return new WaitForSeconds(1f);

            // get list of opposition
            List<Combatant> enemies = CombatManager.Instance.Combatants.Where(c => c.Data.Affiliation != Data.Affiliation && c.Data.Health > 0).ToList();
            // get list of allies that aren't themself
            List<Combatant> allies = CombatManager.Instance.Combatants.Where(c => c.Data.Affiliation == Data.Affiliation && c.Data.Health > 0 && c.Data != Data).ToList();
            // choose a random enemy to attack
            Combatant randomEnemy = null;
            if (enemies.Count > 0)
                randomEnemy = enemies[Random.Range(0, enemies.Count)];

            // choose a random ally to heal/go to
            Combatant randomAlly = null;
            if (allies.Count > 0)
                randomAlly = allies[Random.Range(0, allies.Count)];

            if (Data.Class.MaxActions > 0 && enemies.Count > 0)
            {
                _isDoingAction = true;
                // low health action
                if (ClassData.GetPercentage(Data.Health, Data.Class.MaxHealth) <= 50)
                {
                    bool hasDoneValidMove = true;
                    // LOOP THROUGH RANDOM MOVES IF THEY CAN'T DO THE SELECTED ACTION
                    do
                    {
                        hasDoneValidMove = true;
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                if (Data.Class.Heal != null)
                                {
                                    //heal self
                                    // Debug.Log("Healing self");
                                    yield return HealSelf();
                                }
                                else
                                    hasDoneValidMove = false;
                                break;
                            case 1:
                                if (Data.MovementRemaining > 0 && randomAlly != null)
                                {
                                    // Debug.Log("Running to ally");
                                    // run to ally
                                    yield return RunToAlly(randomAlly);
                                }
                                else
                                    hasDoneValidMove = false;
                                break;
                            case 2:
                                if (Data.MovementRemaining > 0)
                                {
                                    // Debug.Log("Moving randomly");
                                    yield return MoveRandomly();
                                }
                                else
                                    hasDoneValidMove = false;
                                break;
                            case 3:
                                // Debug.Log("Attacking as a last resort");
                                // attack out of desperation idk
                                yield return Attack(randomEnemy);
                                break;
                        }
                    }
                    while (!hasDoneValidMove);
                }
                else // high health action
                {
                    bool hasDoneValidMove = true;
                    // LOOP THROUGH RANDOM MOVES IF THEY CAN'T DO THE SELECTED ACTION
                    do
                    {
                        hasDoneValidMove = true;
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                if (Data.Class.Heal != null && allies.Count > 0)
                                {
                                    Combatant lowHealthAlly = allies.OrderBy(a => a.Data.Health).ToList()[0];
                                    // heal random teammate
                                    // Debug.Log("Healing ally");
                                    if (lowHealthAlly != null)
                                        yield return HealAlly(lowHealthAlly);
                                    else
                                        hasDoneValidMove = false;
                                }
                                else
                                    hasDoneValidMove = false;
                                break;
                            case 1:
                                if (Data.MovementRemaining > 0 && allies.Count > 0)
                                {
                                    // Debug.Log("Running to ally");
                                    // run to ally
                                    yield return RunToAlly(randomAlly);
                                }
                                else
                                    hasDoneValidMove = false;
                                break;
                            case 2:
                                if (Data.MovementRemaining > 0)
                                {
                                    // Debug.Log("Moving randomly");
                                    yield return MoveRandomly();
                                }
                                else
                                    hasDoneValidMove = false;
                                break;
                            case 3:
                                // Debug.Log("Attacking enemy");
                                // attack out of desperation idk
                                yield return Attack(randomEnemy);
                                break;
                        }
                    }
                    while (!hasDoneValidMove);
                }
            }
            else
            {
                yield return new WaitForSeconds(1f); // wait a little before ending turn
                Debug.Log("Enemy no longer has actions. Ending turn");
                EndTurn = true;
            }

            yield return new WaitForSeconds(1f);
        }

        if (turnTimer <= 0)
            Debug.Log("Enemy's turn timed out.");

        EndTurn = false;
        Debug.Log("Enemy did turn");
    }

    protected override void Update()
    {
        // extremely shitty code that shouldn't be placed here but we're out of time so i'll do it anyway
        // handles changing controlled player
        if (CombatManager.Instance.State == CombatManager.CombatState.None && Data.Affiliation == Entity.AffiliationState.Ally)
        {
            // if this is the selected player
            if (CurrentCameraObject() != null && CurrentCameraObject() == _cam.gameObject)
            {   
                if (CombatManager.Instance.NavigationTarget != null)
                    CombatManager.Instance.NavigationTarget.position = transform.position;
                else
                    Debug.LogError("Navigation Target is null!");
            }
            else if (CombatManager.Instance.CurrentSelected != null)
            {
                if (CombatManager.Instance.CurrentSelected.TryGetComponent(out Entity e))
                {
                    if (e.Affiliation == Entity.AffiliationState.Ally
                    && _nav != null
                    && _nav.enabled
                    && _nav.isOnNavMesh
                    && CurrentCameraObject() != null)
                    {
                        _nav.SetDestination(CurrentCameraObject().transform.parent.transform.position);
                    }
                    else if (CurrentCameraObject() == null)
                    {
                        Debug.LogError("CurrentCameraObject returns null!");
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

        if (CombatManager.Instance.State != CombatManager.CombatState.None
        && _nav != null
        && _nav.enabled
        && _nav.isOnNavMesh)
        {
            if (DestinationReached(_nav, transform.position))
            {
                ResetPaths();
            }
        }
    }
}
