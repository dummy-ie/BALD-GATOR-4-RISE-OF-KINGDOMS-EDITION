using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SwipeProperty
{
    [Tooltip("The time it takes to swipe.")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _time = 2.0f;
    public float Time
    { get { return _time; } set { _time = value; } }

    [Tooltip("The type of movement. 0 Perpendicular, 1 Diagonal")]
    [Range(0,1)]
    [SerializeField]
    private int _moveType = 0;
    public int MoveType { get { return _moveType; } set { _moveType = value; } }

    [Tooltip("The minimum distance it takes to swipe.")]
    [SerializeField]
    private float _minDistance = 0.7f;
    public float MinDistance { get { return _minDistance; } set { _minDistance = value; } }


}
