using Photon.Voice.Unity;
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
    private AudioClip[] _sfxClip;

    private float _sfxVolume = 1f;
    public float SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }
    private float _bgmVolume = 1f;
    public float BGMVolume { get { return _bgmVolume; } set { _bgmVolume = value; } }

    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private Recorder _lobbyRecoder;
    public Recorder LobbyRecorder { get { return _lobbyRecoder; } }

    private void Awake()
    {
        _lobbyRecoder = GetComponent<Recorder>();
    }

    private void OnEnable()
    {
        PlayBGMSound(PlayerPrefs.GetFloat
            (VolumeController.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.BackGroundVolume]));
    }

    private void Update()
    {
        _bgmPlayer.volume = PlayerPrefs.GetFloat
            (VolumeController.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.BackGroundVolume]);
    }

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
        _bgmPlayer.volume = volume;
        _bgmPlayer.Play();
    }

    public void StopBGMSound()
    {
        _bgmPlayer.Stop();
    }
}