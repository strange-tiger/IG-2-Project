﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

using Column = Asset.EaccountdbColumns;
using UI = Defines.ELogInUIIndex;
using Error = Defines.ELogInErrorType;
using Scene = Defines.ESceneNumber;
using Sql = Asset.MySql.MySqlSetting;
using Hash = Encryption.Hash256;

public class LogInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;
    [SerializeField] LogInServerManager _logInServerManager;

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

    private const bool IS_ONLINE = true;
    
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

    /// <summary>
    /// 입력된 계정 정보(Email, Password)를 계정 DB와 비교해
    /// 일치하면 다음 씬을 로드한다.
    /// </summary>
    private void LogIn()
    {
        if (!Sql.HasValue(Column.ID, _idInput.text))
        {
            _errorPopup.ErrorPopup(Error.ID);
            return;
        }

        if (!Sql.CheckValueByBase(Column.ID, _idInput.text, 
            Column.Password, Hash.Compute(_passwordInput.text)))
        {
            _errorPopup.ErrorPopup(Error.PASSWORD);
            return;
        }

        if (IS_ONLINE == bool.Parse(Sql.GetValueByBase(Column.ID, _idInput.text,
            Column.IsOnline)))
        {
            _errorPopup.ErrorPopup(Error.DUPLICATED);
            return;
        }

        TempAccountDB.SetAccountData(_idInput.text, Sql.GetValueByBase(Column.ID, _idInput.text, Column.Nickname));
        
        _logInServerManager.LogIn();
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
