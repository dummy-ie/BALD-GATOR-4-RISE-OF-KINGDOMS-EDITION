using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using static StaticUtils;
using static ICameraManipulator;

public class Combatant : MonoBehaviour, ITappable
{
    [SerializeField]
    private Entity _data;
    public Entity Data { get { return _data; } }

    [SerializeField]
    private NavMeshAgent _nav;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private LineRenderer _desiredLineRenderer;

    [SerializeField]
    private LineRenderer _actualLineRenderer;
    private CinemachineVirtualCamera _cam;
    private NavMeshPath _desiredPath;
    private NavMeshPath _actualPath;

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

    public virtual IEnumerator StartTurn()
    {
        while (!EndTurn)
        {
            yield return null;
        }

        EndTurn = false;
        Debug.Log("Player has ended turn");
    }

    public void StartMove()
    {
        MoveToPath();
        RenderPathLine();
    }

    private void ResetPaths()
    {
        _desiredPath = new();
        _actualPath = new();
        _actualLineRenderer.positionCount = 0;
        _desiredLineRenderer.positionCount = 0;
    }

    private void MoveToPath()
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
        Debug.Log("Tapped on " + args.HitObject.name + ".");

        GameObject currentCam = CurrentCameraObject();
        GameObject lastObject = currentCam.transform.parent.gameObject;

        // set self as the current selected gameobject in combat
        CombatManager.Instance.CurrentSelected = gameObject;

        // deactivate last highlight
        FindComponentAndSetActive<AnimatedHighlight>(lastObject, false);

        // activate our highlight
        FindComponentAndSetActive<AnimatedHighlight>(gameObject, true);
    }

    // Start is called before the first frame update
    private void Start()
    {
        ResetPaths();

        _cam = GetComponentInChildren<CinemachineVirtualCamera>(true);

        if (_cam == null)
            Debug.LogError(name + " FocusCamera not initialized!");

        // if the current active vcam is this one
        GameObject currentCam = CurrentCameraObject();
        if (currentCam == _cam.gameObject)
        {
            CombatManager.Instance.CurrentSelected = gameObject;
            FindComponentAndSetActive<AnimatedHighlight>(gameObject, true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (ResetMovement)
        {
            ResetMovement = false;
            _data.MovementRemaining = 10;
            ResetPaths();
        }

        if (EndTurn)
        {
            // EndTurn = false;
            // MoveToPath();
            // RenderPathLine();
        }

        // if (StartMove)
        // {
        //     StartMove = false;
        //     MoveToPath();
        //     RenderPathLine();
        // }

        if (DestinationReached(_nav, transform.position))
            ResetPaths();
    }
}
