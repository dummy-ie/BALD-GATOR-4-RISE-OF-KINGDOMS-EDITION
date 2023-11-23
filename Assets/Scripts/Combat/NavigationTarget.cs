using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTarget : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 1f, Space.World);
        transform.localScale = new Vector3(_curve.Evaluate(Time.time), _curve.Evaluate(Time.time), transform.localScale.z);
    }
}
