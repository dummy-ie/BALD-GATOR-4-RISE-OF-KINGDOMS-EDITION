using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceRollView : View
{
    public override void Initialize()
    {
        this._root = this._document.rootVisualElement;
    }
}
