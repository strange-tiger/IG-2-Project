using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

using _UI = Defines.EPetShopUIIndex;
using _DB = Asset.MySql.MySqlSetting;

public class PetPurchaseUI : MonoBehaviour
{
    [Header("UIManager")]
    [SerializeField] PetShopUIManager _ui;

    [Header("Button")]
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _closeButton;

    [Header("Pet Info")]
    [SerializeField] Sprite[] _petImages;
    [SerializeField] TextMeshProUGUI[] _petNames;
    [SerializeField] TextMeshProUGUI[] _petGrades;
    [SerializeField] TextMeshProUGUI[] _petExplanations;
    [SerializeField] TextMeshProUGUI[] _petPrices;

    [Header("Display")]
    [SerializeField] Sprite _petImage;
    [SerializeField] TextMeshProUGUI _petName;
    [SerializeField] TextMeshProUGUI _petGrade;
    [SerializeField] TextMeshProUGUI _petExplanation;
    [SerializeField] TextMeshProUGUI _petPrice;

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

    private int _currentIndex = -1;
    private int _equipedIndex = -1;

    private void OnEnable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _leftButton.onClick.AddListener(OnClickLeftButton);

        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _rightButton.onClick.AddListener(OnClickRightButton);

        _purchaseButton.onClick.RemoveListener(Purchase);
        _purchaseButton.onClick.AddListener(Purchase);

        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);

        OnCurrentPetChanged -= ShowCurrentPet;
        OnCurrentPetChanged += ShowCurrentPet;

        _currentIndex = -1;
        OnClickRightButton();
    }

    private void Purchase()
    {
        if (!_ui.PlayerNetworking.GetComponent<PhotonView>().IsMine)
        {
            return;
        }

        int price = CurrentPet.Price;

        if (!_DB.UseGold(_ui.PlayerNickname, price))
        {
            return;
        }

        if (_equipedIndex != -1)
        {
            _ui.PetList[_equipedIndex].SetStatus(EPetStatus.HAVE);
        }

        _equipedIndex = _currentIndex;
        CurrentPet.SetStatus(EPetStatus.EQUIPED);
        _ui.PetList[_currentIndex].SetStatus(EPetStatus.EQUIPED);

        if (_equipedIndex != -1)
        {
            PetShopUIManager.PlayerPetSpawner.PetChange(_equipedIndex);
        }

        _purchaseButton.enabled = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Close()
    {
        PetData petData = _ui.GetPetData();
        for (int i = 0; i < _ui.PetList.Length; ++i)
        {
            petData.Status[i] = _ui.PetList[i].Status;
        }

        if (!_DB.UpdatePetInventoryData(_ui.PlayerNickname, petData))
        {
            return;
        }
        
        _ui.LoadUI(_UI.FIRST);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnClickLeftButton()
    {
        int prevIndex = _currentIndex;
        do
        {
            if (_currentIndex - 1 < 0)
            {
                _currentIndex = _ui.PetList.Length;
            }
            --_currentIndex;

            if (_currentIndex == prevIndex)
            {
                break;
            }
        }
        while (_ui.PetList[_currentIndex].Status != EPetStatus.NONE);

        CurrentPet = _ui.PetList[_currentIndex];

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnClickRightButton()
    {
        int prevIndex = _currentIndex;
        do
        {
            if (_currentIndex + 1 >= _ui.PetList.Length)
            {
                _currentIndex = -1;
            }
            ++_currentIndex;

            if (_currentIndex == prevIndex)
            {
                break;
            }
        }
        while (_ui.PetList[_currentIndex].Status != EPetStatus.NONE);

        CurrentPet = _ui.PetList[_currentIndex];

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ShowCurrentPet()
    {
        ChangePetImage(CurrentPet.Image);

        _petName.text = CurrentPet.Name;

        ShowPetGrade(CurrentPet.Grade);

        _petExplanation.text = CurrentPet.Explanation;
        _petPrice.text = CurrentPet.Price.ToString();

        if (int.Parse(_petPrice.text) > _DB.CheckHaveGold(_ui.PlayerNickname))
        {
            _purchaseButton.enabled = false;
        }
        else
        {
            _purchaseButton.enabled = true;
        }
    }

    private void ChangePetImage(Sprite currentPet)
    {
        _petImage = currentPet;
    }

    private static readonly Color[] GRADE_COLOR = new Color[4]
    {
        new Color(128f, 128f, 128f),
        new Color(0f, 128f, 0f),
        new Color(0f, 103f, 163f),
        new Color(155f, 17f, 30f)
    };
    private void ShowPetGrade(PetShopUIManager.PetProfile.EGrade grade)
    {
        _petGrade.text = grade.ToString();
        _petGrade.color = GRADE_COLOR[(int)grade];
    }
}
