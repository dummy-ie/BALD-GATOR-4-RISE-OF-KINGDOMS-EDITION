using System;
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

    public virtual void OnTap(object sender, TapEventArgs args) { }
    public virtual void OnDrag(object sender, DragEventArgs args) { }
    public virtual void OnSwipe(object sender, SwipeEventArgs args) { }
    public virtual void OnPan(object sender, PanEventArgs args) { }
    public virtual void OnSpread(object sender, SpreadEventArgs args) { }
    public virtual void OnRotate(object sender, RotateEventArgs args) { }

    protected void SubscribeToGestures()
    {        
        if (GestureManager.Instance == null)
        {
            Debug.LogError("Couldn't subscribe to any gestures. GestureManager is null!", this);
            return;
        }
        if (_gestures.HasFlag(ReceiveGestures.Tap))
        {
            Debug.Log("GestureManager subscribed to Tap.", this);
            GestureManager.Instance.OnTap += OnTap;
        }
        if (_gestures.HasFlag(ReceiveGestures.Drag))
        {
            Debug.Log("GestureManager subscribed to Drag.", this);
            GestureManager.Instance.OnDrag += OnDrag;
        }
        if (_gestures.HasFlag(ReceiveGestures.Swipe))
        {
            Debug.Log("GestureManager subscribed to Swipe.", this);
            GestureManager.Instance.OnSwipe += OnSwipe;
        }
        if (_gestures.HasFlag(ReceiveGestures.Pan))
        {
            Debug.Log("GestureManager subscribed to Pan.", this);
            GestureManager.Instance.OnPan += OnPan;
        }
        if (_gestures.HasFlag(ReceiveGestures.Spread))
        {
            Debug.Log("GestureManager subscribed to Spread.", this);
            GestureManager.Instance.OnSpread += OnSpread;
        }
        if (_gestures.HasFlag(ReceiveGestures.Rotate))
        {
            Debug.Log("GestureManager subscribed to Rotate.", this);
            GestureManager.Instance.OnRotate += OnRotate;
        }
    }

    protected void UnsubscribeToGestures()
    {
        if (GestureManager.Instance == null)
        {
            Debug.LogError("Couldn't unsubscribe from any gestures. GestureManager is null!", this);
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
