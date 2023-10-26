using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationLock : MonoBehaviour
{
    public static OrientationLock Instance;
    private DeviceOrientation _previousOrientation;
    private bool _isLocked = false;
    public bool IsLocked { get { return _isLocked; } }

    /*
        ● In Project Settings → Player, the Default Orientation must be set to Auto Rotation.
        ● Building an APK is REQUIRED to test this script, as it does NOT work in Unity Remote.
        ● The device testing this script must also have its “Lock Orientation” setting turned OFF.
    */

    public void Lock()
    {
        _isLocked = true;
        int orientation = (int)_previousOrientation;

        if (orientation > 4) // 5 and up are face-up/face-down
            orientation = (int)DeviceOrientation.LandscapeLeft;

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = (ScreenOrientation)orientation;
    }

    public void Unlock()
    {
        _isLocked = false;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        Screen.orientation = ScreenOrientation.LandscapeRight;
        Lock();
    }

    // Update is called once per frame
    private void Update()
    {
        // _previousOrientation = Input.deviceOrientation;
    }
}
