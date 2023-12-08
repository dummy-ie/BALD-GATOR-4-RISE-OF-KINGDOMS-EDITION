using JetBrains.Annotations;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [CanBeNull]
    private static T _instance;
    public static T Instance {
        get { return _instance; }
    }

    [SerializeField]
    bool _persistent = true;

    protected virtual void OnAwake() { }
    void Awake() {
        if (_instance == null) {
            _instance = this as T;
        }
        else
            Destroy(gameObject);
        if (_persistent)
            DontDestroyOnLoad(gameObject);
        OnAwake();
    }
}
