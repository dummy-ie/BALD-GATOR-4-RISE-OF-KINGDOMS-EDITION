using System;
using UnityEngine;

[Serializable]
public class TapProperty
{
    [SerializeField]
    private float _time = 0.7f;
    public float Time
    {
        get { return _time; }
        set { _time = value; }
    }
    [SerializeField]
    private float _maxDistance = 0.1f;
    public float MaxDistance
    {
        get { return _maxDistance; }
        set { _maxDistance = value; }
    }
}