using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldGatorManager : Singleton<BaldGatorManager> {

    [SerializeField]
    GameObject _developerMenu;
    [SerializeField]
    private bool _developerMode = false;
    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame() {
        Time.timeScale = 1.0f;
    }
    void Start() {
        _developerMenu.SetActive(_developerMode);
    }

}
