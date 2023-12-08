using Cinemachine;
using UnityEngine;

public class OrbitalPanCamera : GestureReceiver, ICameraManipulator
{
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _panSpeed = 50f;

    [SerializeField]
    private CinemachineVirtualCamera _cam;
    public CinemachineVirtualCamera Cam { get { return _cam; } }

    private CinemachineOrbitalTransposer _transposer;

    private Vector3 _panDelta = Vector3.zero;

    public override void OnPan(object sender, PanEventArgs args)
    {
        // Debug.Log("Panning", this);
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
    protected override void Start()
    {
        // _transposer = GetComponentInChildren<CinemachineOrbitalTransposer>();
        SubscribeToGestures();
        _transposer = Cam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
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
}
