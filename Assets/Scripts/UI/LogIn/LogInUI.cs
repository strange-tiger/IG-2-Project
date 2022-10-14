using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

using Column = Asset.MySql.EAccountColumns;
using UI = Defines.ELogInUIIndex;
using Sql = Asset.MySql.MySqlSetting;
using Hash = Encryption.Hash256;

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
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _passwordInput;

    [Header("Popup")]
    [SerializeField] LogInErrorPopupUI _errorPopup;

    public Defines.ELogInErrorType ErrorType { get; private set; }

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
        if (!Sql.HasValue(Column.Email, _idInput.text))
        {
            return;
        }

        if (!Sql.CheckValueByBase(Column.Email, _idInput.text, 
            Column.Password, Hash.Compute(_passwordInput.text)))
        {
            return;
        }

        TempAccountDB.SetAccountData(_idInput.text, Sql.GetValueByBase(Column.Email, _idInput.text, Column.Nickname));
        Debug.Log("로그인 성공!");
        // PhotonNetwork.LoadLevel() // 다음 씬으로 이어지는 부분 필요
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
        _idInput.text = "";
        _passwordInput.text = "";
        
        _logInButton.onClick.RemoveListener(LogIn);
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _findPasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
    }
}
