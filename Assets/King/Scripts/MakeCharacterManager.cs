using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeCharacterManager : MonoBehaviour
{
    public Color SkinColorValue;

    [SerializeField]
    private Slider _characterRotateSlider;
    [SerializeField]
    private Button _genderSelectButton;
    [SerializeField]
    private Button _skinSelectButton;
    [SerializeField]
    private Button _maleSelectButton;
    [SerializeField]
    private Button _femaleSelectButton;
    [SerializeField]
    private GameObject _maleCharacter;
    [SerializeField]
    private GameObject _femaleCharacter;
    [SerializeField]
    private GameObject _customizingCharacter;
    [SerializeField]
    private GameObject _genderSettingPanel;
    [SerializeField]
    private GameObject _skinSettingPanel;
    
    void Start()
    {
        _genderSelectButton.onClick.RemoveListener(SelectGender);
        _genderSelectButton.onClick.AddListener(SelectGender);

        _skinSelectButton.onClick.RemoveListener(SelectSkin);
        _skinSelectButton.onClick.AddListener(SelectSkin);

        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _maleSelectButton.onClick.AddListener(SelectMale);

        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
        _femaleSelectButton.onClick.AddListener(SelectFemale);

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
    }
    private void SelectFemale()
    {
        _maleCharacter.SetActive(false);
        _femaleCharacter.SetActive(true);
    }
    private void SelectGender()
    {
        _skinSettingPanel.SetActive(false);
        _genderSettingPanel.SetActive(true);
    }
    private void SelectSkin()
    {
        _genderSettingPanel.SetActive(false);
        _skinSettingPanel.SetActive(true);
    }

    private void OnDisable()
    {
        _genderSelectButton.onClick.RemoveListener(SelectGender);
        _skinSelectButton.onClick.RemoveListener(SelectSkin);
        _maleSelectButton.onClick.RemoveListener(SelectMale);
        _femaleSelectButton.onClick.RemoveListener(SelectFemale);
    }
}
