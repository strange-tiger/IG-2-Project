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
    /// EPrivateRoomUIIndex�� ui �Ű������� �޾�, UIManager.LoadUI�� ������ 
    /// UI ������Ʈ�� ��� ��Ȱ��ȭ�� �� �ε����� �ش��ϴ� UI ������Ʈ�� Ȱ��ȭ�Ѵ�.
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
