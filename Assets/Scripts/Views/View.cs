using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class View : MonoBehaviour {
    [SerializeField]
    protected UIDocument _document;
    protected VisualElement _root;
    public abstract void Initialize();

    public virtual void Hide() {
        this.gameObject.SetActive(false);
    }

    public virtual void Show() {
        this.gameObject.SetActive(true);
    }

    protected void OnEnable() {
        this.Initialize();
    }
}
