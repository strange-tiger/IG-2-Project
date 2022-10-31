using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Voice.Unity;

public class ScrollButton : MonoBehaviour
{
    public Recorder PhotonVoice { get; set; }
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
        _voiceTable[Type].Invoke();
    }

    public void OnClickLeftButton()
    {
        if (Type - 1 < Defines.EVoiceType.None)
        {
            Type = Defines.EVoiceType.MaxCount;
            //return;
        }
        --Type;
        _voiceTable[Type].Invoke();
    }
    public void OnClickRightButton()
    {
        if (Type + 1 >= Defines.EVoiceType.MaxCount)
        {
            Type = (Defines.EVoiceType)(-1);
            //return;
        }
        ++Type;
        _voiceTable[Type].Invoke();
    }


    private void VoiceTypeNone()
    {
        PhotonVoice.TransmitEnabled = false;
    }
    private void VoiceTypeAlways()
    {
        PhotonVoice.TransmitEnabled = true;
    }
    private void VoiceTypePushToTalk()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
             PhotonVoice.TransmitEnabled = true;
        }
    }
}