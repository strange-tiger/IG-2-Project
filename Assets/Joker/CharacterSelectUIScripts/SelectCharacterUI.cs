using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using _UI = Defines.ECharacterUIIndex;

public class SelectCharacterUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] CharacterUIManager _characterUIManager;

    [Header("Button")]
    [SerializeField] Button _startButton;
    [SerializeField] Button _makeButton;
    [SerializeField] Button _deleteButton;

    [Header("Popup")]
    [SerializeField] GameObject _deletePopup;

    private void OnEnable()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _makeButton.onClick.RemoveListener(LoadMake);
        _deleteButton.onClick.RemoveListener(PopupDelete);
        _startButton.onClick.AddListener(StartGame);
        _makeButton.onClick.AddListener(LoadMake);
        _deleteButton.onClick.AddListener(PopupDelete);

        _deletePopup.SetActive(false);
    }

    private void StartGame()
    {
        //SceneManager.LoadScene(2);
    }

    private void LoadMake() => _characterUIManager.LoadUI(_UI.MAKE);
    private void PopupDelete() => _deletePopup.SetActive(true);

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _makeButton.onClick.RemoveListener(LoadMake);
        _deleteButton.onClick.RemoveListener(PopupDelete);
    }
}
