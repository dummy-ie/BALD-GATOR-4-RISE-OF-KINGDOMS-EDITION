using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dice : MonoBehaviour, ISwipeable
{
    [SerializeField][Range(0.1f, 60f)]
    private float _timeOut = 10f;

    [SerializeField][Range(0.1f, 100f)]
    private float _rollForce = 10f;

    [SerializeField][Range(0.1f, 100f)]
    private float _torqueAmount = 10f;
    private Rigidbody _rigidbody;
    private List<GameObject> _dieFaces = new(20);
    private bool rolling = false;

    public void RollDice(Vector2 swipeRawDirection)
    {
        StopAllCoroutines();
        _rigidbody.constraints = RigidbodyConstraints.None;
        rolling = true;
        _rigidbody.AddForce(_rollForce * Time.deltaTime * swipeRawDirection, ForceMode.Impulse);
        _rigidbody.AddTorque(_torqueAmount * Time.deltaTime * swipeRawDirection, ForceMode.Impulse);
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

        return number;
    }

    public void OnSwipe(SwipeEventArgs args)
    {
        if (args.HitObject == gameObject && !rolling)
        {
            RollDice(args.RawDirection);
        }
    }

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

    // Update is called once per frame
    private void Update()
    {
        
    }
}
