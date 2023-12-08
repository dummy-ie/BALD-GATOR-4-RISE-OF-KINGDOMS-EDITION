using System.Collections;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;
    public TextMeshPro Text { get { return _text; } }

    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private AnimationCurve _movement;
    private bool _fadingOut = false;
    public float Speed = 1f;
    public float Lifespan = 1.5f;
    public bool Activate = false;
    
    public void Initialize(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startColor = color;
        //_particleSystem.main.startColor = color;
    }

    private IEnumerator FadeOut()
    {
        // Color startColor = _text.color;
        // Debug.Log(_text.color.a);
        while (_text.alpha > 0)
        {
            // _text.color = startColor;
            _text.alpha -= 0.1f;
            yield return null;
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Activate)
            return;

        Lifespan -= Time.deltaTime;

        // transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.forward = Camera.main.transform.forward;

        transform.position += Speed * Time.deltaTime * Vector3.up;

        if (Lifespan <= 0.5 && !_fadingOut)
        {
            _fadingOut = true;
            StartCoroutine(FadeOut());
        }
    }
}
