using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] GameObject _errorPopup;

    private void OnEnable()
    {
        _logInButton.onClick.AddListener(LoadLogIn);
        _findPasswordButton.onClick.AddListener(FindPassword);
    }

    public void LoadLogIn() => _logInUIManager.LoadUI(LogInUIManager.ELogInUIIndex.LOGIN);
    
    public void FindPassword()
    {
        if (_emailInput.text != _emailInput.text) // DB 접근 필요
        {
            return;
        }
        if (_answerInput.text != _answerInput.text) // DB 접근 필요
        {
            return;
        }

        _passwordOutput.text = "비밀번호"; // DB 접근 필요
    }

    public void Quit() => Application.Quit();

    private void OnDisable()
    {
        _emailInput.text = "";
        _answerInput.text = "";
        _passwordOutput.text = "";
        
        _logInButton.onClick.RemoveListener(LoadLogIn);
        _findPasswordButton.onClick.RemoveListener(FindPassword);
    }
}
