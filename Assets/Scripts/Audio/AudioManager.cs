using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    AudioMixerGroup _audioMixerGroup;
    [SerializeField]
    private AudioClip[] _sfxFiles;
    [SerializeField]
    private AudioClip[] _bgmFiles;
    [SerializeField]
    private GameObject _sfxObjectPrefab;

    private AudioSource[] _sourceChannels;

    public AudioClip GetSFX(string name) { 
        for (int i = 0; i < _sfxFiles.Length; i++) {
            Debug.Log(this._sfxFiles[i].name);
            if (_sfxFiles[i].name == name) {
                return _sfxFiles[i];
            }
        }
        return null;
    }

    public AudioClip GetBGM(string name) { 
        for (int i = 0; i < _bgmFiles.Length; i++) {
            Debug.Log(this._bgmFiles[i].name);
            if (_bgmFiles[i].name == name) {
                return _bgmFiles[i];
            }
        }
        return null;
    }

    public void PlaySFX(ESFXIndex index)
    {
        GameObject sfxObject = Instantiate(_sfxObjectPrefab, transform);
        AudioSource sfxSource = sfxObject.GetComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = _audioMixerGroup;
        sfxSource.clip = _sfxFiles[(int)index];
        sfxSource.PlayOneShot(sfxSource.clip, 1);
    }

    public void PlayBGM(EBGMIndex index, int channel) {
        if (_sourceChannels[channel].clip == null || !(_sourceChannels[channel].clip.name == _bgmFiles[(int)index].name)) {
            this._sourceChannels[channel].clip = _bgmFiles[(int)index];
            this._sourceChannels[channel].Play();
            this._sourceChannels[channel].loop = true;
        }
    }

    public void StopBGM() { 
        foreach (AudioSource sourceChannel in _sourceChannels)
        {
            sourceChannel.Stop();
            sourceChannel.clip = null;
            sourceChannel.loop = false;
        }
    }

    protected override void OnAwake()
    {
        _sourceChannels = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource sourceChannel in _sourceChannels)
        {
            sourceChannel.outputAudioMixerGroup = _audioMixerGroup;
        }
    }
}
