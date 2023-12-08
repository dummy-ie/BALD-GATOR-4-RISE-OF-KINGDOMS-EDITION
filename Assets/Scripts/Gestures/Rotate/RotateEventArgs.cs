using System;
using UnityEngine;

public class RotateEventArgs : EventArgs
{
    private Touch[] _trackedFingers;
    public Touch[] TrackedFingers
    {
        get { return _trackedFingers; }
    }

    private ERotateDirection _direction;
    public ERotateDirection Direction
    {
        get { return _direction; }
    }

    private float _angle;
    public float Angle
    {
        get { return _angle; }
    }

    private GameObject _hitObject;
    public GameObject HitObject
    {
        get { return _hitObject; }
    }

    public RotateEventArgs(Touch[] trackedFingers, ERotateDirection direction, float angle, GameObject hitObject = null)
    {
        _direction = direction;
        _angle = angle;
        _hitObject = hitObject;
        _trackedFingers = trackedFingers;

    }
}