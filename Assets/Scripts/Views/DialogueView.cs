using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueView : View
{
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

    private Button Option1;
    public Button Choice1
    {
        get { return Option1; }
    }

    private Button Option2;
    public Button Choice2
    {
        get { return Option2; }
    }

    private Button Option3;
    public Button Choice3
    {
        get { return Option3; }
    }

    private Button Option4;
    public Button Choice4
    {
        get { return Option4; }
    }

    public override void Initialize()
    {
        this._root = this._document.rootVisualElement;
        this.BG = (VisualElement)this._root.Q("BG");
        this.text = (Label)this._root.Q("Text");
        this.Option1 = (Button)this._root.Q("Option1");
        this.Option2 = (Button)this._root.Q("Option2");
        this.Option3 = (Button)this._root.Q("Option3");
        this.Option4 = (Button)this._root.Q("Option4");

        Option1.visible = false;
        Option2.visible = false;

        this.BG.visible = false;
        this.BG.SetEnabled(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
