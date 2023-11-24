using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedHighlight : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    public bool startPaused = false;

    public void Pause()
    {
        Debug.Log(transform.parent.name + " Highlight Pause() called");
        gameObject.SetActive(false);
        // GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Play()
    {
        Debug.Log(transform.parent.name + " Highlight Play() called");
        gameObject.SetActive(true);
        // GetComponent<SpriteRenderer>().enabled = true;
    }

    private void Start()
    {
        if (startPaused)
            Pause();
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
