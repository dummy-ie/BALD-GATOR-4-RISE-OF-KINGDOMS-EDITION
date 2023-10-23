using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeReceiver : MonoBehaviour
{
    private void EnableGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
        else
            Debug.LogError("NO GYRO MOMENTO");
    }

    private void CheckGyroscope()
    {
        if (Input.gyro.rotationRate != Vector3.zero)
            FireGyroscope();
    }

    private void FireGyroscope()
    {
        Vector3 rotation = Input.gyro.rotationRate;

        rotation.x = -rotation.x;
        rotation.y = -rotation.y;
        transform.Rotate(rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        EnableGyroscope();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGyroscope();
    }
}
