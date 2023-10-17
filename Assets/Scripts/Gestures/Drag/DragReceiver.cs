using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GestureManager.Instance.OnDrag += OnDrag;
    }

    private void OnDrag(object sender, DragEventArgs args)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        GestureManager.Instance.OnDrag -= OnDrag;
    }
}
