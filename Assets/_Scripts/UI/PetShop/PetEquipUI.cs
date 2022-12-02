﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using _UI = Defines.EPetShopUIIndex;
using _DB = Asset.MySql.MySqlSetting;

public class PetEquipUI : MonoBehaviour
{
    [Header("UIManager")]
    [SerializeField] PetShopUIManager _ui;

    [Header("Scriptable Object")]
    [SerializeField] PetTransformList[] _transformList;

    [Header("Button")]
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _leftTransformButton;
    [SerializeField] Button _rightTransformButton;
    [SerializeField] Button _backButton;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _saveButton;

    [Header("Equiped Pet Info")]
    [SerializeField] Image _equipedPetImage;
    [SerializeField] Sprite _airImage;

    [Header("Current Pet")]
    [SerializeField] Image _petImage;
    [SerializeField] TextMeshProUGUI _petName;
    [SerializeField] TextMeshProUGUI _petGrade;
    [SerializeField] TextMeshProUGUI _petExplanation;
    [SerializeField] TextMeshProUGUI _petTransformOption;

    [Header("Apply Text")]
    [SerializeField] TextMeshProUGUI _applyText;

    private const string DEFAULT_APPLY_TEXT = "[저장하기]를 누르면 변환이 반영됩니다.";
    private const string SAVED_APPLY_TEXT = "저장되었습니다!";
    private static readonly WaitForSeconds APPLY_TEXT_DURATION = new WaitForSeconds(1f);
    private static readonly PetShopUIManager.PetProfile PET_AIR = new PetShopUIManager.PetProfile();

    public event Action OnCurrentPetChanged;
    public PetShopUIManager.PetProfile CurrentPet
    {
        get
        {
            return _currentPet;
        }
        set
        {
            _currentPet = value;
            OnCurrentPetChanged.Invoke();
        }
    }
    private PetShopUIManager.PetProfile _currentPet = new PetShopUIManager.PetProfile();

    private PetShopUIManager.PetProfile[] _petList;

    private int _petDataLength;
    private int _currentIndex = 0;
    private int _equipedIndex = -1;
    private int _transformIndex = 0;
    private int _maxTransformIndex = 0;
    private bool _doTransformScale = false;

    private void Awake()
    {
        _petDataLength = _ui.PetList.Length;
        PET_AIR.SetImage(_airImage);
        PET_AIR.SetStatus(EPetStatus.HAVE);
        _petList = new PetShopUIManager.PetProfile[_petDataLength + 1];
    }

    private void OnEnable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _leftButton.onClick.AddListener(OnClickLeftButton);

        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _rightButton.onClick.AddListener(OnClickRightButton);

        _leftTransformButton.onClick.RemoveListener(OnClickLeftTransformButton);
        _leftTransformButton.onClick.AddListener(OnClickLeftTransformButton);

        _rightTransformButton.onClick.RemoveListener(OnClickRightTransformButton);
        _rightTransformButton.onClick.AddListener(OnClickRightTransformButton);

        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);

        _backButton.onClick.RemoveListener(Back);
        _backButton.onClick.AddListener(Back);

        _saveButton.onClick.RemoveListener(SaveOption);
        _saveButton.onClick.AddListener(SaveOption);

        OnCurrentPetChanged -= ShowCurrentPet;
        OnCurrentPetChanged += ShowCurrentPet;

        for (int i = 0; i < _petDataLength; ++i)
        {
            _petList[i] = _ui.PetList[i];
        }
        _petList[_petDataLength] = PET_AIR;

        _equipedIndex = PetShopUIManager.PlayerPetSpawner.EquipedNum;

        _currentIndex = _equipedIndex;

        if (_equipedIndex != -1)
        {
            CurrentPet = _ui.PetList[_equipedIndex];
        }
        else
        {
            CurrentPet = PET_AIR;
        }

        _equipedPetImage.sprite = CurrentPet.Image;

        _applyText.text = DEFAULT_APPLY_TEXT;
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _leftTransformButton.onClick.RemoveListener(OnClickLeftTransformButton);
        _rightTransformButton.onClick.RemoveListener(OnClickRightTransformButton);
        _closeButton.onClick.RemoveListener(Close);
        _backButton.onClick.RemoveListener(Back);
        _saveButton.onClick.RemoveListener(SaveOption);
        OnCurrentPetChanged -= ShowCurrentPet;
    }

    private void Back() => _ui.LoadUI(_UI.FIRST);

    private void Close() => _ui.ShutPetUI();

    private void OnClickLeftButton()
    {
        int prevIndex = _currentIndex;

        do
        {
            if (_currentIndex - 1 < 0)
            {
                _currentIndex = _petList.Length;
            }
            --_currentIndex;

            if (_currentIndex == prevIndex)
            {
                break;
            }
        }
        while (_petList[_currentIndex].Status < EPetStatus.HAVE);

        UpdateCurrentPet();
    }

    private void OnClickRightButton()
    {
        int prevIndex = _currentIndex;

        do
        {
            if (_currentIndex + 1 >= _petList.Length)
            {
                _currentIndex = -1;
            }
            ++_currentIndex;

            if (_currentIndex == prevIndex)
            {
                break;
            }
        }
        while (_petList[_currentIndex].Status < EPetStatus.HAVE);

        UpdateCurrentPet();
    }

    private void SaveOption()
    {
        if (!_ui.PlayerNetworking.photonView.IsMine)
        {
            return;
        }

        if (_equipedIndex < 0 || _equipedIndex >= _petDataLength)
        {
            return;
        }
        _ui.PetList[_equipedIndex].SetStatus(EPetStatus.HAVE);

        if (_currentIndex >= 0 && _currentIndex < _petDataLength)
        {
            _ui.PetList[_currentIndex].SetStatus(EPetStatus.EQUIPED);
            
            TransformPet();
        }

        _equipedPetImage.sprite = CurrentPet.Image;

        _equipedIndex = _currentIndex;

        PetData petData = _ui.GetPetData();
        for (int i = 0; i < _ui.PetList.Length; ++i)
        {
            petData.Size[i] = _ui.PetList[i].Size;
            petData.ChildIndex[i] = _ui.PetList[i].AssetIndex;
            petData.Status[i] = _ui.PetList[i].Status;
        }

        if (!_DB.UpdatePetInventoryData(_ui.PlayerNickname, petData))
        {
            return;
        }

        PetShopUIManager.PlayerPetSpawner.PetChange(_equipedIndex);

        StartCoroutine(ChangeApplyText());
    }

    private IEnumerator ChangeApplyText()
    {
        _applyText.text = SAVED_APPLY_TEXT;

        yield return APPLY_TEXT_DURATION;

        _applyText.text = DEFAULT_APPLY_TEXT;
    }

    private void UpdateCurrentPet()
    {
        CurrentPet = _petList[_currentIndex];
        
        _transformIndex = 0;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnClickLeftTransformButton()
    {
        if (_currentIndex < 0 || _currentIndex >= _petList.Length)
        {
            return;
        }

        int prevIndex = _transformIndex;
        do
        {
            if (_transformIndex - 1 < 0)
            {
                _transformIndex = _maxTransformIndex;
            }
            --_transformIndex;

            if (_transformIndex == prevIndex)
            {
                break;
            }
        }
        while (CurrentPet.Level < (int)_transformList[_currentIndex].Level[_transformIndex]);

        ShowTransformOption(_transformIndex);
    }

    private void OnClickRightTransformButton()
    {
        if (_currentIndex < 0 || _currentIndex >= _petList.Length)
        {
            return;
        }

        int prevIndex = _transformIndex;
        do
        {
            if (_transformIndex + 1 >= _maxTransformIndex)
            {
                _transformIndex = -1;
            }
            ++_transformIndex;

            if (_transformIndex == prevIndex)
            {
                break;
            }
        }
        while (CurrentPet.Level < _transformList[_currentIndex].Level[_transformIndex]);

        ShowTransformOption(_transformIndex);
    }

    private void TransformPet()
    {
        if (_doTransformScale)
        {
            TransformPetScale(_transformIndex);
        }
        else
        {
            TransformPetChildAsset(_transformIndex);
        }
    }

    private void TransformPetChildAsset(int index)
    {
        _ui.PetList[_currentIndex].SetAssetIndex(index);
    }

    private static readonly float[] TRANSFORM_SCALE = new float[3] { 0.3f, 0.5f, 1f };
    private void TransformPetScale(int index)
    {
        _ui.PetList[_currentIndex].SetSize(TRANSFORM_SCALE[index]);
    }

    private void ShowCurrentPet()
    {
        _petImage.sprite = CurrentPet.Image;

        _petName.text = CurrentPet.Name;

        ShowPetGrade(CurrentPet.Grade);

        _petExplanation.text = CurrentPet.Explanation;

        UpdateTransformOption();
    }

    
    private void ShowPetGrade(PetShopUIManager.PetProfile.EGrade grade)
    {
        _petGrade.text = grade.ToString();
        _petGrade.color = PetShopUIManager.GRADE_COLOR[(int)grade];
    }

    private const int BEFORE_S_INDEX = 12;
    private void UpdateTransformOption()
    {
        if (_currentIndex < 0 || _currentIndex > _petDataLength)
        {
            return;
        }

        _doTransformScale = (_currentIndex > BEFORE_S_INDEX);

        _maxTransformIndex = _transformList[_currentIndex].Image.Length;

        ShowTransformOption(0);
    }

    private void ShowTransformOption(int index)
    {
        _petTransformOption.text = _transformList[_currentIndex].Name[index];

        CurrentPet.SetImage(_transformList[_currentIndex].Image[index]);

        _petImage.sprite = CurrentPet.Image;

        EventSystem.current.SetSelectedGameObject(null);
    }
}