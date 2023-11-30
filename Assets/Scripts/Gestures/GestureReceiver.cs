using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GestureReceiver : MonoBehaviour
{
    [Flags]
    public enum ReceiveGestures
    {
        None = 0,
        Tap = 1 << 0,
        Drag = 1 << 1,
        Swipe = 1 << 2,
        Pan = 1 << 3,
        Spread = 1 << 4,
        Rotate = 1 << 5
    }

    [Header("Gesture Manager Subscriptions")]
    [SerializeField]
    protected ReceiveGestures _gestures;

    public virtual void OnTap(object sender, TapEventArgs e) { }
    public virtual void OnDrag(object sender, DragEventArgs e) { }
    public virtual void OnSwipe(object sender, SwipeEventArgs e) { }
    public virtual void OnPan(object sender, PanEventArgs e) { }
    public virtual void OnSpread(object sender, SpreadEventArgs e) { }
    public virtual void OnRotate(object sender, RotateEventArgs e) { }

    protected void SubscribeToGestures()
    {
        if (GestureManager.Instance == null)
        {
            Debug.LogError("Couldn't subscribe to any gestures. GestureManager is null!");
            return;
        }
        if (_gestures.HasFlag(ReceiveGestures.Tap))
        {
            Debug.Log("GestureManager subscribed to Tap.");
            GestureManager.Instance.OnTap += OnTap;
        }
        if (_gestures.HasFlag(ReceiveGestures.Drag))
        {
            Debug.Log("GestureManager subscribed to Drag.");
            GestureManager.Instance.OnDrag += OnDrag;
        }
        if (_gestures.HasFlag(ReceiveGestures.Swipe))
        {
            Debug.Log("GestureManager subscribed to Swipe.");
            GestureManager.Instance.OnSwipe += OnSwipe;
        }
        if (_gestures.HasFlag(ReceiveGestures.Pan))
        {
            Debug.Log("GestureManager subscribed to Pan.");
            GestureManager.Instance.OnPan += OnPan;
        }
        if (_gestures.HasFlag(ReceiveGestures.Spread))
        {
            Debug.Log("GestureManager subscribed to Spread.");
            GestureManager.Instance.OnSpread += OnSpread;
        }
        if (_gestures.HasFlag(ReceiveGestures.Rotate))
        {
            Debug.Log("GestureManager subscribed to Rotate.");
            GestureManager.Instance.OnRotate += OnRotate;
        }
    }

    protected void UnsubscribeToGestures()
    {
        if (GestureManager.Instance == null)
        {
            Debug.LogError("Couldn't unsubscribe from any gestures. GestureManager is null!");
            return;
        }

        if (_gestures.HasFlag(ReceiveGestures.Tap))
            GestureManager.Instance.OnTap -= OnTap;
        if (_gestures.HasFlag(ReceiveGestures.Drag))
            GestureManager.Instance.OnDrag -= OnDrag;
        if (_gestures.HasFlag(ReceiveGestures.Swipe))
            GestureManager.Instance.OnSwipe -= OnSwipe;
        if (_gestures.HasFlag(ReceiveGestures.Pan))
            GestureManager.Instance.OnPan -= OnPan;
        if (_gestures.HasFlag(ReceiveGestures.Spread))
            GestureManager.Instance.OnSpread -= OnSpread;
        if (_gestures.HasFlag(ReceiveGestures.Rotate))
            GestureManager.Instance.OnRotate -= OnRotate;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SubscribeToGestures();
    }

    protected virtual void OnEnable()
    {
        SubscribeToGestures();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeToGestures();
    }
}
