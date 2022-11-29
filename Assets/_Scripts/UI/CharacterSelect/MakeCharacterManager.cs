using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Asset.MySql;
using UnityEngine.SceneManagement;

using SceneType = Defines.ESceneNumber;

public class MakeCharacterManager : MonoBehaviour
{
    public UnityEvent OnClickFemaleButton = new UnityEvent();

    [Header("Player")]
    [SerializeField] private PlayerCustomize _playerCustomize;

    [Header("UI")]
    [SerializeField] private Button _maleSelectButton;
    [SerializeField] private Button _femaleSelectButton;
    [SerializeField] private Button _makeCharacterButton;

    [Header("Block Raycast")]
    [SerializeField] private GameObject _femalePanel;
    [SerializeField] private GameObject _malePanel;
    [SerializeField] private GameObject _rayPlane;

    private void Start()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _maleSelectButton.onClick.AddListener(SelectMale);

        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _femaleSelectButton.onClick.AddListener(SelectFemale);

        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);
        _makeCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void SelectMale()
    {
        _playerCustomize.IsFemale = false;
        _playerCustomize.MakeAvatarData();
        _malePanel.SetActive(true);
        _femalePanel.SetActive(false);
    }

    private void SelectFemale()
    {
        OnClickFemaleButton.Invoke();
        _playerCustomize.IsFemale = true;
        _playerCustomize.MakeAvatarData();
        _malePanel.SetActive(false);
        _femalePanel.SetActive(true);
        _rayPlane.SetActive(false);
    }

    private void CreateCharacter()
    {
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, TempAccountDB.Nickname, Asset.EaccountdbColumns.HaveCharacter, "1");
        MySqlSetting.AddNewCharacter(TempAccountDB.Nickname, $"{Convert.ToInt32(_playerCustomize.IsFemale)}");
        MySqlSetting.AddNewPetInventory(TempAccountDB.Nickname);
        SceneManager.LoadScene((int)SceneType.StartRoom);
    }

    private void OnDisable()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);
    }
}
