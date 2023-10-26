using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _moveSpeed = 5f;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float _rotationSpeed = 5f;

    [SerializeField]
    [Range(0.1f, 100f)]
    private float _panSpeed = 50f;

    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } }

    // public bool lockMovement = false;

    private Rigidbody _rigidbody;
    private GameView _gameView;
    private CinemachineOrbitalTransposer _transposer;
    private Vector3 _panDelta = Vector3.zero;

    public void OnPan(object sender, PanEventArgs args)
    {
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
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gameView = ViewManager.Instance.GetComponentInChildren<GameView>();
        _transposer = GetComponentInChildren<CinemachineOrbitalTransposer>();
        GestureManager.Instance.OnPan += OnPan;

        if (_gameView == null)
            Debug.LogError("No GameView in ViewManager!");
    }

    private void FixedUpdate()
    {
        // movement
        if (GameView.Input != Vector3.zero)
        {
            _transposer.m_RecenterToTargetHeading.m_enabled = false;
            _isMoving = true;

            //camera forward and right vectors:
            Vector3 forward = _transposer.transform.forward;
            Vector3 right = _transposer.transform.right;

            //project forward and right vectors on the horizontal plane (y = 0)
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            //this is the direction in the world space we want to move:
            Vector3 move = forward * GameView.Input.y + right * GameView.Input.x;

            _rigidbody.MovePosition(transform.position + (_moveSpeed * Time.deltaTime * move));
            _rigidbody.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(-move),
                                                  Time.deltaTime * _rotationSpeed);
        }
        else
            _isMoving = false;

        if (_panDelta != Vector3.zero)
        {
            _transposer.m_RecenterToTargetHeading.m_enabled = false;

            // camera pan
            _transposer.m_Heading.m_Bias += _panDelta.x * _panSpeed;
            _panDelta.x = Mathf.Lerp(_panDelta.x, 0, _panSpeed * Time.deltaTime);
        }

        if (_panDelta == Vector3.zero
            && GameView.Input == Vector3.zero)
            _transposer.m_RecenterToTargetHeading.m_enabled = true; // recenter cam while stood still & not panning

    }

    private void OnDisable()
    {
        GestureManager.Instance.OnPan -= OnPan;
    }
}
