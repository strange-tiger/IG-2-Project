using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : InteracterableObject
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = false;
    }

    public override void OnFocus()
    {
        base.OnFocus();
        _button.interactable = true;
    }

    public override void OutFocus()
    {
        base.OutFocus();
        _button.interactable = false;
    }

    public override void Interact()
    {
        _button.onClick.Invoke();
    }
}
