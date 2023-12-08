using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;

public class MainMenuView : View {
    [SerializeField] private AssetReference _nextSceneReference;
    [SerializeField] private SceneConnection _sceneConnection;
    private UIDocument _document;
    private VisualElement _root;
    private Button _playButton;
    private Button _settingsButton;

    public override void Initialize() {
        this._document = GetComponent<UIDocument>();
        this._root = _document.rootVisualElement;
        this._playButton = this._root.Q<Button>("PlayButton");
        this._settingsButton = this._root.Q<Button>("SettingsButton");
        this._playButton.clicked += PlayButtonClicked;
        this._settingsButton.clicked += SettingsButtonClicked;
        AudioManager.Instance.StartBGM(EBGMIndex.MAIN_MENU, 0);
        _document.sortingOrder = 0;
    }

    public void PlayButtonClicked() {
        Debug.Log("Clicked Play Button");
        //AudioManager.Instance.PlaySFX(ESFXIndex.MAJESTIC_SOUND);
        SceneLoader.Instance.LoadSceneWithFade(_nextSceneReference);
        _playButton.clicked -= PlayButtonClicked;
        AudioManager.Instance.StartBGM(EBGMIndex.TOWN_1, 0);
        SceneConnection.ActiveConnection = _sceneConnection;
        //ViewManager.Instance.Show(ViewManager.Instance.GetComponentInChildren<GameView>());
    }

    public void SettingsButtonClicked() {
        Debug.Log("Clicked Settings Button");
        ViewManager.Instance.PopUp<SettingsView>();
    }

    private void OnDisable() {
        //AudioManager.Instance.StopBGM();
    }
}