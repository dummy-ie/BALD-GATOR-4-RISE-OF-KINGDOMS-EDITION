using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSpawnReceiver : MonoBehaviour
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

    private Quaternion Convert(Quaternion gyroscope)
    {
        return new Quaternion(gyroscope.x, gyroscope.y, -gyroscope.z, -gyroscope.w);
    }

    private void FireGyroscope()
    {
        Quaternion rotation = Convert(Input.gyro.attitude);
        transform.rotation = rotation;
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
