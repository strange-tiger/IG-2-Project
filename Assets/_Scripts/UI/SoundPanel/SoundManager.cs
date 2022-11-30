using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    // 볼륨 값 들고있기
    private List<UnityEvent<float>> _actions = new List<UnityEvent<float>>();
    public UnityEvent<float> OnChangedMasterVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedEffectVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedBackgroundVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedInputVolume { get; private set; } = new UnityEvent<float>();
    public UnityEvent<float> OnChangedOutputVolume { get; private set; } = new UnityEvent<float>();

    // PushToTalk 관련
    private bool _isPushToTalk;
    public bool IsPushToTalk { get => _isPushToTalk; set => _isPushToTalk = value; }

    private Recorder _lobbyRecoder;
    public Recorder LobbyRecorder { get { return _lobbyRecoder; } }

    public readonly static string[] VOLUME_CONTROLLER =
       { "MasterVolume", "EffectVolume", "BackGroundVolume", "InputVolume", "OutputVolume" };

    private void Awake()
    {
        _actions.Add(OnChangedMasterVolume);
        _actions.Add(OnChangedEffectVolume);
        _actions.Add(OnChangedBackgroundVolume);
        _actions.Add(OnChangedInputVolume);
        _actions.Add(OnChangedOutputVolume);

        _lobbyRecoder = GetComponent<Recorder>();
        _lobbyRecoder.TransmitEnabled = false;

        for (int i = 0; i < VOLUME_CONTROLLER.Length; i++)
        {
            InitValue(VOLUME_CONTROLLER[i]);
            SoundManager.Instance.Refresh(i);
            _actions[i]?.Invoke(PlayerPrefs.GetFloat(VOLUME_CONTROLLER[i]));
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
    public void Refresh(int num)
    {
        if (num == 0)
        {
            _actions[num]?.Invoke(PlayerPrefs.GetFloat(VOLUME_CONTROLLER[num]));
        }
        else
        {
            _actions[num]?.Invoke(PlayerPrefs.GetFloat(VOLUME_CONTROLLER[num]) * PlayerPrefs.GetFloat(VOLUME_CONTROLLER[0]));
        }
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