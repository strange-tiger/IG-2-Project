using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Asset.MySql;
using UnityEngine.SceneManagement;

using SceneType = Defines.ESceneNumder;

public class MakeCharacterManager : MonoBehaviour
{

    [SerializeField] PlayerCustomize _playerCustomize;
    [SerializeField] Button _maleSelectButton;
    [SerializeField] Button _femaleSelectButton;
    [SerializeField] Button _makeCharacterButton;
    [SerializeField] GameObject _femalePanel;
    [SerializeField] GameObject _malePanel;

    void Start()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _maleSelectButton.onClick.AddListener(SelectMale);

        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _femaleSelectButton.onClick.AddListener(SelectFemale);

        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);
        _makeCharacterButton.onClick.AddListener(CreateCharacter);

        //StartCoroutine(FindPlayerCustomize());
    }

    //IEnumerator FindPlayerCustomize()
    //{
    //    yield return new WaitForSeconds(5f);

    //    _playerCustomize = GameObject.Find("SM_Chr_Peasant_Male_01").GetComponent<PlayerCustomize>();

    //    yield return null;

    //}

    private void SelectMale()
    {
        _playerCustomize.IsFemale = false;
        _playerCustomize.MakeAvatarData();
        _malePanel.SetActive(true);
        _femalePanel.SetActive(false);
    }
    private void SelectFemale()
    {
        _playerCustomize.IsFemale = true;
        _playerCustomize.MakeAvatarData();
        _malePanel.SetActive(false);
        _femalePanel.SetActive(true);
    }


    private void CreateCharacter()
    {
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, TempAccountDB.Nickname, Asset.EaccountdbColumns.HaveCharacter, "1");
        MySqlSetting.AddNewCharacter(TempAccountDB.Nickname, $"{Convert.ToInt32(_playerCustomize.IsFemale)}");
        Debug.Log(_playerCustomize.IsFemale);
        SceneManager.LoadScene((int)SceneType.StartRoom);
    }

    private void OnDisable()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);

    }
}
