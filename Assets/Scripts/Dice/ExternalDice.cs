using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExternalDice : MonoBehaviour
{
    [SerializeField]
    private Light _diceLight;

    [SerializeField]
    [Range(0.1f, 60f)]
    private float _timeOut = 10f;

    [SerializeField]
    [Range(0.1f, 1000f)]
    private float _rollForce = 10f;

    [SerializeField]
    [Range(0.1f, 1000f)]
    private float _torqueAmount = 10f;

    [SerializeField]
    private AccelerometerProperty _accelerometerProperty;

    private Rigidbody _rigidbody;
    private List<GameObject> _dieFaces = new(20);
    private bool _rolling = false;

    private int _result = 0;
    public int Result { get { return _result; } }
    
    // private int _difficultyClass;
    // public int DifficultyClass { get { return _difficultyClass; } }

    private void CheckAccelerometer()
    {
        if (Math.Abs(Input.acceleration.x) >= _accelerometerProperty.MinChangeX
        && !_rolling)
        {
            FireAccelerometerEvent();
        }
    }

    private void FireAccelerometerEvent()
    {
        Vector3 deltaTransform = Vector3.zero;
        deltaTransform.x = Input.acceleration.x * (_accelerometerProperty.SpeedX * Time.deltaTime);
        YeetDice(Input.acceleration * (_accelerometerProperty.SpeedX * Time.deltaTime));
        // transform.Translate(deltaTransform);
    }

    private void YeetDice(Vector2 direction)
    {
        StopAllCoroutines();

        _diceLight.enabled = false;

        _rigidbody.constraints = RigidbodyConstraints.None;
        _rolling = true;
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
        _result = dieValue;
        Debug.Log("Player rolled: " + dieValue);

        // OnDieRolled?.Invoke(dieValue); // Event for later
    }

    private int GetDieValue()
    {
        int number = -1;
        GameObject faceUp = _dieFaces.OrderByDescending(face => face.transform.position.y).First();
        if (faceUp != null)
            int.TryParse(faceUp.name, out number);

        _rolling = false;

        _diceLight.enabled = true;


        return number;
    }

    public bool ResultExternal(int difficultyClass, int modifier = 0)
    {
        if (_result + modifier >= difficultyClass)
            return true;

        return false;
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
