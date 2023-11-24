using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class FocusCameraOnTap : MonoBehaviour, ITappable
{
    [SerializeField]
    private CinemachineVirtualCamera _cam;

    public void OnTap(TapEventArgs args)
    {
        if (args.HitObject != gameObject)
            Debug.LogWarning(name + " Receiving tap from different HitObject.");


        // deactivate current active vcam
        GameObject currentCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;

        // Debug.Log(currentCam + " OnTap()");
        // Debug.Log(_cam.gameObject.name + " OnTap()");
        
        currentCam.SetActive(false);
        _cam.gameObject.SetActive(true);

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
