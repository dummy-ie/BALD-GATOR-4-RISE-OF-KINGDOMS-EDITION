using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEventArgs : EventArgs
{
    private ESwipeDirection _direction;
    public ESwipeDirection Direction { get { return _direction; } }

    private Vector2 _rawDirection;
    public Vector2 RawDirection { get { return _rawDirection; } }

    private Vector2 _position;
    public Vector2 Position { get { return _position; } }

    private int _moveType;
    public int MoveType { get { return _moveType; } }

    private GameObject _hitObject;
    public GameObject HitObject { get { return _hitObject; } }


    public SwipeEventArgs(ESwipeDirection direction, Vector2 rawDirection, Vector2 position, int moveType, GameObject hitObject = null)
    {
        _direction = direction;
        _rawDirection = rawDirection;
        _position = position;
        _moveType = moveType;
        _hitObject = hitObject;
    }
}
