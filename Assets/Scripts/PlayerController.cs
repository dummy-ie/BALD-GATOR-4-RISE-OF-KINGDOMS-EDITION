using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPannable
{
    [SerializeField]
    [Range(0.1f, 100f)]
    private float _moveSpeed = 5f;

    [SerializeField]
    [Range(0.1f, 100f)]
   // private float _panSpeed = 5f;

    private Rigidbody _rigidbody;
    private Vector3 _panTarget = Vector3.zero;

    public void OnPan(PanEventArgs args)
    {
        Vector2 deltaPosition0 = args.TrackedFingers[0].deltaPosition;
        Vector2 deltaPosition1 = args.TrackedFingers[1].deltaPosition;

        Vector2 averagePosition = (deltaPosition0 + deltaPosition1) / 2;
        averagePosition /= Screen.dpi;

        _panTarget = averagePosition;
        Debug.Log("Pan Target: " + _panTarget);
        // figure out later ig, also figure out how to disable joystick while multi touch
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Debug.Log("Input: " + JoystickController.input);

        // transform.Rotate(_panSpeed * _panTarget);
        // transform.RotateAround();
        _rigidbody.MovePosition(transform.position + (_moveSpeed * Time.deltaTime * new Vector3(JoystickController.input.x, 0f, JoystickController.input.y)));
    }
}
