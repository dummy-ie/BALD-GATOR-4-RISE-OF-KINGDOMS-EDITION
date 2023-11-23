using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class OrbitalPanCamera : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _panSpeed = 50f;

    [SerializeField]
    private CinemachineVirtualCamera _camera;

    private CinemachineOrbitalTransposer _transposer;

    private Vector3 _panDelta = Vector3.zero;


    public void OnPan(object sender, PanEventArgs args)
    {
        Debug.Log("Panning", this);
        Vector2 position0 = args.TrackedFingers[0].position;
        Vector2 position1 = args.TrackedFingers[1].position;

        Vector2 currentPosition = (position0 + position1) / 2;
        // currentPosition /= Screen.dpi;

        _panDelta = (currentPosition - args.StartPoint) / Screen.dpi;
        // _joystick.DetectJoystickMovement = false;
        // Debug.Log("Pan Target: " + _panTarget);
        // figure out later ig, also figure out how to disable joystick while multi touch
    }

    // Start is called before the first frame update
    void Start()
    {
        // _transposer = GetComponentInChildren<CinemachineOrbitalTransposer>();
        _transposer = _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        GestureManager.Instance.OnPan += OnPan;
    }

    // Update is called once per frame
    void Update()
    {
        if (_panDelta != Vector3.zero)
        {
            _transposer.m_RecenterToTargetHeading.m_enabled = false;

            // camera pan
            _transposer.m_Heading.m_Bias += _panDelta.x * _panSpeed;
            _panDelta.x = Mathf.Lerp(_panDelta.x, 0, _panSpeed * Time.deltaTime);
        }

        if (_panDelta == Vector3.zero)
            _transposer.m_RecenterToTargetHeading.m_enabled = true; // recenter cam while not panning
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnPan -= OnPan;
    }
}
