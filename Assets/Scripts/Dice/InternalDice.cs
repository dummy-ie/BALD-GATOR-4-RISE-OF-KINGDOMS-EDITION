using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalDice : MonoBehaviour
{
    private enum RollType
    {
        CRITICAL_FAIL, // 1
        CRITICAL_SUCCESS, // 20
        DEFAULT
    }

    public static InternalDice Instance;

    private RollType _rollType = RollType.DEFAULT;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ToggleSuccess()
    {
        if (_rollType == RollType.DEFAULT)
            _rollType = RollType.CRITICAL_SUCCESS;
        else
            _rollType = RollType.DEFAULT;
    }

    public void ToggleFail()
    {
        if (_rollType == RollType.DEFAULT)
            _rollType = RollType.CRITICAL_FAIL;
        else
            _rollType = RollType.DEFAULT;
    }

    public bool RollInternal(int difficultyClass, int modifier = 0)
    {
        if (_rollType == RollType.CRITICAL_SUCCESS)
            return true;

        if (_rollType == RollType.CRITICAL_FAIL)
            return false;

        int roll = Random.Range(1, 21);
        roll += modifier;

        if (roll >= difficultyClass)
            return true;

        return false;
    }
}
