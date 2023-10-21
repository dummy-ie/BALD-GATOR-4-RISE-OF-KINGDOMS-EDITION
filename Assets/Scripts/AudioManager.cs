using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioClip[] _files;
    [SerializeField]
    private AudioSource _source;

    public AudioClip GetAudio(string name) { 
        for (int i = 0; i < _files.Length; i++) {
            Debug.Log(this._files[i].name);
            if (_files[i].name == name) {
                return _files[i];
            }
        }
        return null;
    }

    public void Play(string name) { 
        for (int i = 0; i < _files.Length; i++) {
            Debug.Log(this._files[i].name);
            if (_files[i].name == name) {
                this._source.clip = _files[i];
                Debug.Log(this._source.clip.name);
                this._source.Play();
                return;
            }
        }
    }

    public void Stop() { 
        this._source.Stop();
        this._source.clip = null;
    }


    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
