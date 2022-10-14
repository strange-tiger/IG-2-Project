using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

using _UI = Defines.ECharacterUIIndex;

public class SelectCharacterUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] CharacterUIManager _characterUIManager;

    [Header("Button")]
    [SerializeField] Button _startButton;
    [SerializeField] Button _makeButton;
    [SerializeField] Button _deleteButton;

    [Header("Character")]
    [SerializeField] GameObject _maleCharacter;
    [SerializeField] GameObject _femaleCharacter;
    [SerializeField] GameObject _customizingCharacter;

    [Header("Popup")]
    [SerializeField] GameObject _deletePopup;

    private bool _isFemaleCharacter;

    private void OnEnable()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _makeButton.onClick.RemoveListener(LoadMake);
        _deleteButton.onClick.RemoveListener(PopupDelete);
        _startButton.onClick.AddListener(StartGame);
        _makeButton.onClick.AddListener(LoadMake);
        _deleteButton.onClick.AddListener(PopupDelete);

        // DB 연결 필요 (AccountInfoDB.CharacterDB.Gender 데이터 파싱)
        _isFemaleCharacter = true;
        SetGender(_isFemaleCharacter);

        _deletePopup.SetActive(false);
    }

    private void SetGender(bool isFemale)
    {
        _maleCharacter.SetActive(!isFemale);
        _femaleCharacter.SetActive(isFemale);
    }

    private void StartGame()
    {
        // PhotonNetwork.LoadLevel() // 다음 씬으로 이어지는 부분 필요
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
