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
    public float SFXVolume { get { return _sfxPlayer.volume; } }
    [SerializeField]
    private AudioSource _bgmPlayer;
    public float BGMVolume { get { return _bgmPlayer.volume; } }
    [SerializeField]
    private AudioClip[] _sfxClip;

    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private Recorder _lobbyRecoder;
    public Recorder LobbyRecorder { get { return _lobbyRecoder; } }
    public AudioSource PlayerAudioSource { get; set; }

    public readonly static string[] VOLUME_CONTROLLER =
       { "MasterVolume", "EffectVolume", "BackGroundVolume", "InputVolume", "OutputVolume" };

    private void Awake()
    {
        Debug.Log("SoundManager Awake");
        _lobbyRecoder = GetComponent<Recorder>();

        for (int i = 0; i < VOLUME_CONTROLLER.Length; i++)
        {
            InitValue(VOLUME_CONTROLLER[i]);
        }
        SoundManager.Instance.Refresh();
    }

    private float _initVolume = 0.5f;
    private void InitValue(string key)
    {
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetFloat(key, _initVolume);
        }
    }
    public void Refresh()
    {
        AudioListener.volume = 
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.MasterVolume]);

        if(_bgmPlayer != null) _bgmPlayer.volume = 
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.BackGroundVolume]);

        if (_sfxPlayer != null) _sfxPlayer.volume =
             PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.EffectVolume]);

        if (PlayerAudioSource != null) PlayerAudioSource.volume =
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.InputVolume]);

    }

    public void PlaySFXSound(string name, float volume = 1f)
    {
        if (_audioClips.ContainsKey(name) == false)
        {
            Debug.Log(name + " is not Contained audioClips");
            return;
        }
        _sfxPlayer.PlayOneShot(_audioClips[name], volume * _sfxPlayer.volume);
    }

    public void StopBGMSound()
    {
        _bgmPlayer.Stop();
    }
}