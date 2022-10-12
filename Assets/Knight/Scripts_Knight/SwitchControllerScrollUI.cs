using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using TMPro;

public class SwitchControllerScrollUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _switchControllerText;

    public void TextOutput(Defines.ESwitchController controllerType)
    {
        _switchControllerText.text = controllerType.ToString();
    }
}
