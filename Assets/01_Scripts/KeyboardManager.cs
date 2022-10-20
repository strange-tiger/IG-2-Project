using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KeyboardManager : GlobalInstance<KeyboardManager>
{
    public enum EKeyboardLayout
    {
        QWERTY,
        QWERTY_SHIFTED,
        KOREAN,
        KOREAN_SHIFTED,
        NUMPAD,
        MAX
    }

    private static TMP_InputField _inputField;
    private static EKeyboardLayout _currentLayout;

    private static TMP_InputField _typedText;
    private static GameObject[] _layouts = new GameObject[(int)EKeyboardLayout.MAX];

    private void Start()
    {
        _typedText = transform.GetChild(0).GetComponent<TMP_InputField>();
        _typedText.gameObject.SetActive(false);

        int j;
        for (int i = 1; i < transform.childCount; ++i)
        {
            j = i - 1;
            _layouts[j] = transform.GetChild(i).gameObject;
            Debug.Log(_layouts[j].name);
            _layouts[j].SetActive(false);
        }
    }

    public static void OpenKeyboard()
    {
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        _typedText.gameObject.SetActive(true);

        ChangeLayout(EKeyboardLayout.QWERTY);
    }

    public static void OpenKeyboard(EKeyboardLayout type)
    {
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        _typedText.gameObject.SetActive(true);

        ChangeLayout(type);
    }

    private static void ChangeLayout(EKeyboardLayout type)
    {
        CloseLayout();

        _currentLayout = type;
        _layouts[(int)_currentLayout].SetActive(true);
    }

    public static void CloseKeyboard()
    {
        CloseLayout();

        _inputField = null;
        _typedText.gameObject.SetActive(false);
    }

    private static void CloseLayout()
    {
        foreach (GameObject layout in _layouts)
        {
            layout.SetActive(false);
        }
    }

    public static void PressKey()
    {
        if (EventSystem.current.alreadySelecting) return;

        _typedText.text += EventSystem.current.currentSelectedGameObject.name;
    }

    public static void PressSpace()
    {
        if (EventSystem.current.alreadySelecting) return;

        _typedText.text += " ";
    }

    public static void PressBackspace()
    {
        if (_typedText.text.Length == 0) return;

        _typedText.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
    }

    public static void Clear()
    {
        if (_typedText.text.Length == 0) return;

        _typedText.text = "";
    }

    public static void Shift()
    {
        if (_currentLayout == EKeyboardLayout.QWERTY
            || _currentLayout == EKeyboardLayout.KOREAN)
        {
            ChangeLayout(_currentLayout + 1);
        }
        else if (_currentLayout == EKeyboardLayout.QWERTY_SHIFTED
            || _currentLayout == EKeyboardLayout.KOREAN_SHIFTED)
        {
            ChangeLayout(_currentLayout - 1);
        }
    }

    public static void Submit()
    {
        _inputField.text = _typedText.text;
        CloseKeyboard();
    }

    public static void ChangeLanguage()
    {
        if (_currentLayout == EKeyboardLayout.QWERTY
            || _currentLayout == EKeyboardLayout.QWERTY_SHIFTED)
        {
            ChangeLayout(EKeyboardLayout.KOREAN);
        }
        else if (_currentLayout == EKeyboardLayout.KOREAN
            || _currentLayout == EKeyboardLayout.KOREAN_SHIFTED)
        {
            ChangeLayout(EKeyboardLayout.QWERTY);
        }
    }
}
