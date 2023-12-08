using UnityEngine;

public class AnimatedHighlight : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    public bool startPaused = false;

    // use StaticUtils.FindComponentAndSetActive instead.
    // public static void FindAndSetHighlight(GameObject objWithHighlight, bool value)
    // {
    //     if (objWithHighlight == null)
    //     {
    //         Debug.LogWarning("SetHighlight(): GameObject is null.");
    //         return;
    //     }

    //     AnimatedHighlight highlight = objWithHighlight.GetComponentInChildren<AnimatedHighlight>(true); // get highlight in its children
    //     if (highlight != null)
    //         highlight.gameObject.SetActive(value);
    //     else if (objWithHighlight.TryGetComponent(out highlight)) // try in its components
    //         highlight.gameObject.SetActive(value);
    //     else
    //         Debug.LogWarning("SetHighlight(): " + objWithHighlight.name + "'s AnimatedHighlight couldn't be found.");
    // }

    private void Start()
    {
        if (startPaused)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // if (isPaused)
        //     return;

        transform.Rotate(Vector3.up, 1f, Space.World);
        transform.localScale = new Vector3(_curve.Evaluate(Time.time), _curve.Evaluate(Time.time), transform.localScale.z);
    }
}
