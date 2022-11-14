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

    private void ChangePassword()
    {
        SetErrorMessageDeactive();

        string hash = Crypt.Compute(_passwordInput.text);

        if (hash == Sql.GetValueByBase(Account.Email, _ui.GetID(), Account.Password))
        {
            _passwordErrorMessage.SetActive(true);
            return;
        }

        if (_passwordInput.text != _passwordCheckInput.text)
        {
            _passwordCheckErrorMessage.SetActive(true);
            return;
        }

        Sql.UpdateValueByBase(Account.Email, _ui.GetID(), Account.Password, hash);

        _successPopup.SetActive(true);
        gameObject.SetActive(false);
    }

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
