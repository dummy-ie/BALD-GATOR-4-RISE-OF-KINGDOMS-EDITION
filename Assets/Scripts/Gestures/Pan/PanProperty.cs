using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanProperty
{
    [SerializeField]
    private float _maxDistance = 0.7f;
    public float MaxDistance
    {
        get { return _maxDistance; }
        set { _maxDistance = value; }
    }
}