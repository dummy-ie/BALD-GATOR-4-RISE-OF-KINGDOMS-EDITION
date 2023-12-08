using UnityEngine;
using UnityEngine.UIElements;

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

    private void Update()
    {
        _developerMenu.GetComponent<UIDocument>().sortingOrder = 999;
    }

}
