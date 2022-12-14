using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePasswordSuccessPopupUI : PopupUI
{
    [Header("Parent")]
    [SerializeField] ChangePasswordUI _ui;

    /// <summary>
    /// 게임 오브젝트를 비활성화하고 ChangePasswordUI의 LoadLogin 함수를 호출, 
    /// 로그인 UI의 로그인 화면만 활성화한다.
    /// </summary>
    protected override void Close()
    {
        base.Close();
        _ui.LoadLogIn();
    }
}
