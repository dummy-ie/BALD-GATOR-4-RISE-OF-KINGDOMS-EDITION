using UnityEngine;

public abstract class View : MonoBehaviour {
    [SerializeField]
    bool _onStart = false;
    public bool OnStart { 
        get { return _onStart; } 
    }

    protected int _sortingOrder = 0;
    public int SortingOrder {
        get { return _sortingOrder; }
        set { SetSortingOrder(value); }
    }
    public abstract void Initialize();

    public virtual void Hide() {
        this.gameObject.SetActive(false);
    }

    public virtual void Show() {
        this.gameObject.SetActive(true);
    }

    public virtual void SetSortingOrder(int value) {
        _sortingOrder = value;
    }

    protected void OnEnable() {
        this.Initialize();
    }
}

