using UnityEngine;

[System.Serializable]
public class AccelerometerProperty
{
    [SerializeField]
    private float _speedX = 30f;
    public float SpeedX {get {return _speedX;} set {_speedX = value;} }
    
    [SerializeField]
    private float _minChangeX = 5f;
    public float MinChangeX {get {return _minChangeX;} set {_minChangeX = value;} }
}
