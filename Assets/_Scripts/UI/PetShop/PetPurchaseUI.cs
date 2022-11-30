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
    [SerializeField] Button[] _backButtons;
    [SerializeField] Button[] _petInfoButtons;

    [Header("Equiped Pet Info")]
    [SerializeField] Image _equipedPetImage;
    [SerializeField] TextMeshProUGUI _equipedPetName;
    [SerializeField] TextMeshProUGUI _equipedPetGrade;
    [SerializeField] TextMeshProUGUI _haveGold;

    [Header("Pet Info")]
    [SerializeField] Image[] _petImages;
    [SerializeField] TextMeshProUGUI[] _petNames;
    [SerializeField] TextMeshProUGUI[] _petGrades;
    [SerializeField] TextMeshProUGUI[] _petPrices;
    private PetShopUIManager.PetProfile[] _petInfoList = new PetShopUIManager.PetProfile[3];

    [Header("Popup")]
    [SerializeField] GameObject _petInfoPopup;
    [SerializeField] GameObject _purchaseSuccessPopup;

    [Header("Pet Info Popup")]
    [SerializeField] Image _petImage;
    [SerializeField] TextMeshProUGUI _petName;
    [SerializeField] TextMeshProUGUI _petGrade;
    [SerializeField] TextMeshProUGUI _petExplanation;
    [SerializeField] TextMeshProUGUI _petPrice;

    private PetShopUIManager.PetProfile _currentPet = new PetShopUIManager.PetProfile();

    private int _currentIndex = -1;
    private int _equipedIndex = -1;
    private int _chooseIndex = -1;
    private int[] _chooseIndexes = new int[3];

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


        _equipedIndex = PetShopUIManager.PlayerPetSpawner.EquipedNum;

        if (_equipedIndex != -1)
        {
            ShowEquipedPet(_ui.PetList[_equipedIndex]);
        }
        else
        {
            ShowEquipedPet(new PetShopUIManager.PetProfile());
        }


        _currentIndex = -1;
        OnClickRightButton();
        
        foreach (Button back in _backButtons)
        {
            back.onClick.RemoveListener(Back);
            back.onClick.AddListener(Back);
        }

        foreach (Button petInfoButton in _petInfoButtons)
        {
            petInfoButton.onClick.RemoveListener(ShowCurrentPet);
            petInfoButton.onClick.AddListener(ShowCurrentPet);
        }

        _petInfoPopup.SetActive(false);
        _purchaseSuccessPopup.SetActive(false);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _purchaseButton.onClick.RemoveListener(Purchase);
        _closeButton.onClick.RemoveListener(Close);

        foreach (Button back in _backButtons)
        {
            back.onClick.RemoveListener(Back);
        }

        foreach (Button petInfoButton in _petInfoButtons)
        {
            petInfoButton.onClick.RemoveListener(ShowCurrentPet);
        }
    }

    private void Purchase()
    {
        if (!_ui.PlayerNetworking.photonView.IsMine)
        {
            return;
        }

        int price = _currentPet.Price;

        if (!_DB.UseGold(_ui.PlayerNickname, price))
        {
            return;
        }

        if (_equipedIndex != -1)
        {
            _ui.PetList[_equipedIndex].SetStatus(EPetStatus.HAVE);
        }

        _equipedIndex = _chooseIndex;
        _currentPet.SetStatus(EPetStatus.EQUIPED);
        _ui.PetList[_equipedIndex].SetStatus(EPetStatus.EQUIPED);

        PetShopUIManager.PlayerPetSpawner.PetChange(_equipedIndex);

        PetData petData = _ui.GetPetData();
        petData.Status[_equipedIndex] = _ui.PetList[_equipedIndex].Status;

        _DB.UpdatePetInventoryData(_ui.PlayerNickname, petData);

        PetShopUIManager.PlayerPetSpawner.PetChange(_equipedIndex);

        _purchaseButton.enabled = false;

        _purchaseSuccessPopup.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Back() => _ui.LoadUI(_UI.FIRST);

    private void Close() => _ui.ShutPetUI();

    private void OnClickLeftButton()
    {
        int count = _petInfoButtons.Length;

        while (count > 0)
        {
            --count;

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
                    return;
                }
            }
            while (_ui.PetList[_currentIndex].Status != EPetStatus.NONE);

            ShowPet(count, _ui.PetList[_currentIndex]);
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnClickRightButton()
    {
        int count = 0;

        while (count < _petInfoButtons.Length)
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
                    return;
                }
            }
            while (_ui.PetList[_currentIndex].Status != EPetStatus.NONE);

            ShowPet(count, _ui.PetList[_currentIndex]);

            ++count;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ShowPet(int index, PetShopUIManager.PetProfile pet)
    {
        _chooseIndexes[index] = _currentIndex;

        _petImages[index].sprite = pet.Image;

        _petNames[index].text = pet.Name;

        ShowPetGrade(index, pet.Grade);

        _petPrices[index].text = pet.Price.ToString();

        _petInfoList[index] = pet;
    }

    private void ShowEquipedPet(PetShopUIManager.PetProfile pet)
    {
        _equipedPetImage.sprite = pet.Image;

        _equipedPetName.text = pet.Name;

        _equipedPetGrade.text = pet.Grade.ToString();
        _equipedPetGrade.color = GRADE_COLOR[(int)pet.Grade];

        _haveGold.text = _DB.CheckHaveGold(_ui.PlayerNickname).ToString();
    }

    private void ShowCurrentPet()
    {
        for (int i = 0; i < _petInfoButtons.Length; ++i)
        {
            if (_petInfoButtons[i].name == EventSystem.current.currentSelectedGameObject.name)
            {
                _chooseIndex = _chooseIndexes[i];
                ShowCurrentPet(_petInfoList[i]);
            }
        }
    }

    private void ShowCurrentPet(PetShopUIManager.PetProfile pet)
    {
        _petInfoPopup.SetActive(true);

        _currentPet = pet;

        _petImage.sprite = pet.Image;

        _petName.text = pet.Name;

        ShowPetGrade(pet.Grade);

        _petExplanation.text = pet.Explanation;
        _petPrice.text = $"가격은 {pet.Price} 골드 입니다.\n구입 하시겠습니까?";

        if (pet.Price > _DB.CheckHaveGold(_ui.PlayerNickname))
        {
            _purchaseButton.enabled = false;
        }
        else
        {
            _purchaseButton.enabled = true;
        }
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

    private void ShowPetGrade(int index, PetShopUIManager.PetProfile.EGrade grade)
    {
        _petGrades[index].text = grade.ToString();
        _petGrades[index].color = GRADE_COLOR[(int)grade];
    }
}
