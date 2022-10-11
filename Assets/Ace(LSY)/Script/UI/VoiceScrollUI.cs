using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using TMPro;

public class VoiceScrollUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI voiceUIText;

    public void TextOutput(Defines.EVoiceType voiceType)
    {
        voiceUIText.text = voiceType.ToString();
    }
}

    //    switch(_voiceType)
    //    {
    //        case Defines.EVoiceType.None:
    //            voiceUIText.text = "None";
    //            // photonVoice.TransmitEnabled = false;
    //            break;
    //        case Defines.EVoiceType.Always:
    //            voiceUIText.text = "Always";
    //            // photonVoice.TransmitEnabled = ture;
    //            break;
    //        case Defines.EVoiceType.PushToTalk:
    //            voiceUIText.text = "PushToTalk";
    //            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
    //            {
    //                //photonVoice.TransmitEnabled = true;
    //            }
    //            break;
    //        default:
    //            Debug.LogError($"정의되지 않은 voiceType : {_voiceType}");
    //            break;
    //    }