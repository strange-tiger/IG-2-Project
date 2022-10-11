using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

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
    [SerializeField] TMP_InputField _passwordOutput;

    [Header("Popup")]
    [SerializeField] FindPasswordErrorPopupUI _errorPopup;

    public Defines.EErrorType ErrorType { get; private set; }

    private void OnEnable()
    {
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
        _logInButton.onClick.AddListener(LoadLogIn);
        _findPasswordButton.onClick.AddListener(FindPassword);

        _errorPopup.gameObject.SetActive(false);
    }

    private void LoadLogIn() => _logInUIManager.LoadUI(Defines.ELogInUIIndex.LOGIN);

    private void FindPassword()
    {
        if (!MySqlSetting.HasValue(EAccountColumns.Email, _emailInput.text))
        {
            _errorPopup.ErrorPopup(Defines.EErrorType.EMAIL);
            return;
        }
        if (_answerInput.text != _answerInput.text)
            // DB 접근 필요 // Answer Column 필요
        {
            _errorPopup.ErrorPopup(Defines.EErrorType.ANSWER);
            return;
        }

        _passwordOutput.text = MySqlSetting.GetValueByBase(EAccountColumns.Email, _emailInput.text, EAccountColumns.Password);
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
