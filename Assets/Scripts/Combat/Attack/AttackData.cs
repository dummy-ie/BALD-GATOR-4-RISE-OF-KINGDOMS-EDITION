using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/AttackData", order = 0)]
[Serializable]
public class AttackData : ScriptableObject
{
    [Tooltip("The hit modifier determines the bonus to the chance for a specific attack to hit.")]
    [SerializeField]
    private int _hitModifier;
    public int HitModifier { get { return _hitModifier; } }
    
    [SerializeField]
    private int _damageModifier;
    public int DamageModifier { get { return _damageModifier; } }
    
    [SerializeField]
    private int _range;
    public int Range { get { return _range; } }

    [SerializeField]
    private int _numDice;
    public int NumDice { get { return _numDice; } }

    [SerializeField]
    private int _diceFaces;
    public int DiceFaces { get { return _diceFaces; } }
}
