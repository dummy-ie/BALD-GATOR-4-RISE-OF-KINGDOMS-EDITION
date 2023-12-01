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
        if (!Camera.main.TryGetComponent<CinemachineBrain>(out var brain))
        {
            Debug.LogError("There is no cinemachine brain on the main camera!");
            return null;
        }

        if (brain.ActiveVirtualCamera == null)
        {
            Debug.LogError("There is no virtual camera active in the scene!");
            return null;
        }

        if (brain.ActiveVirtualCamera.VirtualCameraGameObject == null)
        {
            Debug.LogError("There is no vcam gameobject in the scene!");
            return null;
        }

        return brain.ActiveVirtualCamera.VirtualCameraGameObject;
    }

    public static void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        // Debug.Log("Setting " + CurrentCameraObject().name + " inactive.");
        // Debug.Log("Setting " + newCamera.gameObject.name + " active.");
        if (CurrentCameraObject() != null)
            CurrentCameraObject().SetActive(false);
        else
            Debug.LogWarning("CurrentCameraObject is null.");


        newCamera.gameObject.SetActive(true);
    }
}
