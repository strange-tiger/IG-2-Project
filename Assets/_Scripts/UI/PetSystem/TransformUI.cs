#define debug
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;

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
            OnCurrentPetChanged.Invoke();
        }
    }
    private PetUIManager.PetProfile _currentPet = new PetUIManager.PetProfile();

    private delegate void TransformDelegate(int index);
    TransformDelegate transformPet;

    private int _index = 0;
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

        _index = 0;
        Debug.Log(_ui.PetList[_index]);
        CurrentPet = _ui.PetList[_index];
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
        else if (Input.GetKeyDown(KeyCode.T))
        {
            TransformPet();
        }
    }
#endif

    private void TransformPet()
    {
#if !debug
        if (_DB.UseGold(_ui.PlayerNetworkingInPet.MyNickname, int.Parse(_petPrice.text)))
        {
            return;
        }
#endif
        Debug.Log("변환");
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
        while (!CurrentPet.IsHave);
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
        while (!CurrentPet.IsHave);
    }

    private void OnClickLeftTransformButton()
    {
        if (_transformIndex - 1 < 0)
        {
            _transformIndex = _maxTransformIndex;
        }
        --_transformIndex;

        if (_doTransformScale)
        {
            TransformPetScale(_transformIndex);
        }
        else
        {
            TransformPetChildAsset(_transformIndex);
        }

        ShowTransformOption(_transformIndex);
    }

    private void OnClickRightTransformButton()
    {
        if (_transformIndex + 1 >= _ui.PetList.Length)
        {
            _transformIndex = -1;
        }
        ++_transformIndex;

        if (_doTransformScale)
        {
            TransformPetScale(_transformIndex);
        }
        else
        {
            TransformPetChildAsset(_transformIndex);
        }

        ShowTransformOption(_transformIndex);
    }

    private void TransformPetChildAsset(int index)
    {
        for(int i = 0; i < CurrentPet.Prefab.transform.childCount; ++i)
        {
            CurrentPet.Prefab.transform.GetChild(i).gameObject.SetActive(false);
        }

        CurrentPet.Prefab.transform.GetChild(index).gameObject.SetActive(true);
    }

    private static readonly float[] TRANSFORM_SCALE = new float[3] { 0.3f, 0.5f, 1f };
    private void TransformPetScale(int index)
    {
        CurrentPet.Prefab.transform.localScale = TRANSFORM_SCALE[index] * Vector3.one;
    }

    private void ShowCurrentPet()
    {
        TogglePetObject(CurrentPet.Prefab);

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
        if (CurrentPet.Prefab.transform.GetComponentInChildren<Pet>().PetEvolutionCount == EPetEvolutionCount.NONE)
        {
            _maxTransformIndex = TRANSFORM_SCALE.Length;

            _doTransformScale = true;
        }
        else
        {
            _maxTransformIndex = CurrentPet.Prefab.transform.childCount;

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
            // 임시
            _petTransformOption.text = CurrentPet.Prefab.transform.GetChild(index).name;
        }
    }
}
