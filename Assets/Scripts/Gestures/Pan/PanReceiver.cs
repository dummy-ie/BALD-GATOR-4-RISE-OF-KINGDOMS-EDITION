using UnityEngine;

public class PanReceiver : MonoBehaviour
{
    private Vector3 _targetPosition;

    [SerializeField]
    private float _speed = 50f;
    // Start is called before the first frame update
    private void Start()
    {
        _targetPosition = Camera.main.transform.position;
        GestureManager.Instance.OnPan += OnPan;
    }

    private void OnPan(object sender, PanEventArgs args)
    {
        Vector2 deltaPosition0 = args.TrackedFingers[0].deltaPosition;
        Vector2 deltaPosition1 = args.TrackedFingers[1].deltaPosition;
    
        Vector2 averagePosition = (deltaPosition0 + deltaPosition1) / 2;
        averagePosition /= Screen.dpi;

        _targetPosition = averagePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, _targetPosition, _speed);
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnPan -= OnPan;
    }
}
