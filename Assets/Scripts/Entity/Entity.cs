using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum AffiliationState
    {
        Ally,
        Enemy
    }

    [SerializeField]
    private ClassData _class;
    public ClassData Class { get { return _class; } }

    public int Initiative = 0;

    public AffiliationState Affiliation;
    
    private int _actionsLeft;
    public int ActionsLeft { get { return _actionsLeft; } set { _actionsLeft = value; } }

    private int _health;
    public int Health { get { return _health; } set { _health = value; } }

    private float _movementRemaining = 10;
    public float MovementRemaining { get { return _movementRemaining; } set { _movementRemaining = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _health = _class.MaxHealth;
        _movementRemaining = _class.MovementSpeed;
        // InternalDice.Instance.Roll(out Initiative, 20, _data.GetModifier(_data.Dexterity));
    }
}
