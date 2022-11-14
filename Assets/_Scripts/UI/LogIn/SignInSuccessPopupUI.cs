using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInSuccessPopupUI : PopupUI
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    protected override void OnDisable()
    {
        base.OnDisable();

        _logInUIManager.LoadUI(Defines.ELogInUIIndex.LOGIN);
    }
}
