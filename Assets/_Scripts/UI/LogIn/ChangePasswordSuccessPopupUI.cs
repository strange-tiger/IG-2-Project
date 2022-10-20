using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePasswordSuccessPopupUI : PopupUI
{
    [Header("Parent")]
    [SerializeField] ChangePasswordUI _ui;

    protected override void Close()
    {
        base.Close();
        _ui.LoadLogin();
    }
}
