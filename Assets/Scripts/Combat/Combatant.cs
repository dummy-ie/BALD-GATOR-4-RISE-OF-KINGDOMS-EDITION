using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using static StaticUtils;
using static ICameraManipulator;

public abstract class Combatant : MonoBehaviour, ITappable
{
    [SerializeField]
    private Entity _data;
    public Entity Data { get { return _data; } }

    [SerializeField]
    protected NavMeshAgent _nav;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private LineRenderer _desiredLineRenderer;

    [SerializeField]
    private LineRenderer _actualLineRenderer;
    protected CinemachineVirtualCamera _cam;
    private NavMeshPath _desiredPath;
    private NavMeshPath _actualPath;

    private Projector _attackRange;
    private Projector _healRange;


    // debug stuff vvv

    // public bool StartMove = false;
    public bool EndTurn = false;
    public bool ResetMovement = false;

    public static float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        float lengthSoFar = 0.0F;
        for (int i = 1; i < path.corners.Length; i++)
        {
            lengthSoFar += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return lengthSoFar;
    }

    //  Check if the model is moving on the NavMesh
    public static bool DestinationReached(NavMeshAgent agent, Vector3 actualPosition)
    {
        //  because takes some time to update the remainingDistance and will return a wrong value
        if (agent.pathPending)
        {
            return Vector3.Distance(actualPosition, agent.pathEndPosition) <= agent.stoppingDistance;
        }
        else
        {
            return agent.remainingDistance <= agent.stoppingDistance;
        }
    }

    public string TryHit(AttackData attack)
    {
        if (!InternalDice.Instance.Roll(out int hitRoll, 20, attack.HitModifier, _data.Class.ArmorClass))
        {
            Debug.Log("Attack missed " + name);
            return "Miss!";
        }

        int damageDealt = InternalDice.Instance.RollMultiple(attack.NumDice, attack.DiceFaces, attack.DamageModifier);
        if (_data.Health - damageDealt >= 0)
            _data.Health -= damageDealt;
        else
            Die();

        return damageDealt.ToString();
    }

    public string HealHealth(HealData heal)
    {
        int healthHealed = InternalDice.Instance.RollMultiple(heal.NumDice, heal.DiceFaces, heal.HealModifier);
        if (_data.Health + healthHealed <= _data.Class.MaxHealth)
            _data.Health += healthHealed;
        else
            _data.Health = _data.Class.MaxHealth;

        return healthHealed.ToString();
    }

    public void DecrementAction()
    {
        if (_data.ActionsLeft > 0)
            _data.ActionsLeft--;
        else
            _data.ActionsLeft = 0;
    }

    public virtual IEnumerator StartTurn()
    {
        if (_data.Health <= 0)
        {
            EndTurn = true;
            Debug.Log("Dead, skipping");
        }

        _data.MovementRemaining = _data.Class.MovementSpeed;
        _data.ActionsLeft = _data.Class.MaxActions;

        while (!EndTurn)
        {
            yield return null;
        }

        EndTurn = false;
    }

    public void StartMove()
    {
        MoveToPath();
        RenderPathLine();
    }

    private void Die()
    {
        _data.Health = 0;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = Color.gray;
    }

    protected void ResetPaths()
    {
        _desiredPath = new();
        _actualPath = new();
        _actualLineRenderer.positionCount = 0;
        _desiredLineRenderer.positionCount = 0;
    }

    protected void MoveToPath()
    {
        if (_data.MovementRemaining <= 0)
            return;

        ResetPaths();

        bool foundPath = NavMesh.CalculatePath(transform.position, _target.position, NavMesh.AllAreas, _desiredPath);

        if (!foundPath)
            Debug.LogWarning(name + " did not find a suitable path. Show this to the player in GUI.");

        if (_desiredPath.corners.Length < 2)
        {
            Debug.Log("Path Corner Count: " + _desiredPath.corners.Length, this);
            _nav.SetPath(_desiredPath);
            return;
        }

        // float lengthSoFar = 0.0F;
        for (int i = 1; i < _desiredPath.corners.Length; i++)
        {
            float distanceBetweenLatestCorners = Vector3.Distance(_desiredPath.corners[i - 1], _desiredPath.corners[i]);

            _data.MovementRemaining = _data.MovementRemaining - distanceBetweenLatestCorners >= 0 ? _data.MovementRemaining - distanceBetweenLatestCorners : 0;
            if (_data.MovementRemaining <= 0)
            {
                NavMesh.CalculatePath(transform.position, (_desiredPath.corners[i - 1] - _desiredPath.corners[i]) / 2 + _desiredPath.corners[i], NavMesh.AllAreas, _actualPath);
                _nav.SetPath(_actualPath);
                return;
            }
        }

        _nav.SetPath(_desiredPath);
    }

    private void RenderPathLine()
    {
        _actualLineRenderer.positionCount = _desiredPath.corners.Length;
        _actualLineRenderer.SetPositions(_desiredPath.corners);

        if (_data.MovementRemaining <= 0) // if the agent runs out of movement
        {
            if (_actualPath == null)
            {
                Debug.LogWarning("NavMesh Path Actual Path is NULL.", this);
                return;
            }

            // get duplicate values 
            Vector3[] duplicates = _desiredPath.corners.Intersect(_actualPath.corners).ToArray();
            // get difference between paths and remove duplicates
            Vector3[] pathLeft = _desiredPath.corners.Union(_actualPath.corners).Except(duplicates).ToArray();

            // move last element to first in array if array isn't empty
            if (pathLeft != null && pathLeft.Length > 0)
            {
                Vector3 last = pathLeft.Last();
                pathLeft = pathLeft.SkipLast(1).Prepend(last).ToArray();
            }
            _desiredLineRenderer.positionCount = pathLeft.Length;
            _desiredLineRenderer.SetPositions(pathLeft);
        }
    }

    // public void SetHighlight(bool value)
    // {
    //     AnimatedHighlight highlight = GetComponentInChildren<AnimatedHighlight>(true);
    //     if (highlight != null)
    //         highlight.gameObject.SetActive(value);
    //     else
    //         Debug.LogWarning("OnTap(): " + name + "'s AnimatedHighlight couldn't be found.", this);
    // }

    public void OnTap(TapEventArgs args)
    {
        // Debug.Log("Tapped on " + args.HitObject.name + ".");
        if (CombatManager.Instance.CurrentTurn != null && ViewManager.Instance.GetView<CombatView>() != null)
        {
            if (CombatManager.Instance.CurrentTurn != this)
                ViewManager.Instance.GetView<CombatView>().SetTargetData(this);
            else
                ViewManager.Instance.GetView<CombatView>().SetTargetData();

            if (CombatManager.Instance.CurrentTurn.Data.Affiliation != Data.Affiliation)
                ViewManager.Instance.GetView<CombatView>().SetAttackHitPercentage(20, CombatManager.Instance.CurrentTurn.Data.Class.Attack.HitModifier, Data.Class.ArmorClass);
            else
                ViewManager.Instance.GetView<CombatView>().SetAttackHitPercentage(); // set to no text
        }


        GameObject currentCam = CurrentCameraObject();
        GameObject lastObject = null;
        if (currentCam != null)
            lastObject = currentCam.transform.parent.gameObject;
        else
        {
            Debug.LogError("CurrentCam is null!!!!!!!");
            return;
        }

        // set self as the current selected gameobject in combat
        CombatManager.Instance.CurrentSelected = gameObject;

        // deactivate last highlight
        lastObject.FindComponentAndSetActive<AnimatedHighlight>(false, out _);

        // activate our highlight
        gameObject.FindComponentAndSetActive<AnimatedHighlight>(true, out _);

        // extremely shitty code that shouldn't be placed here but we're out of time so i'll do it anyway
        // handles changing controlled player
        if (CombatManager.Instance.State == CombatManager.CombatState.None)
        {
            if (lastObject.TryGetComponent(out PlayerController lastController))
                lastController.enabled = false;

            if (lastObject.TryGetComponent(out Entity e))
            {
                if (e.Affiliation == Entity.AffiliationState.Ally)
                {
                    if (lastObject.TryGetComponent(out NavMeshAgent lastAgent))
                        lastAgent.enabled = true;
                    if (lastObject.TryGetComponent(out Rigidbody rb))
                        rb.isKinematic = true;
                }
            }

            if (Data.Affiliation != Entity.AffiliationState.Enemy)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<PlayerController>().enabled = true;
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (_target == null)
        {
            GameObject obj = CombatManager.Instance.GetComponentInChildren<AnimatedHighlight>(true).gameObject;
            if (obj != null)
                _target = obj.transform;
            else
                Debug.LogError("Couldn't find the navigation target! obj is null");
        }

        // _target = FindObjectsByType<AnimatedHighlight>(FindObjectsSortMode.InstanceID).First(o => o.name == "Navigation Target").transform;

        ResetPaths();
        _data.ActionsLeft = _data.Class.MaxActions;
        // _attackRange = GetComponentInChildren<Projector>(true);
        // _healRange = GetComponentInChildren<Projector>(true);
        _target = CombatManager.Instance.NavigationTarget;
        foreach (Projector projector in GetComponentsInChildren<Projector>(true))
        {
            if (projector.name == "AttackRangeIndicator")
                _attackRange = projector;

            if (projector.name == "HealRangeIndicator")
                _healRange = projector;
        }

        if (Data.Class.Attack != null)
        {
            _attackRange.orthographicSize = Data.Class.Attack.Range;
            // _attackRange.material.SetColor("_Color", Color.red);
            _attackRange.ChangeColor(Color.red);
        }
        else
            _attackRange.orthographicSize = 0;

        if (Data.Class.Heal != null)
        {
            _healRange.orthographicSize = Data.Class.Heal.Range;
            // _healRange.material.SetColor("_Color", Color.green);
            _healRange.ChangeColor(Color.green);
        }
        else
            _healRange.orthographicSize = 0;


        _cam = GetComponentInChildren<CinemachineVirtualCamera>(true);

        if (_cam == null)
            Debug.LogError(name + " FocusCamera not initialized!");

        AnimatedHighlight highlight = GetComponentInChildren<AnimatedHighlight>(true);

        // if the current active vcam is this one
        GameObject currentCam = CurrentCameraObject();
        if (currentCam != null && currentCam == _cam.gameObject)
        {
            CombatManager.Instance.CurrentSelected = gameObject;
            highlight.gameObject.SetActive(true);
        }

        // Debug.Log(name + "'s Affiliation: " + _data.Affiliation);
        // set the highlight's color
        if (highlight != null)
        {
            // Debug.Log(name + "'s Highlight: " + highlight.name);
            bool hasSprite = highlight.TryGetComponent(out SpriteRenderer sprite);

            if (!hasSprite)
                return;

            if (_data.Affiliation == Entity.AffiliationState.Ally)
                sprite.color = Color.blue;
            else if (_data.Affiliation == Entity.AffiliationState.Enemy)
                sprite.color = Color.red;

            Color tmp = sprite.color;
            tmp.a = 60;
            sprite.color = tmp;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (ResetMovement)
        {
            ResetMovement = false;
            _data.MovementRemaining = 10;
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
