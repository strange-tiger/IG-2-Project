//#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;
using UnityEngine.EventSystems;

public class PurchaseUI : MonoBehaviour
{
    [Header("UIManager")]
    [SerializeField] PetUIManager _ui;

    [Header("Button")]
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _closeButton;

    [Header("Display")]
    [SerializeField] GameObject _petObject;
    [SerializeField] TextMeshProUGUI _petName;
    [SerializeField] TextMeshProUGUI _petGrade;
    [SerializeField] TextMeshProUGUI _petExplanation;
    private TextMeshProUGUI _petPrice;

    public event Action OnCurrentPetChanged;
    public PetUIManager.PetProfile CurrentPet
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
    private PetUIManager.PetProfile _currentPet = new PetUIManager.PetProfile();

    private int _currentIndex = -1;
    private int _equipedIndex = -1;
    private int _purchaseAmount = 0;

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

        _petPrice = _purchaseButton.GetComponentInChildren<TextMeshProUGUI>();

        _currentIndex = -1;
        OnClickRightButton();
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _purchaseButton.onClick.RemoveListener(Purchase);
        _closeButton.onClick.RemoveListener(Close);
        OnCurrentPetChanged -= ShowCurrentPet;
    }

#if debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnClickLeftButton();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnClickRightButton();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Purchase();
        }
    }
#endif

    private void Purchase()
    {
#if !debug
        if (_DB.CheckHaveGold(_ui.PlayerNickname) < _purchaseAmount + int.Parse(_petPrice.text))
        {
            return;
        }
#endif
        _purchaseAmount += int.Parse(_petPrice.text);

        if (_equipedIndex != -1)
        {
            _ui.PetList[_equipedIndex].SetStatus(EPetStatus.HAVE);
        }

        _equipedIndex = _currentIndex;
        CurrentPet.SetStatus(EPetStatus.EQUIPED);
        _ui.PetList[_currentIndex].SetStatus(EPetStatus.EQUIPED);

        Debug.Log(_ui.PetList[_currentIndex].Status);

        _purchaseButton.enabled = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Close()
    {
#if !debug
        if (!_DB.UseGold(_ui.PlayerNickname, _purchaseAmount))
        {
            _purchaseAmount = 0;
            return;
        }
#endif
        _purchaseAmount = 0;

        PetData petData = _ui.GetPetData();
        for (int i = 0; i < _ui.PetList.Length; ++i)
        {
            petData.Status[i] = _ui.PetList[i].Status;
        }

#if !debug
        if (!_DB.UpdatePetInventoryData(_ui.PlayerNickname, petData))
        {
            return;
        }
#endif
        _ui.LoadUI(_UI.POPUP);

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
        TogglePetObject(CurrentPet.PetObject);

        _petName.text = CurrentPet.Name;

        ShowPetGrade(CurrentPet.Grade);

        _petExplanation.text = CurrentPet.Explanation;
        _petPrice.text = CurrentPet.Price.ToString();

#if debug
        if (int.Parse(_petPrice.text) > 100)
#else
        if (int.Parse(_petPrice.text) > _DB.CheckHaveGold(_ui.PlayerNickname))
#endif
        {
            _purchaseButton.enabled = false;
        }
        else
        {
            _purchaseButton.enabled = true;
        }
    }

    private void TogglePetObject(GameObject currentPet)
    {
        _petObject.SetActive(false);
        _petObject = currentPet;
        _petObject.transform.parent = transform;
        _petObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        _petObject.transform.localPosition = Vector3.zero;
        _petObject.SetActive(true);
    }

    private static readonly Color[] GRADE_COLOR = new Color[4]
    {
        new Color(128f, 128f, 128f),
        new Color(0f, 128f, 0f),
        new Color(0f, 103f, 163f),
        new Color(155f, 17f, 30f)
    };
    private void ShowPetGrade(PetUIManager.PetProfile.EGrade grade)
    {
        _petGrade.text = grade.ToString();
        _petGrade.color = GRADE_COLOR[(int)grade];
    }
}