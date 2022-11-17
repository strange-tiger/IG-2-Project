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

    // ���� �� ����ֱ�
    private float _masterVolume;
    private float _backgroundVolume;
    private float _effectVolume;
    private float _inputVolume;
    private float _outputVolume;
    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public float BackgroundVolume { get => _backgroundVolume; set => _backgroundVolume = value; }
    public float EffectVolume { get => _effectVolume; set => _effectVolume = value; }
    public float InputVolume { get => _inputVolume; set => _inputVolume = value; }
    public float OutputVolume { get => _outputVolume; set => _outputVolume = value; }

    // PushToTalk ����
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
        MasterVolume =
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.MasterVolume]);
        BackgroundVolume =
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.BackGroundVolume]);
        EffectVolume =
             PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.EffectVolume]);
        InputVolume =
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.InputVolume]);
        OutputVolume =
            PlayerPrefs.GetFloat(VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.OutputVolume]);
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