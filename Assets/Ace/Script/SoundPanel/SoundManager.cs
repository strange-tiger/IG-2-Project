using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _sfxPlayer;
    [SerializeField]
    private AudioSource _bgmPlayer;

    [SerializeField]
    private AudioClip _bgmClip;
    [SerializeField]
    private AudioClip[] _sfxClip;

    private float _sfxVolume = 1f;
    public float SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }
    private float _bgmVolume = 1f;
    public float BGMVolume { get { return _bgmVolume; } set { _bgmVolume = value; } }

    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    public void PlaySFXSound(string name, float volume = 1f)
    {
        if (_audioClips.ContainsKey(name) == false)
        {
            Debug.Log(name + " is not Contained audioClips");
            return;
        }
        _sfxPlayer.PlayOneShot(_audioClips[name], volume * SFXVolume);
    }
    public void PlayBGMSound(float volume = 1f)
    {
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = volume * BGMVolume;

        _bgmPlayer.clip = _bgmClip;
        _bgmPlayer.Play();
    }

    public void StopBGMSound()
    {
        _bgmPlayer.Stop();
    }
}
