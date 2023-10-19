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
    public abstract void Initialize();

    public virtual void Hide() {
        Debug.Log("Hide View");
        this.gameObject.GetComponent<UIDocument>().enabled = false;
    }

    public virtual void Show() {
        Debug.Log("Show View");
        this.gameObject.GetComponent<UIDocument>().enabled = true;
    }
}
