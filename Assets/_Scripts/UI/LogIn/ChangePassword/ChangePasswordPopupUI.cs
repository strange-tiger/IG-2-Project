using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Crypt = Encryption.Hash256;
using Sql = Asset.MySql.MySqlSetting;
using Account = Asset.EaccountdbColumns;

public class ChangePasswordPopupUI : PopupUI
{
    [SerializeField] Button _changeButton;
    
    [Header("Parent")]
    [SerializeField] ChangePasswordUI _ui;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _passwordCheckInput;

    [Header("Message")]
    [SerializeField] GameObject _passwordErrorMessage;
    [SerializeField] GameObject _passwordCheckErrorMessage;

    [Header("Popup")]
    [SerializeField] GameObject _successPopup;

    protected override void OnEnable()
    {
        base.OnEnable();
        _changeButton.onClick.RemoveListener(ChangePassword);
        _changeButton.onClick.AddListener(ChangePassword);
    }

    /// <summary>
    /// _passwordInput에 입력 받은 새 비밀번호가 이전 비밀번호와 일치하지 않고, 
    /// _passwordCheckInput에 입력 받은 비밀번호 확인 내용과 일치하는 지 비교 후 
    /// 두 조건을 모두 충족하면 DB에 새로운 비밀번호를 저장한다.
    /// </summary>
    private void ChangePassword()
    {
        SetErrorMessageDeactive();

        string hash = Crypt.Compute(_passwordInput.text);

        if (hash == Sql.GetValueByBase(Account.ID, _ui.GetID(), Account.Password))
        {
            _passwordErrorMessage.SetActive(true);
            return;
        }

        if (_passwordInput.text != _passwordCheckInput.text)
        {
            _passwordCheckErrorMessage.SetActive(true);
            return;
        }

        Sql.UpdateValueByBase(Account.ID, _ui.GetID(), Account.Password, hash);

        _successPopup.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 비밀번호 변경에서 에러 발생 시 활성화할 에러 피드백 텍스트들을 비활성화
    /// </summary>
    private void SetErrorMessageDeactive()
    {
        _passwordErrorMessage.SetActive(false);
        _passwordCheckErrorMessage.SetActive(false);
    }

    protected override void OnDisable()
    {
        _passwordInput.text = "";
        _passwordCheckInput.text = "";
        
        base.OnDisable();

        _changeButton.onClick.RemoveListener(ChangePassword);
    }
}
