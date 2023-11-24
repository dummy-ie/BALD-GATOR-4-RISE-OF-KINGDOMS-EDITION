using Cinemachine;
using UnityEngine;

public interface ICameraManipulator
{
    public CinemachineVirtualCamera Cam { get; }
    public static ICinemachineCamera CurrentCamera()
    {
        return Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
    }

    public static GameObject CurrentCameraObject()
    {
        return Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
    }

    public static void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        Debug.Log("Setting " + CurrentCameraObject().name + " inactive.");
        Debug.Log("Setting " + newCamera.gameObject.name + " active.");
        CurrentCameraObject().SetActive(false);
        newCamera.gameObject.SetActive(true);
    }
}
