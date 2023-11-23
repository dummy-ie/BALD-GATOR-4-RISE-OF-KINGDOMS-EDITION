using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueView : View
{
    private UIDocument _document;
    private VisualElement _root;
    private VisualElement BG;
    public VisualElement BackGround
    {
        get { return BG; }
    }

    private Label text;
    public Label Text
    {
        get { return text; }
        set { text = value; }
    }

    private List<Button> _choices = new List<Button>();
    public List<Button> Choices
    {
        get { return _choices;}
        set { _choices = value; }
    }


    public Button Degub;


    public override void Initialize()
    {
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;
        this.BG = (VisualElement)this._root.Q("BG");
        this.text = (Label)this._root.Q("Text");
        for (int i = 0; i < 4;i++)
        {
            Button button = (Button)this._root.Q("Option" + (i + 1));

            _choices.Add(button);
        }

        _choices[0].visible = false;
        _choices[0].SetEnabled(false);
        _choices[1].visible = false;
        _choices[1].SetEnabled(false);

        this.BG.visible = false;
        this.BG.SetEnabled(false);



        Degub = (Button)_root.Q("Degub");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
