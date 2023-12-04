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

    /// <summary>
    /// Returns the percentage chance of a roll succeeding a specific difficulty class including modifiers. 
    /// Having dieFaces to 0 or less returns a negative value.
    /// </summary>
    /// <param name="dieFaces"> The face count of the die. Usually 20. </param>
    /// <param name="modifier"> The hit modifiers you would add to the roll. </param>
    /// <param name="difficultyClass"></param>
    /// <returns></returns>
    public float GetPercentageOfRoll(int dieFaces = 0, int modifier = 0, int difficultyClass = 0)
    {
        if (dieFaces <= 0)
            return -1;

        float percentage = (dieFaces + 1 - (difficultyClass - modifier)) / (float)dieFaces;

        if (percentage < 0 || _rollType == ERollType.CRITICAL_FAIL)
            percentage = 0;
        else if (percentage > 100 || _rollType == ERollType.CRITICAL_SUCCESS)
            percentage = 100;

        // Debug.Log("Percentage chance to hit: " + percentage);

        return percentage * 100;
    }

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
        int total = 0;
        for (int i = 0; i < numDice; i++)
        {
            Roll(out int result, diceFaces);
            total += result;
        }

        return total + modifier;
    }

    public bool Roll(out int result, int dieFaces = 20, int modifier = 0, int difficultyClass = 0)
    {
        if (_rollType == ERollType.CRITICAL_SUCCESS)
        {
            result = dieFaces;
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
