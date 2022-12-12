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
using UnityEngine.EventSystems;

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
    [SerializeField] QuestionList _questionList;

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

    /// <summary>
    /// 비밀번호 변경 UI에 속한 팝업을 모두 비활성화한다.
    /// </summary>
    private void DeactivePopup()
    {
        _errorPopup.gameObject.SetActive(false);
        _changePopup.gameObject.SetActive(false);
        _successPopup.gameObject.SetActive(false);
    }

    /// <summary>
    /// 로그인 UI 로드
    /// </summary>
    public void LoadLogIn() => _logInUIManager.LoadUI(UI.LOGIN);

    /// <summary>
    /// ID 입력과 문답 내용이 모두 DB의 정보와 일치하는지 비교하고 전부 일치하면 
    /// 비밀번호 변경을 위한 팝업 _changePopup을 활성화한다.
    /// 일치하지 않으면 유저에게 팝업으로 피드백한다.
    /// </summary>
    private void ChangePassword()
    {
        if (!Sql.HasValue(Column.ID, _idInput.text))
        {
            _errorPopup.ErrorPopup(Error.ID);
            return;
        }
        if (!Sql.CheckValueByBase(Column.ID, _idInput.text, Column.Question, _questionList.Value.ToString()) ||
            !Sql.CheckValueByBase(Column.ID, _idInput.text, Column.Answer, _answerInput.text))
        {
            _errorPopup.ErrorPopup(Error.ANSWER);
            return;
        }

        _changePopup.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// _idInput에 입력된 텍스트를 전달한다.
    /// </summary>
    /// <returns></returns>
    public string GetID()
    {
        return _idInput.text;
    }

    private void OnDisable()
    {
        _idInput.text = "";
        _answerInput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _changePasswordButton.onClick.RemoveListener(ChangePassword);
    }
}
