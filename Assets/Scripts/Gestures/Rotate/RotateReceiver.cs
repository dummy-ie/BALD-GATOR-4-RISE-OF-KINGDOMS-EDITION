using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RotateReceiver : MonoBehaviour
{
   // Start is called before the first frame update
    private void Start()
    {
        GestureManager.Instance.OnRotate += OnRotate;
    }

    private void OnRotate(object sender, RotateEventArgs args)
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnRotate -= OnRotate;
    }
}
