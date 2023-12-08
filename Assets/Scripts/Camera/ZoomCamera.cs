using Cinemachine;
using UnityEngine;

public class ZoomCamera : GestureReceiver, ICameraManipulator
{
    [SerializeField]
    private CinemachineVirtualCamera _cam;
    public CinemachineVirtualCamera Cam { get { return _cam; } }

    [SerializeField]
    private float _zoomSpeed = 30f; // change as needed ig

    public override void OnSpread(object sender, SpreadEventArgs args)
    {
        float zoom = args.DistanceDelta / Screen.dpi;
        zoom *= Time.deltaTime * _zoomSpeed;
        
        _cam.transform.position += Vector3.down * zoom;
        
        // _cam.m_Lens.FieldOfView += -zoom;

        // if (_cam.m_Lens.FieldOfView > 95f)
        //     _cam.m_Lens.FieldOfView = 95f;
        // else if (_cam.m_Lens.FieldOfView < 80f)
        //     _cam.m_Lens.FieldOfView = 80f;

    }
}
