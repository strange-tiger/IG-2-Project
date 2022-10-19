using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

using Column = Asset.EaccountdbColumns;
using UI = Defines.ELogInUIIndex;
using Error = Defines.ELogInErrorType;
using Sql = Asset.MySql.MySqlSetting;
using Hash = Encryption.Hash256;

public class LogInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _logInButton;
    [SerializeField] Button _signInButton;
    [SerializeField] Button _changePasswordButton;
    [SerializeField] Button _quitButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _passwordInput;

    [Header("Popup")]
    [SerializeField] LogInErrorPopupUI _errorPopup;

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LogIn);
        _logInButton.onClick.AddListener(LogIn);
        
        _signInButton.onClick.RemoveListener(LoadSignIn);
        _signInButton.onClick.AddListener(LoadSignIn);
        
        _changePasswordButton.onClick.RemoveListener(LoadFind);
        _changePasswordButton.onClick.AddListener(LoadFind);
        
        _quitButton.onClick.RemoveListener(Quit);
        _quitButton.onClick.AddListener(Quit);

        Sql.Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _errorPopup.ErrorPopup(Error.ID);
        }
    }

    /// <summary>
    /// 입력된 계정 정보(Email, Password)를 계정 DB와 비교해
    /// 일치하면 다음 씬을 로드한다.
    /// </summary>
    private void LogIn()
    {
        if (!Sql.HasValue(Column.Email, _idInput.text))
        {
            _errorPopup.ErrorPopup(Error.ID);
            return;
        }

        if (!Sql.CheckValueByBase(Column.Email, _idInput.text, 
            Column.Password, Hash.Compute(_passwordInput.text)))
        {
            _errorPopup.ErrorPopup(Error.PASSWORD);
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
        _changePasswordButton.onClick.RemoveListener(LoadFind);
        _quitButton.onClick.RemoveListener(Quit);
    }
}
