using System;
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

    private void Awake()
    {
        LoadUI(_UI.LOGIN);

        foreach(TMP_InputField input in _inputFields)
        {
            input.onSelect.RemoveListener(ActiveKeyboard);
            input.onSelect.AddListener(ActiveKeyboard);
        }
    }

    // _keyboard 활성화 테스트
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        _inputFields[0].onSelect.Invoke("aa");
    //    }

    //    if (Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        _keyboard.Disable();
    //    }
    //}
    
    /// <summary>
    /// ELogInUIIndex를 ui 매개변수로 받아, UIManager.LoadUI에 전달해 
    /// UI 오브젝트를 모두 비활성화한 후 인덱스에 해당하는 UI 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="ui"></param>
    public void LoadUI(_UI ui)
    {
        LoadUI((int)ui);
    }

    private void ActiveKeyboard(string temp)
    {
        _keyboard.Enable();
    }

    private void OnDisable()
    {
        foreach (TMP_InputField input in _inputFields)
        {
            input.onSelect.RemoveListener(ActiveKeyboard);
        }
        _keyboard.Disable();
    }
}
