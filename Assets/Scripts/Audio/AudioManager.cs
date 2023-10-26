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

    public void Play(string name, bool loop = false) { 
        for (int i = 0; i < _files.Length; i++) {
            if (_files[i].name == name) {
                this._source.clip = _files[i];
                this._source.Play();
                this._source.loop = loop;
                return;
            }
        }
    }

    public void Play(int index, bool loop = false) { 
        for (int i = 0; i < _files.Length; i++) {
            this._source.clip = _files[index];
            this._source.Play();
            this._source.loop = loop;
            return;
        }
    }

    public void Stop() { 
        this._source.Stop();
        this._source.clip = null;
        this._source.loop = false;
    }


    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
