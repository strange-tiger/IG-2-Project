//#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;
using UnityEngine.EventSystems;

public enum EPetEvolutionCount
{
    NONE,
    ZERO,
    ONE,
    TWO,
};

public class TransformUI : MonoBehaviour
{
    [Header("UIManager")]
    [SerializeField] PetUIManager _ui;

    [Header("Button")]
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _transformButton;
    [SerializeField] Button _leftTransformButton;
    [SerializeField] Button _rightTransformButton;
    [SerializeField] Button _closeButton;

    [Header("Display")]
    [SerializeField] GameObject _petObject;
    [SerializeField] TextMeshProUGUI _petName;
    [SerializeField] TextMeshProUGUI _petGrade;
    [SerializeField] TextMeshProUGUI _petExplanation;
    private TextMeshProUGUI _petTransformOption;

    [Header("Popup")]
    [SerializeField] GameObject _applyPopup;

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
            _currentPetTransform = _currentPet.PetObject.transform;
            OnCurrentPetChanged.Invoke();
        }
    }
    private PetUIManager.PetProfile _currentPet = new PetUIManager.PetProfile();
    private Transform _currentPetTransform;

    private int _currentIndex = 0;
    private int _equipedIndex = -1;
    private int _transformIndex = 0;
    private int _maxTransformIndex = 0;
    private bool _doTransformScale = false;
    private EPetEvolutionCount _currentPetEvolutionCount;

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

        _transformButton.onClick.RemoveListener(TransformPet);
        _transformButton.onClick.AddListener(TransformPet);

        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);

        OnCurrentPetChanged -= ShowCurrentPet;
        OnCurrentPetChanged += ShowCurrentPet;

        _petTransformOption = _transformButton.GetComponentInChildren<TextMeshProUGUI>();

        for (int i = 0; i < _ui.PetList.Length; ++i)
        {
            if (_ui.PetList[i].Status == EPetStatus.EQUIPED)
            {
                _equipedIndex = i;
                break;
            }
        }
        _currentIndex = _equipedIndex;

        if (_equipedIndex != -1)
            CurrentPet = _ui.PetList[_equipedIndex];

        _applyPopup.SetActive(false);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _transformButton.onClick.RemoveListener(TransformPet);
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
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnClickLeftTransformButton();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnClickRightTransformButton();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            TransformPet();
        }
    }
#endif

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

    private void Close()
    {
        PetData petData = _ui.GetPetData();
        for (int i = 0; i < _ui.PetList.Length; ++i)
        {
            petData.Size[i] = _ui.PetList[i].Size;
            petData.ChildIndex[i] = _ui.PetList[i].AssetIndex;
            petData.Status[i] = _ui.PetList[i].Status;
        }

#if !debug
        if (!_DB.UpdatePetInventoryData("aaa", petData))
        {
            return;
        }
#endif
        _ui.LoadUI(_UI.POPUP);

        _applyPopup.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnClickLeftButton()
    {
        if (_currentIndex == -1)
        {
            return;
        }

        int prevIndex = _currentIndex;

        _ui.PetList[prevIndex].SetStatus(EPetStatus.HAVE);
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
        while (_ui.PetList[_currentIndex].Status < EPetStatus.HAVE);

        UpdateCurrentPet();
    }

    private void OnClickRightButton()
    {
        if (_currentIndex == -1)
        {
            return;
        }

        int prevIndex = _currentIndex;

        _ui.PetList[prevIndex].SetStatus(EPetStatus.HAVE);
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
        while (_ui.PetList[_currentIndex].Status < EPetStatus.HAVE);

        UpdateCurrentPet();
    }

    private void UpdateCurrentPet()
    {
        _ui.PetList[_currentIndex].SetStatus(EPetStatus.EQUIPED);
        CurrentPet = _ui.PetList[_currentIndex];
        _transformIndex = 0;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnClickLeftTransformButton()
    {
        if (_currentIndex == -1)
        {
            return;
        }

        int currentPetEvolutionCount = 0;
        do
        {
            if (_transformIndex - 1 < 0)
            {
                _transformIndex = _maxTransformIndex;
            }
            --_transformIndex;

            currentPetEvolutionCount = (int)_currentPetTransform.GetChild(_transformIndex).GetComponent<PetInteract>().PetEvolutionCount - 1;
        }
        while (CurrentPet.Level <= currentPetEvolutionCount);

        ShowTransformOption(_transformIndex);
    }

    private void OnClickRightTransformButton()
    {
        if (_currentIndex == -1)
        {
            return;
        }

        int currentPetEvolutionCount = 0;
        do
        {
            if (_transformIndex + 1 >= _maxTransformIndex)
            {
                _transformIndex = -1;
            }
            ++_transformIndex;

            currentPetEvolutionCount = (int)_currentPetTransform.GetChild(_transformIndex).GetComponent<PetInteract>().PetEvolutionCount - 1;
        }
        while (CurrentPet.Level <= currentPetEvolutionCount);


        ShowTransformOption(_transformIndex);
    }

    private void TransformPetChildAsset(int index)
    {
        _ui.PetList[_currentIndex].SetAssetIndex(index);

        for (int i = 0; i < _currentPetTransform.childCount; ++i)
        {
            _currentPetTransform.GetChild(i).gameObject.SetActive(false);
        }

        _currentPetTransform.GetChild(index).gameObject.SetActive(true);
    }

    private static readonly float[] TRANSFORM_SCALE = new float[3] { 0.3f, 0.5f, 1f };
    private void TransformPetScale(int index)
    {
        _ui.PetList[_currentIndex].SetSize(TRANSFORM_SCALE[index]);
        _currentPetTransform.localScale = 100f * TRANSFORM_SCALE[index] * Vector3.one;
    }

    private void ShowCurrentPet()
    {
        TogglePetObject(CurrentPet.PetObject);

        _petName.text = CurrentPet.Name;

        ShowPetGrade(CurrentPet.Grade);

        _petExplanation.text = CurrentPet.Explanation;

        UpdateTransformOption();
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

    private void UpdateTransformOption()
    {
        if (_currentPetTransform.GetComponentInChildren<PetInteract>().PetEvolutionCount == EPetEvolutionCount.NONE)
        {
            _maxTransformIndex = TRANSFORM_SCALE.Length;

            _doTransformScale = true;
        }
        else
        {
            _maxTransformIndex = _currentPetTransform.childCount;

            _doTransformScale = false;
        }

        ShowTransformOption(0);
    }

    private void ShowTransformOption(int index)
    {
        if (_doTransformScale)
        {
            _petTransformOption.text = $"{(int)100 * TRANSFORM_SCALE[index]}%";
        }
        else
        {
            // �ӽ�
            _petTransformOption.text = _currentPetTransform.GetChild(index).name;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
}