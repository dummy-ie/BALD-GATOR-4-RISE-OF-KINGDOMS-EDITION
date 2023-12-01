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
    private CinemachineVirtualCamera _camera;

    private CinemachineOrbitalTransposer _transposer;

    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } }

    // public bool lockMovement = false;

    private Rigidbody _rigidbody;
    private GameView _gameView;

    private float _sfxTicks = 0.0f;
    private float _sfxTicksRestart = 0.5f;
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_gameView = ViewManager.Instance.GetComponentInChildren<GameView>(true);
        _gameView = ViewManager.Instance.GetView<GameView>();
        _transposer = _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        if (_gameView == null)
            Debug.LogError("No GameView in ViewManager!");
    }

    private void FixedUpdate()
    {
        // movement
        // Debug.Log("Camera's object: " + ICameraManipulator.CurrentCameraObject().transform.parent.name);
        // Debug.Log("this object: " + name);
        // Debug.Log("Are they the same: " + (ICameraManipulator.CurrentCameraObject().transform.parent == gameObject));
        if (GameView.Input != Vector3.zero 
        && CombatManager.Instance.State == CombatManager.CombatState.None // shittiest if conditions known to man
        && ICameraManipulator.CurrentCameraObject() == _camera.gameObject)
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

        if (_isMoving)
        {
            _sfxTicks += Time.deltaTime;
            if (_sfxTicks >= _sfxTicksRestart)
            {
                _sfxTicks = 0.0f;
                AudioManager.Instance.PlaySFX((ESFXIndex)Random.Range(1, 6));
            }
        }
    }
}
