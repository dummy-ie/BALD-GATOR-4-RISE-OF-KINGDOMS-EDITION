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


    public DragEventArgs(Touch trackedFinger, GameObject hitObject = null)
    {
        _trackedFinger = trackedFinger;
        _hitObject = hitObject;
    }
}
