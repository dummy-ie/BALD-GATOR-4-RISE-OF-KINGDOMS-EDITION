using System;
using UnityEngine;

public class AccelerometerReceiver : MonoBehaviour
{
    [SerializeField]
    private AccelerometerProperty _accelerometerProperty;

    private void CheckAccelerometer()
    {
        if (Math.Abs(Input.acceleration.x) >= _accelerometerProperty.MinChangeX)
        {
            FireAccelerometerEvent();
        }
    }

    private void FireAccelerometerEvent()
    {
        Vector3 deltaTransform = Vector3.zero;
        deltaTransform.x = Input.acceleration.x * (_accelerometerProperty.SpeedX * Time.deltaTime);
        transform.Translate(deltaTransform);
    }

    private void FixedUpdate()
    {
        CheckAccelerometer();
    }
}
