using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance;
    private Touch[] _trackedFingers = new Touch[2];
    private float _gestureTime;
    private Vector2 _startPoint = Vector2.zero;
    private Vector2 _endPoint = Vector2.zero;

    [SerializeField]
    private TapProperty _tapProperty;
    public EventHandler<TapEventArgs> OnTap;

    [SerializeField]
    private SwipeProperty _swipeProperty;
    [SerializeField]
    private int _swipeType;
    public EventHandler<SwipeEventArgs> OnSwipe;

    [SerializeField]
    private DragProperty _dragProperty;
    public EventHandler<DragEventArgs> OnDrag;

    [SerializeField]
    private PanProperty _panProperty;
    public EventHandler<PanEventArgs> OnPan;

    [SerializeField]
    private SpreadProperty _spreadProperty;
    public EventHandler<SpreadEventArgs> OnSpread;

    [SerializeField]
    private RotateProperty _rotateProperty;
    public EventHandler<RotateEventArgs> OnRotate;

    private void CheckTap()
    {
        if (_gestureTime <= _tapProperty.Time
            && Vector2.Distance(_startPoint, _endPoint) <= _tapProperty.MaxDistance * Screen.dpi)
        {
            FireTapEvent();
        }
    }

    private void FireTapEvent()
    {
        GameObject hitObject = GetHitObject(_startPoint);

        TapEventArgs args = new(_startPoint, hitObject);
        OnTap?.Invoke(this, args);

        if (hitObject != null)
        {
            ITappable handler = hitObject.GetComponent<ITappable>();
            handler?.OnTap(args);
        }
    }
    private void CheckSwipe()
    {
        if (_gestureTime <= _swipeProperty.Time
            && Vector2.Distance(_startPoint, _endPoint) >= _swipeProperty.MinDistance * Screen.dpi)
        {
            FireSwipeEvent();
        }
    }

    private void FireSwipeEvent()
    {
        GameObject hitObject = GetHitObject(_startPoint);

        Vector2 rawDirection = _endPoint - _startPoint;
        ESwipeDirection direction = GetSwipeDirection(rawDirection);

        SwipeEventArgs args = new(direction, rawDirection, _startPoint, _swipeProperty.MoveType, hitObject);
        OnSwipe?.Invoke(this, args);

        if (hitObject != null)
        {
            ISwipeable handler = hitObject.GetComponent<ISwipeable>();
            handler?.OnSwipe(args);
        }
    }

    ESwipeDirection GetSwipeDirection(Vector2 rawDirection)
    {
        if (Math.Abs(rawDirection.x) > Math.Abs(rawDirection.y))
        {
            if (rawDirection.x > 0)
            {
                return ESwipeDirection.RIGHT;
            }
            else
            {
                return ESwipeDirection.LEFT;
            }
        }
        else
        {
            if (rawDirection.y > 0)
            {
                return ESwipeDirection.UP;
            }
            else
            {
                return ESwipeDirection.DOWN;
            }
        }
    }

    private void CheckDrag()
    {
        if (_gestureTime >= _dragProperty.Time)
        {
            FireDragEvent();
        }
    }

    private void FireDragEvent()
    {
        GameObject hitObject = GetHitObject(_trackedFingers[0].position);

        DragEventArgs args = new(_trackedFingers[0], hitObject);
        OnDrag?.Invoke(this, args);

        if (hitObject != null)
        {
            IDraggable handler = hitObject.GetComponent<IDraggable>();
            handler?.OnDrag(args);
        }
    }

    private void CheckSingleFingerInput()
    {
        _trackedFingers[0] = Input.GetTouch(0);

        switch (_trackedFingers[0].phase)
        {
            case TouchPhase.Began:
                _startPoint = _trackedFingers[0].position;
                _gestureTime = 0;
                break;
            case TouchPhase.Ended:
                _endPoint = _trackedFingers[0].position;
                CheckTap();
                CheckSwipe();
                break;
            default:
                _gestureTime += Time.deltaTime;
                CheckDrag();
                break;
        }
    }

    private void CheckPan()
    {
        if (Vector2.Distance(_trackedFingers[0].position, _trackedFingers[1].position) <= _panProperty.MaxDistance * Screen.dpi)
            FirePanEvent();
    }

    private void FirePanEvent()
    {
        PanEventArgs args = new(_trackedFingers);
        OnPan?.Invoke(this, args);
    }

    private void CheckSpread()
    {
        float previousDistance = Vector2.Distance(GetPreviousPoint(_trackedFingers[0]), GetPreviousPoint(_trackedFingers[1]));
        float currentDistance = Vector2.Distance(_trackedFingers[0].position, _trackedFingers[1].position);
        float distanceDelta = currentDistance - previousDistance;
        if (Math.Abs(distanceDelta) >= _spreadProperty.MinDistanceChange)
        {
            FireSpreadEvent(distanceDelta);
        }
    }

    private void FireSpreadEvent(float distanceDelta)
    {
        if (distanceDelta > 0)
            Debug.Log("Spread dat ass");
        else
            Debug.Log("Pinch dat ass");

        GameObject hitObject = GetHitObject(GetMidPoint(_trackedFingers[0].position, _trackedFingers[1].position));

        SpreadEventArgs args = new(_trackedFingers, distanceDelta, hitObject);
        OnSpread?.Invoke(this, args);

        if (hitObject != null)
        {
            ISpreadable handler = hitObject.GetComponent<ISpreadable>();
            handler?.OnSpread(args);
        }
    }

    private void CheckRotate()
    {
        Vector2 previousDifference = GetPreviousPoint(_trackedFingers[0]) - GetPreviousPoint(_trackedFingers[1]);
        Vector2 currentDifference = _trackedFingers[0].position - _trackedFingers[1].position;
        float angle = Vector2.Angle(previousDifference, currentDifference);
        if (Vector2.Distance(_trackedFingers[0].position, _trackedFingers[1].position) >= _rotateProperty.MinDistanceChange * Screen.dpi
            && angle >= _rotateProperty.MinRotationChange)
            FireRotateEvent(angle, previousDifference, currentDifference);
    }

    private void FireRotateEvent(float angle, Vector2 previousDifference, Vector2 currentDifference)
    {
        Debug.Log("Rotating by " + angle);

        GameObject hitObject = GetHitObject(GetMidPoint(_trackedFingers[0].position, _trackedFingers[1].position));

        ERotateDirection direction;
        if (Vector3.Cross(previousDifference, currentDifference).z > 0)
            direction = ERotateDirection.COUNTERCLOCKWISE;
        else
            direction = ERotateDirection.CLOCKWISE;

        RotateEventArgs args = new(_trackedFingers, direction, angle, hitObject);
        OnRotate?.Invoke(this, args);

        if (hitObject != null)
        {
            IRotatable handler = hitObject.GetComponent<IRotatable>();
            handler?.OnRotate(args);
        }
    }

    private void CheckDualFingerInput()
    {
        _trackedFingers[0] = Input.GetTouch(0);
        _trackedFingers[1] = Input.GetTouch(1);

        switch (_trackedFingers[0].phase, _trackedFingers[1].phase)
        {
            case (TouchPhase.Moved, TouchPhase.Moved):
                CheckPan();
                break;
        }
        switch (_trackedFingers[0].phase, _trackedFingers[1].phase)
        {
            case (TouchPhase.Moved, _):
            case (_, TouchPhase.Moved):
                CheckSpread();
                CheckRotate();
                break;
        }
    }

    private Vector2 GetPreviousPoint(Touch finger)
    {
        return finger.position - finger.deltaPosition;
    }

    GameObject GetHitObject(Vector2 rayPoint)
    {
        GameObject hitObject = null;
        Ray ray = Camera.main.ScreenPointToRay(rayPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            hitObject = hit.collider.gameObject;
        }

        return hitObject;
    }

    Vector2 GetMidPoint(Vector2 a, Vector2 b)
    {
        return new Vector2((a.x + b.x) / 2, (a.y + b.y) / 2);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            switch (Input.touchCount)
            {
                case 1:
                    CheckSingleFingerInput();
                    break;
                case 2:
                    CheckDualFingerInput();
                    break;
            }
        }
    }
}