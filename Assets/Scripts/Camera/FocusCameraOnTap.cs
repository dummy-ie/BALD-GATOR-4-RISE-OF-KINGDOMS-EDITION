using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocusCameraOnTap : MonoBehaviour, ITappable
{
    public CinemachineVirtualCamera _cam;

    public void OnTap(TapEventArgs args)
    {
        if (args.HitObject != gameObject)
            Debug.LogWarning(name + " Receiving tap from different HitObject.");

        // Debug.Log(name + " OnTap()");
        // if (lastObject != null)
        //     Debug.Log(lastObject.name + " OnTap()");

        // activate our highlight
        AnimatedHighlight current = GetComponentInChildren<AnimatedHighlight>(true);
        if (current != null)
            current.Play();
        else
            Debug.LogWarning("OnTap(): " + name + "'s current AnimatedHighlight couldn't be found.", this);
        
        // deactivate current active vcam
        GameObject currentCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;

        GameObject lastObject = currentCam.transform.parent.gameObject;
        currentCam.SetActive(false);
        _cam.gameObject.SetActive(true);

        // deactivate last highlight
        AnimatedHighlight last;
        if (lastObject != null && lastObject != gameObject)
            last = lastObject.GetComponentInChildren<AnimatedHighlight>(true);
        else
            return;

        if (last != null)
            last.Pause();
        else
            Debug.LogWarning("OnTap(): " + lastObject.name + "'s current AnimatedHighlight couldn't be found.", this);

        // causes abrupt cuts
        // _cam.Follow = transform;
        // _cam.LookAt = transform;
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (_cam == null)
            Debug.LogError(name + " FocusCamera not initialized!");

        // if the current active vcam is this one
        GameObject currentCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
        if (currentCam == _cam.gameObject)
        {
            AnimatedHighlight current = GetComponentInChildren<AnimatedHighlight>(true);
            if (current != null)
                current.Play();
            else
                Debug.LogWarning("Start(): " + name + "'s current AnimatedHighlight couldn't be found.", this);
        }
    }
}
