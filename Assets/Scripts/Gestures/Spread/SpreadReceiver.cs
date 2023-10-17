using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpreadReceiver : MonoBehaviour
{
   // Start is called before the first frame update
    private void Start()
    {
        GestureManager.Instance.OnSpread += OnSpread;
    }

    private void OnSpread(object sender, SpreadEventArgs args)
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnSpread -= OnSpread;
    }
}
