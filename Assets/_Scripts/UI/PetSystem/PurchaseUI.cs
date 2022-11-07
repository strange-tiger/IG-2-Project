#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;

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

    private int _index = 0;

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

        _index = 0;
        Debug.Log(_ui.PetList[_index]);
        CurrentPet = _ui.PetList[_index];
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
        if (_DB.UseGold(_ui.PlayerNetworkingInPet.MyNickname, int.Parse(_petPrice.text)))
        {
            return;
        }
#endif

        CurrentPet.SetIsHave(true);
        _ui.PetList[_index].SetIsHave(true);

        _purchaseButton.enabled = false;
    }

    private void Close() => _ui.LoadUI(_UI.POPUP);

    private void OnClickLeftButton()
    {
        int prevIndex = _index;
        do
        {
            if (_index - 1 < 0)
            {
                _index = _ui.PetList.Length;
            }
            --_index;

            if (_index == prevIndex)
            {
                break;
            }

            CurrentPet = _ui.PetList[_index];
        }
        while (CurrentPet.IsHave);
    }

    private void OnClickRightButton()
    {
        int prevIndex = _index;
        do
        {
            if (_index + 1 >= _ui.PetList.Length)
            {
                _index = -1;
            }
            ++_index;

            if (_index == prevIndex)
            {
                break;
            }

            CurrentPet = _ui.PetList[_index];
        }
        while (CurrentPet.IsHave);
    }

    private void ShowCurrentPet()
    {
        TogglePetObject(CurrentPet.Prefab);

        _petName.text = CurrentPet.Name;

        ShowPetGrade(CurrentPet.Grade);

        _petExplanation.text = CurrentPet.Explanation;
        _petPrice.text = CurrentPet.Price.ToString();

#if debug
        if (int.Parse(_petPrice.text) > 100)
#else
        if (int.Parse(_petPrice.text) > _DB.CheckHaveGold(_ui.PlayerNetworkingInPet.MyNickname))
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
