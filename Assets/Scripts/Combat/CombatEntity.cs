using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CombatEntity : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _nav;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private LineRenderer _desiredLineRenderer;

    [SerializeField]
    private LineRenderer _actualLineRenderer;

    private float _movementLeft = 10;

    private NavMeshPath _desiredPath;
    private NavMeshPath _actualPath;

    public bool StartMove = false;
    public bool ResetMovement = false;

    // private float PathLength(NavMeshPath path)
    // {
    //     if (path.corners.Length < 2)
    //         return 0;

    //     float lengthSoFar = 0.0F;
    //     for (int i = 1; i < path.corners.Length; i++)
    //     {
    //         lengthSoFar += Vector3.Distance(path.corners[i - 1], path.corners[i]);
    //     }
    //     return lengthSoFar;
    // }

    private void ResetPaths()
    {
        _desiredPath = new();
        _actualPath = new();
        _actualLineRenderer.positionCount = 0;
        _desiredLineRenderer.positionCount = 0;
    }

    private void MoveToPath()
    {
        if (_movementLeft <= 0)
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

            _movementLeft = _movementLeft - distanceBetweenLatestCorners >= 0 ? _movementLeft - distanceBetweenLatestCorners : 0;
            if (_movementLeft <= 0)
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

        if (_movementLeft <= 0) // if the agent runs out of movement
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

            // move last element to first in array
            Vector3 last = pathLeft.Last();
            pathLeft = pathLeft.SkipLast(1).Prepend(last).ToArray();

            _desiredLineRenderer.positionCount = pathLeft.Length;
            _desiredLineRenderer.SetPositions(pathLeft);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _actualLineRenderer.positionCount = 0;
        _desiredLineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (ResetMovement)
        {
            ResetMovement = false;
            _movementLeft = 10;
            ResetPaths();
        }

        if (StartMove)
        {
            StartMove = false;
            MoveToPath();
            RenderPathLine();
        }
    }
}
