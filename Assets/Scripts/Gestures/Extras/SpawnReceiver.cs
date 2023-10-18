using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnReceiver : MonoBehaviour, ITappable, ISwipeable, IDraggable, ISpreadable, IRotatable
{
    private Vector3 _targetPosition = Vector3.zero;
    [SerializeField] private float _swipeSpeed = 1f;
    [SerializeField] private float _spreadSpeed = 30f;
    [SerializeField] private float _rotateSpeed = 30f;
    public void OnTap(TapEventArgs args)
    {
        Destroy(gameObject);
    }

    public void OnSwipe(SwipeEventArgs args)
    {
        int moveType = args.MoveType;
        float f = 50f;
        switch (args.Direction)
        {
            case ESwipeDirection.UP:
                if (args.RawDirection.x > f)
                    _targetPosition += new Vector3(5f * moveType, 5f, 0f);
                else if (args.RawDirection.x < -f)
                    _targetPosition += new Vector3(-5f * moveType, 5f, 0f);
                else
                    _targetPosition += new Vector3(0f, 5f, 0f);
                break;
            case ESwipeDirection.LEFT:
                if (args.RawDirection.y > f)
                    _targetPosition += new Vector3(-5f, 5f * moveType, 0f);
                else if (args.RawDirection.y < -f)
                    _targetPosition += new Vector3(-5f, -5f * moveType, 0f);
                else
                    _targetPosition += new Vector3(-5f, 0f, 0f);
                break;
            case ESwipeDirection.DOWN:
                if (args.RawDirection.x > f)
                    _targetPosition += new Vector3(5f * moveType, -5f, 0f);
                else if (args.RawDirection.x < -f)
                    _targetPosition += new Vector3(-5f * moveType, -5f, 0f);
                else
                    _targetPosition += new Vector3(0f, -5f, 0f);
                break;
            case ESwipeDirection.RIGHT:
                if (args.RawDirection.y > f)
                    _targetPosition += new Vector3(5f, 5f * moveType, 0f);
                else if (args.RawDirection.y < -f)
                    _targetPosition += new Vector3(5f, -5f * moveType, 0f);
                else
                    _targetPosition += new Vector3(5f, 0f, 0f);
                break;
        }
    }

    public void OnDrag(DragEventArgs args)
    {
        Debug.Log("Drag deez");
        if (args.HitObject == gameObject)
        {
            Vector2 pos = args.TrackedFinger.position;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            Vector2 worldPos = ray.GetPoint(10f);
            transform.position = _targetPosition = worldPos;
        }
    }

    public void OnSpread(SpreadEventArgs args)
    {
        if (args.HitObject == gameObject)
        {
            Debug.Log("Spread deez");
            float scale = args.DistanceDelta / Screen.dpi;
            scale *= _spreadSpeed * Time.deltaTime;
            transform.localScale += new Vector3(scale, scale, scale);
        }
    }

    public void OnRotate(RotateEventArgs args)
    {
        if (args.HitObject == gameObject)
        {
            float angle = args.Angle * (_rotateSpeed * Time.deltaTime);
            if (args.Direction == ERotateDirection.CLOCKWISE)
                angle = -angle;

            transform.Rotate(0, 0, angle);
        }
    }

    private void OnEnable()
    {
        _targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != _targetPosition)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _swipeSpeed * Time.deltaTime);
    }
}
