using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FlatPanCamera : GestureReceiver, ICameraManipulator
{
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _panSpeed = 50f;

    [SerializeField]
    private CinemachineVirtualCamera _cam;
    public CinemachineVirtualCamera Cam { get { return _cam; } }

    private Vector3 _targetPosition;


    public override void OnPan(object sender, PanEventArgs args)
    {
        Vector2 deltaPosition0 = args.TrackedFingers[0].deltaPosition;
        Vector2 deltaPosition1 = args.TrackedFingers[1].deltaPosition;
    
        Vector2 averagePosition = (deltaPosition0 + deltaPosition1) / 2;
        averagePosition /= Screen.dpi;

        _targetPosition = new Vector3(averagePosition.x, 0, averagePosition.y);
        _targetPosition += _cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _targetPosition.y = transform.position.y;
        _cam.transform.position = Vector3.MoveTowards(_cam.transform.position, _targetPosition, _panSpeed);
    }
}
