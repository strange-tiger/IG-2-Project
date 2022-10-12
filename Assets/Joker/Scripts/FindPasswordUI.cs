using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

using Column = Asset.MySql.EAccountColumns;
using UI = Defines.ELogInUIIndex;
using Error = Defines.EErrorType;

public class FindPasswordUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _findPasswordButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _emailInput;
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

    private void FindPassword()
    {
        if (!MySqlSetting.HasValue(Column.Email, _emailInput.text))
        {
            _errorPopup.ErrorPopup(Error.EMAIL);
            return;
        }
        if (MySqlSetting.CheckValueByBase(Column.Email, _emailInput.text, Column.Qustion, _questionList.value.ToString()) || 
            MySqlSetting.CheckValueByBase(Column.Email, _emailInput.text, Column.Answer, _answerInput.text))
        {
            _errorPopup.ErrorPopup(Error.ANSWER);
            return;
        }

        _passwordOutput.text = MySqlSetting.GetValueByBase(Column.Email, _emailInput.text, Column.Password);
    }

    private void OnDisable()
    {
        _emailInput.text = "";
        _answerInput.text = "";
        _passwordOutput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
    }
}
