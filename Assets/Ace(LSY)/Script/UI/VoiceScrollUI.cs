using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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