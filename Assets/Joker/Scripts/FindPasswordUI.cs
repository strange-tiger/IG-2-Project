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

    /// <summary>
    /// 로그인 UI를 로드
    /// </summary>
    private void LoadLogIn() => _logInUIManager.LoadUI(UI.LOGIN);

    /// <summary>
    /// Email과 Email의 계정 정보에 포함된 Question 인덱스와 Answer을 참조, 비교해
    /// 비밀번호 출력
    /// Email이 틀리면 Defines.EErrorType.EMAIL을 매개변수로 전달해 ErrorPopup 띄움
    /// Question 인덱스 및 Answer 틀리면 Defines.EErrorType.ANSWER 전달, 
    /// ErrorPopup 띄움
    /// </summary>
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
