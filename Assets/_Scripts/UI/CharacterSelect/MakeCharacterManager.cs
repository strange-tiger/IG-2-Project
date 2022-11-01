using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;

public class MakeCharacterManager : MonoBehaviour
{

    [SerializeField] private Button _maleSelectButton;
    [SerializeField] private Button _femaleSelectButton;
    [SerializeField] private Button _makeCharacterButton;

    private int _isFemaleCharacter;

    void Start()
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
        _isFemaleCharacter = 0;
    }
    private void SelectFemale()
    {
        _isFemaleCharacter = 1;
    }


    private void CreateCharacter()
    {
        MySqlSetting.AddNewCharacter(name, $"{_isFemaleCharacter}");
    }

    private void OnDisable()
    {
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);

    }
}
