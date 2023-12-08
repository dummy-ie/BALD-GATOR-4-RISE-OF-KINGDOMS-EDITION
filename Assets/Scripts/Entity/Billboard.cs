using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
    [SerializeField] private AssetReference _spriteReference;
    [SerializeField] private BillboardType billboardType;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    

    private Vector3 originalRotation;

    public enum BillboardType { LookAtCamera, CameraForward };

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
    }

    private void Start()
    {
        if (_spriteReference == null)
        {
            Debug.LogError("Sprite Reference is null");
            return;
        }
        AsyncOperationHandle handle = _spriteReference.LoadAssetAsync<Sprite>();
        handle.Completed += (AsyncOperationHandle handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                GetComponent<SpriteRenderer>().sprite = (Sprite)_spriteReference.Asset;

            else
            {
                Debug.LogError($"{_spriteReference.RuntimeKey}.");
            }
        };
    }

    // Use Late update so everything should have finished moving.
    void LateUpdate()
    {
        // There are two ways people billboard things.
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case BillboardType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
        // Modify the rotation in Euler space to lock certain dimensions.
        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);
    }
}