using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private EntityData _entityData;
    public EntityData EntityData { get { return _entityData; } }

    private int _health;
    public int Health { get { return _health; } set { _health = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _health = _entityData.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
