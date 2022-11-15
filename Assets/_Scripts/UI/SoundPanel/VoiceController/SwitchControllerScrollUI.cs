using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchControllerScrollUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _switchControllerScrollUI;

    public void TextOutput(Defines.ESwitchController switchController)
    {
        _switchControllerScrollUI.text = switchController.ToString();
    }
}
