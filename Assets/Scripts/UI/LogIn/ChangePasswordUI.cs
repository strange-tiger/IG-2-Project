using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Column = Asset.EaccountdbColumns;
using UI = Defines.ELogInUIIndex;
using Error = Defines.EChangePasswordErrorType;
using Sql = Asset.MySql.MySqlSetting;

public class ChangePasswordUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _changePasswordButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _answerInput;
    [SerializeField] TMP_Dropdown _questionList;

    [Header("Popup")]
    [SerializeField] ChangePasswordErrorPopupUI _errorPopup;
    [SerializeField] GameObject _changePopup;
    [SerializeField] GameObject _successPopup;

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _logInButton.onClick.AddListener(LoadLogIn);
        
        _changePasswordButton.onClick.RemoveListener(ChangePassword);
        _changePasswordButton.onClick.AddListener(ChangePassword);

        DeactivePopup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _successPopup.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _changePopup.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _errorPopup.ErrorPopup(Error.ID);
        }
    }

    private void DeactivePopup()
    {
        _errorPopup.gameObject.SetActive(false);
        _changePopup.gameObject.SetActive(false);
        _successPopup.gameObject.SetActive(false);
    }

    private void LoadLogIn() => _logInUIManager.LoadUI(UI.LOGIN);

    private void ChangePassword()
    {
        if (!Sql.HasValue(Column.Email, _idInput.text))
        {
            _errorPopup.ErrorPopup(Error.ID);
            return;
        }
        if (Sql.CheckValueByBase(Column.Email, _idInput.text, Column.Question, _questionList.value.ToString()) ||
            Sql.CheckValueByBase(Column.Email, _idInput.text, Column.Answer, _answerInput.text))
        {
            _errorPopup.ErrorPopup(Error.ANSWER);
            return;
        }

        _changePopup.SetActive(true);
    }

    public string GetID()
    {
        return _idInput.text;
    }

    public void LoadLogin()
    {
        _logInUIManager.LoadUI(UI.LOGIN);
    }

    private void OnDisable()
    {
        _idInput.text = "";
        _answerInput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _changePasswordButton.onClick.RemoveListener(ChangePassword);
    }
}
