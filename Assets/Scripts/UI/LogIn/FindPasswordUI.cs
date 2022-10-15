using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Column = Asset.EaccountdbColumns;
using UI = Defines.ELogInUIIndex;
using Error = Defines.EFindPasswordErrorType;
using Sql = Asset.MySql.MySqlSetting;

public class FindPasswordUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _findPasswordButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _answerInput;
    [SerializeField] TMP_Dropdown _questionList;
    [SerializeField] TMP_InputField _passwordOutput;

    [Header("Popup")]
    [SerializeField] FindPasswordErrorPopupUI _errorPopup;

    public Error ErrorType { get; private set; }

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
        
        _logInButton.onClick.AddListener(LoadLogIn);
        _findPasswordButton.onClick.AddListener(FindPassword);

        _errorPopup.gameObject.SetActive(false);
    }

    private void LoadLogIn() => _logInUIManager.LoadUI(UI.LOGIN);

    [Obsolete]
    private void FindPassword()
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

        _passwordOutput.text = Sql.GetValueByBase(Column.Email, _idInput.text, Column.Password);
    }

    private void OnDisable()
    {
        _idInput.text = "";
        _answerInput.text = "";
        _passwordOutput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
    }
}
