using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocusCameraOnTap : MonoBehaviour, ITappable
{
    public CinemachineVirtualCamera _cam;

    public void OnTap(TapEventArgs args)
    {
        if (args.HitObject != gameObject)
            Debug.LogWarning(name + " Receiving tap from different HitObject.");        

        _cam.Follow = transform;
        _cam.LookAt = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_cam == null)
            Debug.LogError(name + " FocusCamera not initialized!");
    }
}
