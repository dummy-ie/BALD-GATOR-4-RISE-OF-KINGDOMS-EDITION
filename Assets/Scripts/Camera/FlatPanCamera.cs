using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FlatPanCamera : GestureReceiver, ICameraManipulator
{
    [SerializeField]
    private CinemachineVirtualCamera _cam;
    public CinemachineVirtualCamera Cam { get { return _cam; } }

    // Start is called before the first frame update
    protected override void Start()
    {
        // _transposer = GetComponentInChildren<CinemachineOrbitalTransposer>();
        SubscribeToGestures();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
