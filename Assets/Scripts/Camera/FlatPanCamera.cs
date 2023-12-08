using System;
using Cinemachine;
using UnityEngine;

public class FlatPanCamera : GestureReceiver, ICameraManipulator
{
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _panSpeed = 50f;
    
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _dragSpeed = 50f;

    [SerializeField]
    private CinemachineVirtualCamera _cam;
    public CinemachineVirtualCamera Cam { get { return _cam; } }

    private Vector3 _panOffset;
    private Vector3 _dragOffset;

    public override void OnDrag(object sender, DragEventArgs args)
    {
        Vector2 position = args.TrackedFinger.position;

        position -= args.StartPoint;
        Vector3 pos = new(position.x, 0, position.y); // change axis of movement to x and z since the camera is facing down

        _dragOffset = pos / Screen.dpi; // normalize to screen?
    }

    public override void OnPan(object sender, PanEventArgs args)
    {
        Vector2 position0 = args.TrackedFingers[0].position;
        Vector2 position1 = args.TrackedFingers[1].position;
    
        Vector2 averagePosition = (position0 + position1) / 2;

        // i need to add comments to this code but ngl i forgot what it all means
        averagePosition -= args.StartPoint;
        Vector3 pos = new(averagePosition.x, 0, averagePosition.y); // change axis of movement to x and z since the camera is facing down

        _panOffset = pos / Screen.dpi; // normalize to screen?

        // _targetPosition += _cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _cam.transform.position += _panSpeed * Time.deltaTime * _panOffset;   
        _panOffset.x = Mathf.Lerp(_panOffset.x, 0, _panSpeed * Time.deltaTime); 
        _panOffset.z = Mathf.Lerp(_panOffset.z, 0, _panSpeed * Time.deltaTime);

        _cam.transform.position += _dragSpeed * Time.deltaTime * _dragOffset;   
        _dragOffset.x = Mathf.Lerp(_dragOffset.x, 0, _dragSpeed * Time.deltaTime); 
        _dragOffset.z = Mathf.Lerp(_dragOffset.z, 0, _dragSpeed * Time.deltaTime); 
    }
}
