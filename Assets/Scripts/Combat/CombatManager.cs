using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField]
    public Transform NavigationTarget;

    private void OnTap(object sender, TapEventArgs args)
    {
        if (args.HitObject != null && args.HitObject.CompareTag("Walkable"))
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                NavigationTarget.position = hit.point + Vector3.up * 0.1f;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.OnTap += OnTap;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
