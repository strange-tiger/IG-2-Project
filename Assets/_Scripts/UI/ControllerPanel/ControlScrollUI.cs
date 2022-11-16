using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlScrollUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI controlUIText;

    public void TextOutput(Defines.ESwitchController controlType)
    {
        controlUIText.text = controlType.ToString();
    }
}