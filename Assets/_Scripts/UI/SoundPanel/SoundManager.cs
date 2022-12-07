using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    /// <summary>
    /// 각 사운드에 필요한 Event들
    /// </summary>
    private List<UnityEvent<float>> _volumeEvents = new List<UnityEvent<float>>();
    public UnityEvent<float> OnChangedMasterVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedEffectVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedBackgroundVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedInputVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedOutputVolume { get; private set; } = new UnityEvent<float>();

    /// <summary>
    /// PlayerPref에 사용하는 Key값
    /// </summary>
    public readonly static string[] VOLUME_CONTROLLER =
       { "MasterVolume", "EffectVolume", "BackGroundVolume", "InputVolume", "OutputVolume" };

    /// <summary>
    /// Push to talk에 필요한 기능
    /// </summary>
    public bool IsPushToTalk { get; set; }

    private Recorder _lobbyRecoder;
    public Recorder LobbyRecorder { get { return _lobbyRecoder; } }

    private new void Awake()
    {
        base.Awake();

        _volumeEvents.Add(OnChangedMasterVolume);
        _volumeEvents.Add(OnChangedEffectVolume);
        _volumeEvents.Add(OnChangedBackgroundVolume);
        _volumeEvents.Add(OnChangedInputVolume);
        _volumeEvents.Add(OnChangedOutputVolume);

        _lobbyRecoder = GetComponent<Recorder>();

        for (int i = 0; i < VOLUME_CONTROLLER.Length; i++)
        {
            InitValue(VOLUME_CONTROLLER[i]);
            SoundManager.Instance.Refresh(i);
            _volumeEvents[i]?.Invoke(PlayerPrefs.GetFloat(VOLUME_CONTROLLER[i]));
        }
    }

    private float _initVolume = 0.5f;
    private void InitValue(string key)
    {
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetFloat(key, _initVolume);
        }
        else
        {
            return;
        }
    }

    private const int MASTER_VOLUME = 0;
    private const int EFFECT_VOLUME = 1;
    private const int BGM_VOLUME = 2;
    private const int INPUT_VOLUME = 3;
    private const int OUTPUT_VOLUME = 4;

    public void Refresh(int num)
    {
        if (num == MASTER_VOLUME || num == INPUT_VOLUME)
        {
            _volumeEvents[num]?.Invoke(PlayerPrefs.GetFloat(VOLUME_CONTROLLER[num]));
        }
        else
        {
            _volumeEvents[num]?.Invoke(PlayerPrefs.GetFloat(VOLUME_CONTROLLER[num]) * PlayerPrefs.GetFloat(VOLUME_CONTROLLER[MASTER_VOLUME]));
        }
    }
    
    private void CheckPushToTalkInput()
    {
        if (IsPushToTalk == false)
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