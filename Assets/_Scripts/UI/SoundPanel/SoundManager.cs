using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    // 볼륨 값 들고있기
    private List<float> _soundVolume = new List<float>();
    private float _masterVolume;
    private float _backgroundVolume;
    private float _effectVolume;
    private float _inputVolume;
    private float _outputVolume;
    public float MasterVolume 
    {
        get => _masterVolume; 
        set
        {
            _masterVolume = value;
            OnChangedMasterVolume.Invoke();
        }
    }
    public float BackgroundVolume
    {
        get => _backgroundVolume;
        set
        {
            _backgroundVolume = value;
            OnChangedBackgroundVolume.Invoke();
        }
    }
    public float EffectVolume 
    { 
        get => _effectVolume;
        set 
        {
            _effectVolume = value;
            OnChangedEffectVolume.Invoke();
        }    
    }
    public float InputVolume
    {
        get => _inputVolume;
        set
        {
            _inputVolume = value;
            OnChangedInputVolume.Invoke();
        }
    }
    public float OutputVolume
    {
        get => _outputVolume;
        set
        {
            _outputVolume = value;
            OnChangedOutputVolume.Invoke();
        }
    }

    public Action OnChangedMasterVolume { private get; set; } = null;
    public Action OnChangedBackgroundVolume { private get; set; } = null;
    public Action OnChangedEffectVolume { private get; set; } = null;
    public Action OnChangedInputVolume { private get; set; } = null;
    public Action OnChangedOutputVolume { private get; set; } = null;

    // PushToTalk 관련
    private bool _isPushToTalk;
    public bool IsPushToTalk { get => _isPushToTalk; set => _isPushToTalk = value; }

    private Recorder _lobbyRecoder;
    public Recorder LobbyRecorder { get { return _lobbyRecoder; } }

    public readonly static string[] VOLUME_CONTROLLER =
       { "MasterVolume", "EffectVolume", "BackGroundVolume", "InputVolume", "OutputVolume" };

    private void Awake()
    {
        _lobbyRecoder = GetComponent<Recorder>();
        _lobbyRecoder.TransmitEnabled = false;

        for (int i = 0; i < VOLUME_CONTROLLER.Length; i++)
        {
            InitValue(VOLUME_CONTROLLER[i]);
            SoundManager.Instance.Refresh(i);
        }

        _soundVolume.Add(MasterVolume);
        _soundVolume.Add(BackgroundVolume);
        _soundVolume.Add(EffectVolume);
        _soundVolume.Add(InputVolume);
        _soundVolume.Add(OutputVolume);
    }

    private float _initVolume = 0.5f;
    private void InitValue(string key)
    {
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetFloat(key, _initVolume);
        }
    }
    public void Refresh(int num)
    {
        _soundVolume[num] = PlayerPrefs.GetFloat(VOLUME_CONTROLLER[num]);
    }
    
    private void CheckPushToTalkInput()
    {
        if (_isPushToTalk == false)
        {
            return;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            LobbyRecorder.TransmitEnabled = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            LobbyRecorder.TransmitEnabled = false;
        }
    }
    private void Update()
    {
        CheckPushToTalkInput();
    }
}