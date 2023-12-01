using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;
    public TextMeshPro Text { get { return _text; } }


    [SerializeField]
    private AnimationCurve _movement;
    private Vector3 _startPos;    
    public float lifespan = 1.5f;
    public bool start = false;

    private void Start()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!start)
            return;

        lifespan -= Time.deltaTime;

        transform.Translate(Vector3.up * 0.1f);

        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }
}
