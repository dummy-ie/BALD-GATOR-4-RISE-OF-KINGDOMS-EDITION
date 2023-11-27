using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalDice : Singleton<InternalDice>
{
    // public static InternalDice Instance;

    private ERollType _rollType = ERollType.DEFAULT;
    public ERollType RollType { get { return _rollType; } }

    // private void Awake()
    // {
    //     if (Instance == null)
    //         Instance = this;
    //     else
    //         Destroy(gameObject);
    // }

    public void ToggleSuccess(bool success)
    {
        if (success)
            _rollType = ERollType.CRITICAL_SUCCESS;
        else
            _rollType = ERollType.DEFAULT;
    }

    public void ToggleFail(bool fail)
    {
        if (fail)
            _rollType = ERollType.CRITICAL_FAIL;
        else
            _rollType = ERollType.DEFAULT;
    }

    // rolls numDice-d-diceFaces and returns the result. Ex: 2d6 would be RollMultiple(2, 6, modifier)
    public int RollMultiple(int numDice, int diceFaces, int modifier = 0)
    {
        int result;
        int total = 0;
        for (int i = 0; i < numDice; i++)
        {
            Roll(out result, diceFaces, modifier);
            total += result;
        }

        return total;
    }

    public bool Roll(out int result, int dieFaces = 20, int modifier = 0, int difficultyClass = 0)
    {
        if (_rollType == ERollType.CRITICAL_SUCCESS)
        {
            result = 20;
            return true;
        }

        if (_rollType == ERollType.CRITICAL_FAIL)
        {
            result = 1;
            return false;
        }

        // Random.Range is inclusive min, EXCLUSIVE max for some reason 
        int roll = Random.Range(1, dieFaces + 1);
        roll += modifier;

        result = roll;

        if (roll >= difficultyClass)
            return true;

        return false;
    }
}
