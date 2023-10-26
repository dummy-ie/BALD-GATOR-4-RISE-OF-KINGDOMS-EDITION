using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class BaldGatorManager : MonoBehaviour {
    public static BaldGatorManager Instance;

    [SerializeField] private bool _developerMode = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start() {
        GameObject.Find("DevMenu").SetActive(_developerMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
