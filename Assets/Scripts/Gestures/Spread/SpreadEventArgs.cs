using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEventArgs : EventArgs
{
    private Touch[] _trackedFingers;
    public Touch[] TrackedFingers
    {
        get { return _trackedFingers; }
    }

    private float _distanceDelta;
    public float DistanceDelta
    {
        get { return _distanceDelta; }
    }

    private GameObject _hitObject;
    public GameObject HitObject
    {
        get { return _hitObject; }
    }

    public SpreadEventArgs(Touch[] trackedFingers, float distanceDelta, GameObject hitObject = null)
    {
        _distanceDelta = distanceDelta;
        _hitObject = hitObject;
        _trackedFingers = trackedFingers;

    }
}