using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollButton : MonoBehaviour
{
    //[SerializeField]
    //private Recorder photonVoice;
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
        Type = Defines.EVoiceType.None;

        _voiceTable.Add(Defines.EVoiceType.None, VoiceTypeNone);
        _voiceTable.Add(Defines.EVoiceType.Always, VoiceTypeAlways);
        _voiceTable.Add(Defines.EVoiceType.PushToTalk, VoiceTypePushToTalk);
    }

    public void OnClickLeftButton()
    {
        if (Type - 1 < Defines.EVoiceType.None)
        {
            return;
        }
        --Type;
        _voiceTable[Type].Invoke();
    }
    public void OnClickRightButton()
    {
        if (Type + 1 >= Defines.EVoiceType.End)
        {
            return;
        }
        ++Type;
        _voiceTable[Type].Invoke();
    }


    private void VoiceTypeNone()
    {
        // photonVoice.TransmitEnabled = false;
    }
    private void VoiceTypeAlways()
    {
        // photonVoice.TransmitEnabled = ture;
    }
    private void VoiceTypePushToTalk()
    {
        // if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        // {
        //     //photonVoice.TransmitEnabled = true;
        // }
    }
}