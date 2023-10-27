using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class View : MonoBehaviour
{
    [SerializeField]
    protected UIDocument _document;
    protected VisualElement _root;
    public VisualElement Root { get { return _root; } }
    public abstract void Initialize();

    public virtual void Hide()
    {
        // this.gameObject.SetActive(false);
        this._root.style.display = DisplayStyle.None;
    }

    public virtual void Show()
    {
        // this.gameObject.SetActive(true);
        this._root.style.display = DisplayStyle.Flex;
    }

    protected void OnEnable()
    {
        this.Initialize();
    }
}
