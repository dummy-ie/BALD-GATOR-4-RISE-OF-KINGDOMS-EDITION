using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldGatorManager : Singleton<BaldGatorManager> {

    [SerializeField]
    GameObject _developerMenu;
    [SerializeField]
    private bool _developerMode = false;
    void PauseGame()
    {
        
    }

    void ResumeGame() { 
    
    }
    void Start() {
        _developerMenu.SetActive(_developerMode);
    }

}
