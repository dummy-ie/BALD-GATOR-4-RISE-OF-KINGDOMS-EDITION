using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    private Light _diceLight;

    [SerializeField][Range(0.1f, 60f)]
    private float _timeOut = 10f;

    [SerializeField][Range(0.1f, 1000f)]
    private float _rollForce = 10f;

    [SerializeField][Range(0.1f, 1000f)]
    private float _torqueAmount = 10f;
    private Rigidbody _rigidbody;
    private List<GameObject> _dieFaces = new(20);
    private bool rolling = false;

    [SerializeField]
    private AccelerometerProperty _accelerometerProperty;

    private void CheckAccelerometer()
    {
        if (Math.Abs(Input.acceleration.x) >= _accelerometerProperty.MinChangeX
        && !rolling)
        {
            FireAccelerometerEvent();
        }
    }

    private void FireAccelerometerEvent()
    {
        Vector3 deltaTransform = Vector3.zero;
        deltaTransform.x = Input.acceleration.x * (_accelerometerProperty.SpeedX * Time.deltaTime);
        RollDice(Input.acceleration * (_accelerometerProperty.SpeedX * Time.deltaTime));
        // transform.Translate(deltaTransform);
    }

    public void RollDice(Vector2 direction)
    {
        StopAllCoroutines();

        _diceLight.enabled = false;

        _rigidbody.constraints = RigidbodyConstraints.None;
        rolling = true;
        _rigidbody.AddForce(_rollForce * Time.deltaTime * direction, ForceMode.Impulse);
        _rigidbody.AddTorque(_torqueAmount * Time.deltaTime * direction, ForceMode.Impulse);
        StartCoroutine(WaitForDieToStop());
    }

    private IEnumerator WaitForDieToStop()
    {
        float timeOut = Time.time + _timeOut;

        // wait until die settles or if the time runs out
        while (!_rigidbody.IsSleeping() && Time.time < timeOut) yield return null;
 
        yield return null; // wait another frame

        int dieValue = GetDieValue();
        Debug.Log("Player rolled: " + dieValue);

        // OnDieRolled?.Invoke(dieValue); // Event for later
    }

    private int GetDieValue()
    {
        int number = -1;
        GameObject faceUp = _dieFaces.OrderByDescending(face => face.transform.position.y).First();
        if (faceUp != null)
            int.TryParse(faceUp.name, out number);

        rolling = false;

        _diceLight.enabled = true;


        return number;
    }

    // public void OnSwipe(SwipeEventArgs args)
    // {
    //     if (args.HitObject == gameObject && !rolling)
    //     {
    //         RollDice(args.RawDirection);
    //     }
    // }

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();    
        _rigidbody.constraints = RigidbodyConstraints.FreezePosition;

        foreach (Transform child in transform)
        {
            _dieFaces.Add(child.gameObject);
        }
    }

    private void FixedUpdate()
    {
        CheckAccelerometer();
    }
}
