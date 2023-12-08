using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealData", menuName = "ScriptableObjects/HealData", order = 0)]
[Serializable]
public class HealData : ScriptableObject
{
    [SerializeField]
    private int _healModifier;
    public int HealModifier { get { return _healModifier; } }

    [SerializeField]
    private int _range;
    public int Range { get { return _range; } }

    [SerializeField]
    private int _numDice;
    public int NumDice { get { return _numDice; } }

    [SerializeField]
    private int _diceFaces;
    public int DiceFaces { get { return _diceFaces; } }

    // add animation function?
}
