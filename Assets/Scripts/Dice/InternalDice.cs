using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalDice : MonoBehaviour
{
    public static InternalDice Instance;

    private ERollType _rollType = ERollType.DEFAULT;
    public ERollType RollType { get { return _rollType; } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

    public bool RollInternal(int difficultyClass, int modifier = 0)
    {
        if (_rollType == ERollType.CRITICAL_SUCCESS)
            return true;

        if (_rollType == ERollType.CRITICAL_FAIL)
            return false;

        int roll = Random.Range(1, 21);
        roll += modifier;

        if (roll >= difficultyClass)
            return true;

        return false;
    }
}
