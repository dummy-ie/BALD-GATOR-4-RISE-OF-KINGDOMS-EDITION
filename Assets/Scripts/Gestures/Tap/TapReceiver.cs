using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    [Tooltip("The GameObject to spawn.")]
    [SerializeField] private GameObject spawn;
    private void Spawn(Vector3 spawnPosition)
    {
        Instantiate(spawn, spawnPosition, Quaternion.identity);
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
