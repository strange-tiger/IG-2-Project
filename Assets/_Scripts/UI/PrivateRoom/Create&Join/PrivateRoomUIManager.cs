using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using _UI = Defines.EPrivateRoomUIIndex;

public class PrivateRoomUIManager : UIManager
{
    [SerializeField] TMP_InputField[] _inputFields;
    [SerializeField] TMP_InputField[] _inputNumField;
    [SerializeField] Collider _npcCollider;

    private void Awake()
    {
        ShutUIPrivateRoom();

        foreach (TMP_InputField input in _inputFields)
        {
            input.onSelect.AddListener((string temp) =>
            {
                KeyboardManager.OpenKeyboard();
            });
        }

        foreach (TMP_InputField input in _inputNumField)
        {
            input.onSelect.AddListener((string temp) =>
            {
                KeyboardManager.OpenKeyboard(KeyboardManager.EKeyboardLayout.NUMPAD);
            });
        }
    }

    /// <summary>
    /// EPrivateRoomUIIndex를 ui 매개변수로 받아, UIManager.LoadUI에 전달해 
    /// UI 오브젝트를 모두 비활성화한 후 인덱스에 해당하는 UI 오브젝트를 활성화한다.
    /// </summary>
    /// <param name="ui"></param>
    public void LoadUI(_UI ui)
    {
        _npcCollider.enabled = false;

        LoadUI((int)ui);
    }

    public void ShutUIPrivateRoom()
    {
        ShutUI();

        _npcCollider.enabled = true;
    }
}
