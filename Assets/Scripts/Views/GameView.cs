using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class GameView : View
{

    // [SerializeField] private VisualTreeAsset _joystickUXML; // Joystick.uxml file
    // [SerializeField] private StyleSheet _joystickUSS; // Joystick.uss file
    private UIDocument _document;
    private VisualElement _root;
    private Vector3 _startPos;
    private static Vector3 _input = Vector3.zero;
    public static Vector3 Input { get { return _input; } }
    private bool _detectJoystickMovement = false; // you can assume it as a bug fixer flag. App triggers <PointerMoveEvent> for once at the ver first frame of app for a reason I dont know why. So this flag is to prevent it happen
    private VisualElement _joystickElement; // joystick itself (parent joystick element) it will be used to show and hide joystick by changing its style (display: none | flex)
    private VisualElement _joystickKnob; // inner circle of joystick, dynamic moving part
    private float _size = 250; // size(width and height) of joystick element, modify it if you want
    private float _sensitivity = 50; // the higher, the more sensitive. 0 means sudden switches between directions(no sensitivity)

    private Button _menuButton;
    private Button _questButton;
    private VisualElement _menuContainer;
    private Button _resumeButton;
    private Button _quitButton;
    private Label _questName;
    private Label _stepName;

    private ListView _questList;
    public override void Initialize()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        VisualElement joystickTouchArea = _root.Q<VisualElement>("JoystickTouchArea");
        _joystickElement = _root.Q("JoystickOuterBorder"); // There is a parent node named "JoystickOuterBorder" in Joystick.uxml file, just leave it as it is, you will need this variable to show/hide joystick later
        _joystickKnob = _joystickElement.Q("JoystickKnob"); // There is a child node named "JoystickKnob" in Joystick.uxml file, just leave it as it is, you will need this variable to move the little circle on the middle of the joystick later

        _joystickElement.style.width = _size; // applying width of joystick
        _joystickElement.style.height = _size; // applying height of joystick

        _joystickKnob.style.transformOrigin = new TransformOrigin(Length.Percent(100), 0, 0);

        joystickTouchArea.RegisterCallback<PointerDownEvent>((ev) =>
        {
            ShowJoystick(ev);
        });

        joystickTouchArea.RegisterCallback<PointerMoveEvent>((ev) =>
        {
            UpdateJoystick(ev);
        });

        joystickTouchArea.RegisterCallback<PointerUpEvent>((ev) =>
        {
            HideJoystick(ev);
        });

        joystickTouchArea.RegisterCallback<PointerLeaveEvent>((ev) =>
        {
            HideJoystick(ev);
        });

        _menuButton = _root.Q<Button>("MenuButton");
        _questButton = _root.Q<Button>("QuestButton");
        _questName = _root.Q<Label>("QuestName");
        _stepName = _root.Q<Label>("StepName");
        _menuContainer = _root.Q<VisualElement>("MenuContainer");
        _resumeButton = _root.Q<Button>("ResumeButton");
        _quitButton = _root.Q<Button>("QuitButton");
        _menuButton.clicked += OnMenuButtonClicked;
        _questButton.clicked += OnQuestButtonClicked;
        _resumeButton.clicked += OnResumeButtonClicked;
        _quitButton.clicked += OnQuitButtonClicked;
        CombatManager.Instance.CurrentSelected = GameObject.FindGameObjectWithTag("Player");

        _questList = _root.Q<ListView>("QuestList");
    }

    private void ShowJoystick(PointerDownEvent _ev)
    {
        _detectJoystickMovement = true;
        _startPos = _ev.position;
        // _joystickElement.style.left = _ev.position.x - _size / 2;
        // _joystickElement.style.top = _ev.position.y - _size / 2;
        _joystickElement.style.display = DisplayStyle.Flex;
    }

    private void UpdateJoystick(PointerMoveEvent _ev)
    {
        if (_detectJoystickMovement)
        {
            float deltaX = _ev.position.x - _startPos.x;
            float deltaY = _startPos.y - _ev.position.y;
            _input = new Vector3(deltaX, deltaY, 0);
            _input = _input.normalized;

            ApplySensitivity(ref _input, deltaX, deltaY, _sensitivity);

            _joystickKnob.style.translate = new StyleTranslate(new Translate(new Length(_input.x * _size / 2, LengthUnit.Pixel), new Length(-_input.y * _size / 2, LengthUnit.Pixel)));
        }
    }

    private void HideJoystick(PointerUpEvent _ev)
    {
        _input = Vector3.zero;
        _detectJoystickMovement = false;
        // _joystickElement.style.display = DisplayStyle.None;
        _joystickKnob.style.translate = new StyleTranslate(new Translate(new Length(0, LengthUnit.Pixel), new Length(0, LengthUnit.Pixel)));
    }

    private void HideJoystick(PointerLeaveEvent _ev)
    {
        _input = Vector3.zero;
        _detectJoystickMovement = false;
        // _joystickElement.style.display = DisplayStyle.None;
        _joystickKnob.style.translate = new StyleTranslate(new Translate(new Length(0, LengthUnit.Pixel), new Length(0, LengthUnit.Pixel)));
    }

    private static void ApplySensitivity(ref Vector3 input, float _deltaX, float _deltaY, float sensitivity)
    {
        if (Mathf.Abs(_deltaX) >= sensitivity || Mathf.Abs(_deltaY) >= sensitivity) { return; } // it is to avoid stuttering when one of directions is above sensitivity limit, you can assume it as a bug fixer line

        if (_deltaX > 0) // if finger movement is towards right
        {
            input.x = (_deltaX >= sensitivity) ? input.x : Mathf.Lerp(0f, 1f, _deltaX / sensitivity);
        }
        else // if finger movement is towards left
        {
            input.x = (_deltaX <= -sensitivity) ? input.x : Mathf.Lerp(0f, -1f, _deltaX / -sensitivity);
        }

        if (_deltaY > 0) // if finger movement is towards up
        {
            input.y = (_deltaY >= sensitivity) ? input.y : Mathf.Lerp(0f, 1f, _deltaY / sensitivity);
        }
        else // if finger movement is towards down
        {
            input.y = (_deltaY <= -sensitivity) ? input.y : Mathf.Lerp(0f, -1f, _deltaY / -sensitivity);
        }

        // Debug.Log("Input: " + input);
    }

    

    void OnMenuButtonClicked() {
        _menuContainer.style.display = StyleKeyword.Initial;
        Camera.main.gameObject.GetComponent<PostProcessVolume>().enabled = true;
        BaldGatorManager.Instance.PauseGame();
    
    }
    void OnQuestButtonClicked() {
        Debug.Log("Quest Button Clicked");
        Camera.main.gameObject.GetComponent<PostProcessVolume>().enabled = true;
        BaldGatorManager.Instance.PauseGame();
        ViewManager.Instance.GetView<QuestView>().Show();
        Hide();
    }

    void OnResumeButtonClicked() {
        _menuContainer.style.display = StyleKeyword.None;
        Camera.main.gameObject.GetComponent<PostProcessVolume>().enabled = false;
        BaldGatorManager.Instance.ResumeGame();
    }

    void OnQuitButtonClicked() {
        Camera.main.gameObject.GetComponent<PostProcessVolume>().enabled = false;
        BaldGatorManager.Instance.ResumeGame();
        SceneLoader.Instance.LoadMainMenu();
    }

    private void Update()
    {
        if (QuestManager.Instance.TrackedQuest != null)
        {
            _questName.text = QuestManager.Instance.TrackedQuest.Data.DisplayName;
            _stepName.text = QuestManager.Instance.TrackedQuest.GetCurrentStepPrefab().GetComponent<QuestStep>().StepName;
        }
        else
        {
            _questName.text = "";
            _stepName.text = "";
        }
    }
}
