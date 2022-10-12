using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollButton : MonoBehaviour
{
    private VoiceScrollUI _voiceScrollUI;
    private Defines.EVoiceType _type = Defines.EVoiceType.None;
    private Dictionary<Defines.EVoiceType, VoiceTypeDelegate> _voiceTable =
        new Dictionary<Defines.EVoiceType, VoiceTypeDelegate>();
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

    public void OnClickRightButton()
    {
        if (Type == Defines.EVoiceType.End - 1)
        {
            return;
        }
        ++Type;
        _voiceTable[Type].Invoke();
    }

    public void OnClickLeftButton()
    {
        if(Type == Defines.EVoiceType.None)
        {
            return;
        }
        --Type;
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