using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    [Tooltip("The 1st GameObject to spawn.")]
    [SerializeField] private GameObject spawn0;
    
    [Tooltip("The 2nd GameObject to spawn.")]
    [SerializeField] private GameObject spawn1;

    private bool _willSpawn2nd = false;

    private void Spawn(Vector3 spawnPosition)
    {
        if (!_willSpawn2nd)
            Instantiate(spawn0, spawnPosition, Quaternion.identity);
        else
            Instantiate(spawn1, spawnPosition, Quaternion.identity);

        _willSpawn2nd = !_willSpawn2nd;
    }

    // Start is called before the first frame update
    private void Start()
    {
        GestureManager.Instance.OnTap += OnTap;
    }

    private void OnTap(object sender, TapEventArgs args)
    {
        if (args.HitObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            Spawn(ray.GetPoint(5f));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        GestureManager.Instance.OnTap -= OnTap;
    }
}
