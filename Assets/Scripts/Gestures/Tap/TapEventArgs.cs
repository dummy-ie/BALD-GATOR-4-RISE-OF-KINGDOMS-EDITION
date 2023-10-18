using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEventArgs : EventArgs
{
    private Vector2 _position;
    public Vector2 Position
    {
        get { return _position; }
    }

    private GameObject _hitObject;
    public GameObject HitObject
    {
        get { return _hitObject; }
    }

    public TapEventArgs(Vector2 position, GameObject hitObject = null)
    {
        _position = position;
        _hitObject = hitObject;
    }
}