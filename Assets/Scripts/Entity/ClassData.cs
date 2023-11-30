using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassData", menuName = "ScriptableObjects/ClassData", order = 0)]
[Serializable]
public class ClassData : ScriptableObject
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

    [SerializeField]
    [Tooltip("This movement 'speed' is actually the DnD kind, where this is the distance the entity can move on their turn.")]
    private int _movementSpeed = 30;
    public int MovementSpeed { get { return _movementSpeed; } }

    [SerializeField]
    private int _maxHealth;
    public int MaxHealth { get { return _maxHealth; } }

    [SerializeField]
    private AttackData _attack = null;
    public AttackData Attack { get { return _attack; } }

    [SerializeField]
    private HealData _heal = null;
    public HealData Heal { get { return _heal; } }

    [SerializeField]
    private int _maxActions = 1;
    public int MaxActions { get { return _maxActions; } }

    public int ArmorClass { get { return 10 + GetModifier(_dexterity); } }

    public static int GetHealthPercentage(int currentHealth, int maxHealth)
    {
        return (int)(100f / maxHealth * currentHealth);
    }

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

        // fill health with calculated max (moved to Entity class)
        // _health = _maxHealth;
    }
}
