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

    // 입력된 계정 정보를 계정 DB와 비교해 일치하면 다음 씬을 로드한다.
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

        SceneManager.LoadScene(1); // 다음 씬으로 이어지는 부분 필요
    }

    private void LoadSignIn() => _logInUIManager.LoadUI(UI.SIGNIN);
    private void LoadFind() => _logInUIManager.LoadUI(UI.FINDPASSWORD);
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
