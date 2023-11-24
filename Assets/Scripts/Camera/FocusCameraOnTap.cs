using Cinemachine;
using UnityEngine;
using static ICameraManipulator;

public class FocusCameraOnTap : MonoBehaviour, ICameraManipulator, ITappable
{
    [SerializeField]
    private CinemachineVirtualCamera _cam;
    public CinemachineVirtualCamera Cam { get { return _cam; } }

    public void OnTap(TapEventArgs args)
    {
        if (args.HitObject != gameObject)
            Debug.LogWarning(name + " Receiving tap from different HitObject.");

        Debug.Log("Tapped on " + args.HitObject.name + ".");

        // switch cameras
        SwitchCamera(Cam);
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (Cam == null)
            Debug.LogError(name + " FocusCamera not initialized!");
    }
}
