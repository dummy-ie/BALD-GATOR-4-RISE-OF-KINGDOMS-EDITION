using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEventArgs : EventArgs
{
    private Touch _trackedFinger;
    public Touch TrackedFinger { get { return _trackedFinger; } }

    private GameObject _hitObject;
    public GameObject HitObject { get { return _hitObject; } }

    private Vector2 _startPoint;
    public Vector2 StartPoint
    {
        get { return _startPoint; }
    }

    public DragEventArgs(Touch trackedFinger, Vector2 startPoint, GameObject hitObject = null)
    {
        _trackedFinger = trackedFinger;
        _startPoint = startPoint;
        _hitObject = hitObject;
    }
}
