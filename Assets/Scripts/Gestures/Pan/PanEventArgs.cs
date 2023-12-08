using System;
using UnityEngine;

public class PanEventArgs : EventArgs
{
    private Touch[] _trackedFingers;
    public Touch[] TrackedFingers
    {
        get { return _trackedFingers; }
    }

    private Vector2 _startPoint;
    public Vector2 StartPoint
    {
        get { return _startPoint; }
    }

    public PanEventArgs(Touch[] trackedFingers, Vector2 startPoint)
    {
        _trackedFingers = trackedFingers;
        _startPoint = startPoint;
    }
}