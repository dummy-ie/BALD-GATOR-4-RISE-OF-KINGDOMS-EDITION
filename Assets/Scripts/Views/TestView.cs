using UnityEngine.UIElements;

public class TestView : View
{
    UIDocument _document;
    VisualElement _root;
    Button _testButton;
    QuestPoint _questPoint;
    public override void Initialize()
    {
        /*_document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        _testButton = _root.Q<Button>("TestButton");
        _questPoint = GetComponent<QuestPoint>();
        _testButton.clicked += _questPoint.SubmitPressed;*/
    }
}
