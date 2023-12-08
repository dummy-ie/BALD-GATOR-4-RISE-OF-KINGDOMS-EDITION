using UnityEngine;
using UnityEngine.UIElements;

public class DevMenu : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private Toggle _succeed;
    private Toggle _fail;
    private Slider _endingMeter;

    private bool _succeedToggled = false;
    private bool _failToggled = false;

    void FixToggles()
    {
        if (this._succeed.value && !this._succeedToggled)
        {
            this._succeedToggled = true;
            this._failToggled = false;
            this._fail.value = false;
        }
        if (this._fail.value && !this._failToggled)
        {
            this._failToggled = true;
            this._succeedToggled = false;
            this._succeed.value = false;
        }
    }

    void ToggleDice()
    {
        if (InternalDice.Instance != null)
        {
            if (!this._succeed.value)
                InternalDice.Instance.ToggleFail(this._fail.value);
            if (!this._fail.value)
                InternalDice.Instance.ToggleSuccess(this._succeed.value);
        }
    }

    void Start()
    {
        this._document = GetComponent<UIDocument>();
        this._root = _document.rootVisualElement;
        this._succeed = this._root.Q<Toggle>("SucceedToggle");
        this._fail = this._root.Q<Toggle>("FailToggle");
        this._endingMeter = this._root.Q<Slider>("EndingMeterValue");
        _document.sortingOrder = 999;
    }

    // Update is called once per frame
    void Update()
    {
        this._endingMeter.label = "Ending Meter [" + this._endingMeter.value + "]";
        FixToggles();
        ToggleDice();
    }
}
