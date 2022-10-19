using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KeyboardManager : SingletonBehaviour<KeyboardManager>
{
    TMP_InputField inputField;

    [SerializeField] GameObject qwerty;
    [SerializeField] GameObject numpad;

    public void OpenKeyboard(int _type)
    {
        CloseKeyboard();

        inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

        switch (_type)
        {
            case 0: // qwerty + numpad
                qwerty.SetActive(true);
                numpad.SetActive(true);
                break;
            case 1: // only numpad
                numpad.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CloseKeyboard()
    {
        qwerty.SetActive(false);
        numpad.SetActive(false);
    }

    public void PressKey()
    {
        inputField.text += EventSystem.current.currentSelectedGameObject.name;
    }

    public void PressBackspace()
    {
        if (inputField.text.Length == 0) return;
        inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }
}
