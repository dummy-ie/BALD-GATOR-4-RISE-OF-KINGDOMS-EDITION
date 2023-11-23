using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    private static int _result = 0;
    // public static int Result { get { return _result; } }

    private bool _finishedRolling = false;
    public bool FinishedRolling { get { return _finishedRolling; } }

    // public DialogueArgs nextArgs;
    // public DialogueArgs nextArgsOther;
    // public DialogueClass target;

    // private int _difficultyClass;
    // public int DifficultyClass { get { return _difficultyClass; } }

    public void CheckAccelerometer()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(0)) && !_rolling)
        {
            Debug.Log("Rolling dice with spacebar");
            YeetDice(new Vector2(UnityEngine.Random.Range(1, 101), UnityEngine.Random.Range(1, 101)));
        }

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
        _finishedRolling = true;

        if (ResultExternal(10))
        {
            DialogueManager.Instance.SetOutcome(0);
            DialogueManager.Instance.ContinueDialogue();
        }

        else
        {
            DialogueManager.Instance.SetOutcome(1);
            DialogueManager.Instance.ContinueDialogue();
        }
               

        yield return new WaitForSeconds(1f); // wait a bit
        SceneManager.UnloadSceneAsync("Dice Roller");
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

    public static bool ResultExternal(int difficultyClass, int modifier = 0)
    {
        Debug.Log("Result External _result: " + _result);
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
