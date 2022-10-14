using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;

public class MakeCharacterManager : MonoBehaviour
{

    [SerializeField] private Slider _characterRotateSlider;
    [SerializeField] private Button _genderSelectButton;
    [SerializeField] private Button _maleSelectButton;
    [SerializeField] private Button _femaleSelectButton;
    [SerializeField] private Button _makeCharacterButton;
    [SerializeField] private GameObject _maleCharacter;
    [SerializeField] private GameObject _femaleCharacter;
    [SerializeField] private GameObject _customizingCharacter;
    [SerializeField] private GameObject _genderSettingPanel;
    [SerializeField] private GameObject _skinSettingPanel;
    private bool _isFemaleCharacter;

    void Start()
    {
        _genderSelectButton.onClick.RemoveListener(SelectGender);
        _genderSelectButton.onClick.AddListener(SelectGender);

        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _maleSelectButton.onClick.AddListener(SelectMale);

        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _femaleSelectButton.onClick.AddListener(SelectFemale);

        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);
        _makeCharacterButton.onClick.AddListener(CreateCharacter);

        _characterRotateSlider.minValue = 0f;
        _characterRotateSlider.maxValue = 360f;
        _characterRotateSlider.value = 180f;

    }

    private void Update()
    {
        _customizingCharacter.transform.rotation = Quaternion.Euler(0, _characterRotateSlider.value, 0);
    }

    private void SelectMale()
    {
        _femaleCharacter.SetActive(false);
        _maleCharacter.SetActive(true);
        _isFemaleCharacter = false;
    }
    private void SelectFemale()
    {
        _maleCharacter.SetActive(false);
        _femaleCharacter.SetActive(true);
        _isFemaleCharacter = true;
    }
    private void SelectGender()
    {
        _skinSettingPanel.SetActive(false);
        _genderSettingPanel.SetActive(true);
    }

    private void CreateCharacter()
    {
       
    }

    private void OnDisable()
    {
        _genderSelectButton.onClick.RemoveListener(SelectGender);
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _makeCharacterButton.onClick.RemoveListener(CreateCharacter);

    }
}
