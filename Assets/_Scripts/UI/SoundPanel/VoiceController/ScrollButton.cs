using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollButton : MonoBehaviour
{
    private VoiceScrollUI _voiceScrollUI;

    private Dictionary<Defines.EVoiceType, VoiceTypeDelegate> _voiceTable =
        new Dictionary<Defines.EVoiceType, VoiceTypeDelegate>();
    private Defines.EVoiceType _type = Defines.EVoiceType.None;
    public Defines.EVoiceType Type
    {
        get
        {
            return _type;
        }
        private set
        {
            _type = value;
            _voiceScrollUI.TextOutput(_type);
        }
    }

    private delegate void VoiceTypeDelegate();

    void Awake()
    {
        _voiceScrollUI = GetComponentInParent<VoiceScrollUI>();

        _voiceTable.Add(Defines.EVoiceType.None, VoiceTypeNone);
        _voiceTable.Add(Defines.EVoiceType.Always, VoiceTypeAlways);
        _voiceTable.Add(Defines.EVoiceType.PushToTalk, VoiceTypePushToTalk);

        Type = Defines.EVoiceType.None;
        VoiceTypeNone();
    }

    public void OnClickLeftButton()
    {
        if (Type == Defines.EVoiceType.None)
        {
            return;
        }
        --Type;
        _voiceTable[Type]?.Invoke();
    }
    public void OnClickRightButton()
    {
        if (Type == Defines.EVoiceType.MaxCount - 1)
        {
            return;
        }
        ++Type;
        _voiceTable[Type]?.Invoke();
    }

    private void VoiceTypeNone()
    {
        SoundManager.Instance.LobbyRecorder.TransmitEnabled = false;
        SoundManager.Instance.IsPushToTalk = false;
    }
    private void VoiceTypeAlways()
    {
        SoundManager.Instance.LobbyRecorder.TransmitEnabled = true;
        SoundManager.Instance.IsPushToTalk = false;
    }

    private void VoiceTypePushToTalk()
    {
        SoundManager.Instance.LobbyRecorder.TransmitEnabled = false;
        SoundManager.Instance.IsPushToTalk = true;
    }
}