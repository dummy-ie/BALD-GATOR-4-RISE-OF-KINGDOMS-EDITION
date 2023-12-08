using UnityEngine;

public class SwipeReceiver : MonoBehaviour
{

    // Start is called before the first frame update
    private void Start()
    {
        GestureManager.Instance.OnSwipe += OnSwipe;
    }

    private void OnSwipe(object sender, SwipeEventArgs args)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        GestureManager.Instance.OnSwipe -= OnSwipe;
    }
}
