using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Asset.MySql;

using Column = Asset.MySql.EAccountColumns;
using UI = Defines.ELogInUIIndex;

public class LogInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _signInButton;
    [SerializeField] Button _findPasswordButton;
    [SerializeField] Button _quitButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _emailInput;
    [SerializeField] TMP_InputField _passwordInput;

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findPasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
        _logInButton.onClick.AddListener(LogIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        _findPasswordButton.onClick.AddListener(LoadFind);
        _quitButton.onClick.AddListener(Quit);
    }

    /// <summary>
    /// 입력된 계정 정보(Email, Password)를 계정 DB와 비교해
    /// 일치하면 다음 씬을 로드한다.
    /// </summary>
    private void LogIn()
    {
        if (!MySqlSetting.HasValue(Column.Email, _emailInput.text))
        {
            return;
        }

        if (!MySqlSetting.CheckValueByBase(Column.Email, _emailInput.text, Column.Password, _passwordInput.text))
        {
            return;
        }

        Debug.Log("로그인 성공!");
        // SceneManager.LoadScene(1); // 다음 씬으로 이어지는 부분 필요
    }

    /// <summary>
    /// 회원가입 UI 로드
    /// </summary>
    private void LoadSignIn() => _logInUIManager.LoadUI(UI.SIGNIN);

    /// <summary>
    /// 비밀번호 찾기 UI 로드
    /// </summary>
    private void LoadFind() => _logInUIManager.LoadUI(UI.FINDPASSWORD);

    /// <summary>
    /// 게임 종료
    /// </summary>
    private void Quit() => Application.Quit();

    private void OnDisable()
    {
        _emailInput.text = "";
        _passwordInput.text = "";
        
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findPasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
    }
}
