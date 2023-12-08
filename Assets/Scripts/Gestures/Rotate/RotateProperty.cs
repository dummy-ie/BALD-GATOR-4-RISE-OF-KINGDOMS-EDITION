using System;
using UnityEngine;

[Serializable]
public class RotateProperty
{
    [SerializeField]
    private float _minDistanceChange = 0.75f;
    public float MinDistanceChange
    {
        get { return _minDistanceChange; }
        set { _minDistanceChange = value; }
    }

    private float _minRotationChange = 0.4f;
    public float MinRotationChange
    {
        get { return _minRotationChange; }
        set { _minRotationChange = value; }
    }
}