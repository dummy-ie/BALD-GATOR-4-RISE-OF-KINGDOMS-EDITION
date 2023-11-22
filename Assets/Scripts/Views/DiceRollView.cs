using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceRollView : View
{
    private UIDocument _document;
    private VisualElement _root;
    public override void Initialize()
    {   
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;
    }
}
