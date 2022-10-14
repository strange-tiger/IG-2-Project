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

    // private AudioSource[] _audioSources = new AudioSource[(int)Defines.ESoundType.MaxCount];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

}
