using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "BALD-GATOR-4-RISE-OF-KINGDOMS-EDITION/EntityData", order = 0)]
[Serializable]
public class EntityData : ScriptableObject
{
    [SerializeField]
    [Range(8, 20)]
    private int _strength;
    public int Strength { get { return _strength; } }

    [SerializeField]
    [Range(8, 20)]
    private int _dexterity;
    public int Dexterity { get { return _dexterity; } }

    [SerializeField]
    [Range(8, 20)]
    private int _constitution;
    public int Constitution { get { return _constitution; } }

    [SerializeField]
    [Range(8, 20)]
    private int _intelligence;
    public int Intelligence { get { return _intelligence; } }

    [SerializeField]
    [Range(8, 20)]
    private int _wisdom;
    public int Wisdom { get { return _wisdom; } }

    [SerializeField]
    [Range(8, 20)]
    private int _charisma;
    public int Charisma { get { return _charisma; } }

    [SerializeField]
    private EJobClass _jobClass;
    public EJobClass JobClass { get { return _jobClass; } }

    private int _maxHealth;
    public int MaxHealth { get { return _maxHealth; } }
    private int _health;
    public int Health { get { return _health; } set { _health = value; } }

    public int GetModifier(int statValue)
    {
        return (int)Math.Floor((statValue - 10f) / 2f);
    }

    private void Awake()
    {
        int baseHealth;
        switch (_jobClass)
        {
            case EJobClass.ARTIFICER:
                baseHealth = 70;
                break;
            case EJobClass.BARBARIAN:
                baseHealth = 120;
                break;
            case EJobClass.MONK:
                baseHealth = 100;
                break;
            case EJobClass.PALADIN:
                baseHealth = 110;
                break;
            case EJobClass.ROGUE:
                baseHealth = 90;
                break;
            case EJobClass.WARLOCK:
                baseHealth = 80;
                break;
            default:
                Debug.LogError("No class initialized in entity: " + name + "!");
                return;
        }

        // calculate max health based on con modifier (5 is arbitrary, can change later)
        _maxHealth = baseHealth + (GetModifier(_constitution) * 5);  
        
        // fill health with calculated max
        _health = _maxHealth;
    }
}
