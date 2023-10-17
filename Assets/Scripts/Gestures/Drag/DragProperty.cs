using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DragProperty
{
    [Tooltip("The time it takes to drag.")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _time = 1.0f;
    public float Time
    { get { return _time; } set { _time = value; } }
}
