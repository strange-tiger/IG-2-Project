﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VRKeys;
using _UI = Defines.ELogInUIIndex;

public class LogInUIManager : UIManager
{
    [SerializeField] Keyboard _keyboard;

    [SerializeField] TMP_InputField[] _inputFields;

    private TMP_InputField _selectedInputField;
    private void Awake()
    {
        LoadUI(_UI.LOGIN);

        foreach(TMP_InputField input in _inputFields)
        {
            input.onSelect.AddListener((string temp) =>
            {
                ActivateKeyboard(input);
            });
        }
    }
    
    /// <summary>
    /// ELogInUIIndex를 ui 매개변수로 받아, UIManager.LoadUI에 전달해 
    /// UI 오브젝트를 모두 비활성화한 후 인덱스에 해당하는 UI 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="ui"></param>
    public void LoadUI(_UI ui)
    {
        LoadUI((int)ui);
    }

    private void ActivateKeyboard(TMP_InputField inputField)
    {
        _keyboard.Enable();

        _selectedInputField = inputField;
        _keyboard.OnSubmit.RemoveListener(SendText);
        _keyboard.OnSubmit.AddListener(SendText);
    }

    private void SendText(string message)
    {
        _selectedInputField.text = message;
    }

    private void OnDisable()
    {
        _keyboard.Disable();
    }
}