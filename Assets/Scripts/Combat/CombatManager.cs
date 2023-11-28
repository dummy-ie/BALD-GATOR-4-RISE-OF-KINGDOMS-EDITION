using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using static Entity;

public class CombatManager : Singleton<CombatManager>
{
    public enum CombatState
    {
        None,
        Start,
        PlayerTurn,
        EnemyTurn,
        Won,
        Lost
    }

    [SerializeField]
    public Transform NavigationTarget;
    public GameObject CurrentSelected;
    public CombatState State;

    // private List<Combatant> _currentTurn;
    [SerializeField]
    private Combatant _currentTurn;

    private int _currentTurnIndex = 0;

    [SerializeField]
    private List<Combatant> _combatants;

    public List<Entity> Entities;

    public bool StartFight = false;

    public IEnumerator StartCombat(List<Entity> entities)
    {
        NavigationTarget.gameObject.SetActive(true);
        State = CombatState.Start;
        foreach (Entity entity in entities)
        {
            // roll for initiative
            InternalDice.Instance.Roll(out entity.Initiative, 20, entity.Class.GetModifier(entity.Class.Dexterity));

            if (entity.TryGetComponent(out Combatant combatant))
            {
                _combatants.Add(combatant);
            }
            else
                Debug.LogError("Entity " + entity.name + " does not have a Combatant component!");
        }

        // wait a little bit before starting 
        // possibly insert animation
        yield return new WaitForSeconds(2f);

        // ViewManager.Instance.Show<CombatView>();
        // Sort combatants by descending initiative
        _combatants.OrderByDescending(e => e.Data.Initiative);
        // normalize the initiative rolls 
        for (int i = 0; i < _combatants.Count; i++)
        {
            _combatants[i].Data.Initiative = i;
        }

        if (_combatants.Count <= 1)
        {
            Debug.LogError("Too Few Combatants in Combat!");
            yield break;
        }
        StartCoroutine(StartNextTurn());
    }

    public void EndCombat()
    {
        NavigationTarget.gameObject.SetActive(false);
        _currentTurn = null;
        _combatants.Clear();
        State = CombatState.None;
    }

    public void AttackSelectedTarget()
    {
        CurrentSelected.TryGetComponent(out Combatant combatant);
        if (combatant == null)
        {
            Debug.LogError("Could not find a Combatant script from the selected target!");
            return;
        }

        if (_currentTurn.Data.Affiliation == combatant.Data.Affiliation)
        {
            Debug.Log(_currentTurn.name + " trying to attack " + CurrentSelected.name + ", but they have the same affiliation.");
            return;
        }

        // Scriptable Object Attack
        // CurrentSelected.TakeDamage(_currentTurn.Attack);

        _currentTurn.DecrementAction();

    }

    public void HealSelectedTarget()
    {

    }

    public void EndCurrentTurn()
    {

    }

    // private void InstantiateEnemy()
    // {
    //     // import Entity stats to enemy
    // }

    // private void InstantiateAlly()
    // {
    //     // import Entity stats to ally
    // }

    private IEnumerator StartNextTurn()
    {
        _currentTurn = _combatants[_currentTurnIndex];

        // Move Camera to current turn
        _currentTurn.OnTap(new TapEventArgs(Vector2.zero));

        AffiliationState current = _combatants[_currentTurnIndex].Data.Affiliation;

        if (current == AffiliationState.Enemy)
            State = CombatState.EnemyTurn;
        else
            State = CombatState.PlayerTurn;

        // just make them do their turn and wait until they choose to end it
        yield return StartCoroutine(_currentTurn.StartTurn());

        // set index to next turn
        if (_currentTurnIndex + 1 < _combatants.Count)
            _currentTurnIndex++;
        else
            _currentTurnIndex = 0;

        // check if won or lost 
        if (CheckWin())
        {
            EndCombat();
        }
        else if (CheckLoss())
        {
            EndCombat();
        }
        else
        {
            // else repeat
            StartCoroutine(StartNextTurn());
        }
    }

    private bool CheckWin()
    {
        return false;
    }

    private bool CheckLoss()
    {
        return false;
    }

    private void OnTap(object sender, TapEventArgs args)
    {
        //Debug.Log("Object hit by CombatManager OnTap(): " + args.HitObject.name);
        if (args.HitObject != null && args.HitObject.CompareTag("Walkable"))
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                NavigationTarget.position = hit.point + Vector3.up * 0.1f;
                if (CurrentSelected.TryGetComponent(out Combatant entity) 
                && State != CombatState.None
                && _currentTurn == entity)
                {
                    entity.StartMove();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        State = CombatState.None;
        GestureManager.Instance.OnTap += OnTap;
        NavigationTarget.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFight)
        {
            StartFight = false;
            StartCoroutine(StartCombat(Entities));
        }
    }
}
