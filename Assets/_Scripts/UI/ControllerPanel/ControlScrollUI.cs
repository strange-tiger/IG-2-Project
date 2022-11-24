using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ControlScrollUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI controlUIText;

    private bool _isControllerRight;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("WhatIsTheMainController"))
        {
            _isControllerRight = Convert.ToBoolean(PlayerPrefs.GetInt("WhatIsTheMainController"));

            if (_isControllerRight)
            {
                controlUIText.text = Defines.ESwitchController.Right.ToString();
            }
            else
            {
                controlUIText.text = Defines.ESwitchController.Left.ToString();
            }
        }
    }

    public void TextOutput(Defines.ESwitchController controlType)
    {
        controlUIText.text = controlType.ToString();
    }
}