using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Column = Asset.EaccountdbColumns;
using UI = Defines.ELogInUIIndex;
using Sql = Asset.MySql.MySqlSetting;
using Hash = Encryption.Hash256;
using UnityEngine.EventSystems;

public class SignInUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] LogInUIManager _logInUIManager;

    [Header("Button")]
    [SerializeField] Button _signInButton;
    [SerializeField] Button _idDoubleCheckButton;
    [SerializeField] Button _passwordDoubleCheckButton;
    [SerializeField] Button _nicknameDoubleCheckButton;
    [SerializeField] Button _logInButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _idInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _passwordCheckInput;
    [SerializeField] TMP_InputField _nicknameInput;
    [SerializeField] TMP_InputField _answerInput;

    [Space(15)]
    [SerializeField] TMP_Dropdown _questionList;

    [Header("Error Text")]
    [SerializeField] GameObject _idErrorText;
    [SerializeField] GameObject _passwordErrorText;
    [SerializeField] GameObject _nicknameErrorText;

    [Header("Popup")]
    [SerializeField] GameObject _successPopup;

    private bool _hasNicknameCheck;
    private bool _hasIdCheck;
    private bool _hasPasswordCheck;

    private void OnEnable()
    {
        _signInButton.onClick.RemoveListener(SignIn);
        _signInButton.onClick.AddListener(SignIn);
        
        _passwordDoubleCheckButton.onClick.RemoveListener(PasswordDoubleCheck);
        _passwordDoubleCheckButton.onClick.AddListener(PasswordDoubleCheck);
        
        _nicknameDoubleCheckButton.onClick.RemoveListener(NicknameDoubleCheck);
        _nicknameDoubleCheckButton.onClick.AddListener(NicknameDoubleCheck);
        
        _idDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);
        _idDoubleCheckButton.onClick.AddListener(EmailDoubleCheck);

        _logInButton.onClick.RemoveListener(LoadLogIn);
        _logInButton.onClick.AddListener(LoadLogIn);

        _nicknameErrorText?.SetActive(false);
        _passwordErrorText?.SetActive(false);
        _idErrorText?.SetActive(false);

        _successPopup.SetActive(false);

        _hasIdCheck = false;
        _hasPasswordCheck = false;
        _hasNicknameCheck = false;
    }

    /// <summary>
    /// �Էµ� ���� ����(Email, Password, Nickname)�� ������
    /// �� ������ �ߺ�üũ�� �Ϸ�Ǿ��ٸ� ���� DB�� �����Ѵ�.
    /// </summary>
    private void SignIn()
    {
        if (!_hasIdCheck
            || !_hasPasswordCheck
            || !_hasNicknameCheck)
        {
            return;
        }

        Debug.Assert
        (
            Sql.AddNewAccount
            (
                _idInput.text,
                Hash.Compute(_passwordInput.text),
                _nicknameInput.text,
                _questionList.value,
                _answerInput.text
            ),
            "���� ���� ����!"
        );

        _successPopup.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// �Էµ� Email ������ DB�� ���� �ߺ�üũ
    /// </summary>
    private void EmailDoubleCheck()
    {
        if (!Sql.HasValue(Column.Email, _idInput.text))
        {
            _hasIdCheck = true;
            _idErrorText.SetActive(false);
        }
        else
        {
            _hasIdCheck = false;
            _idErrorText.SetActive(true);
        }
    }

    /// <summary>
    /// �Էµ� ��й�ȣ�� ��й�ȣ üũ�� �Է��� ���� ��ġ�ϴ��� Ȯ��
    /// </summary>
    private void PasswordDoubleCheck()
    {
        if (_passwordInput.text == _passwordCheckInput.text)
        {
            _hasPasswordCheck = true;
            _passwordErrorText.SetActive(false);
        }
        else
        {
            _hasPasswordCheck = false;
            _passwordErrorText.SetActive(true);
        }
    }

    /// <summary>
    /// �Էµ� Nickname ������ DB�� ���� �ߺ�üũ
    /// </summary>
    private void NicknameDoubleCheck()
    {
        if (!Sql.HasValue(Column.Nickname, _nicknameInput.text))
        {
            _hasNicknameCheck = true;
            _nicknameErrorText.SetActive(false);
        }
        else
        {
            _hasNicknameCheck = false;
            _nicknameErrorText.SetActive(true);
        }
    }

    /// <summary>
    /// ȸ������ UI �ε�
    /// </summary>
    private void LoadLogIn() => _logInUIManager.LoadUI(UI.LOGIN);

    private void OnDisable()
    {
        _nicknameInput.text = "";
        _passwordInput.text = "";
        _passwordCheckInput.text = "";
        _idInput.text = "";
        _answerInput.text = "";

        _signInButton.onClick.RemoveListener(SignIn);
        _passwordDoubleCheckButton.onClick.RemoveListener(PasswordDoubleCheck);
        _nicknameDoubleCheckButton.onClick.RemoveListener(NicknameDoubleCheck);
        _idDoubleCheckButton.onClick.RemoveListener(EmailDoubleCheck);
        _logInButton.onClick.RemoveListener(LoadLogIn);
    }
}
