using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using System;
using UnityEngine.UI;
using TMPro;

public class CustomizeShop : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI _avatarName;
    [SerializeField] TextMeshProUGUI _avatarValue;
    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] GameObject _noneLight;
    [SerializeField] Material _noneMaterial;

    public CustomizeData _costomizeDatas;
    public UserCustomizeData _maleUserCostomizeData;
    public UserCustomizeData _femaleUserCostomizeData;
    public UserCustomizeData _userCostomizeData;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private int _setAvatarNum;
    private int _setMaterialNum;
    private bool _isFemale;

    private void OnEnable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _leftAvatarButton.onClick.AddListener(LeftAvartarButton);

        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _rightAvatarButton.onClick.AddListener(RightAvatarButton);

        _purchaseButton.onClick.RemoveListener(PurchaseButton);
        _purchaseButton.onClick.AddListener(PurchaseButton);

    }

    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        MySqlSetting.Init();

        // 성별을 확인함.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.Gender));

        // 성별을 확인하여 맞는 데이터를 불러옴
        if (_isFemale)
        {
            _userCostomizeData = _femaleUserCostomizeData;
        }
        else
        {
            _userCostomizeData = _maleUserCostomizeData;
        }

        // 해당 유저의 아바타 데이터를 불러옴
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 불러온 아바타 데이터를 스크립터블오브젝트에 넣어줌.
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCostomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // 유저의 색 데이터를 불러옴
        _userCostomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor));

        // 착용중이었던 아바타의 데이터를 불러옴.
        for (int i = 0; i < _userCostomizeData.AvatarState.Length - 1; ++i)
        {
            if (_userCostomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                break;
            }
        }
        // Material과 유저의 아바타 데이터를 커스터마이즈 창에 적용시킴
        _setMaterialNum = _userCostomizeData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _userCostomizeData.AvatarMesh[_setAvatarNum];

        // 상점에서 아바타의 이름과 가격을 적용시킴.
        _avatarName.text = _userCostomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCostomizeData.AvatarValue[_setAvatarNum].ToString();
    }

    void PurchaseButton()
    {
        if (_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _userCostomizeData.AvatarState[_setAvatarNum] = EAvatarState.HAVE;
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
        else
        {
            return;
        }
    }



    void LeftAvartarButton()
    {
        if (_setAvatarNum == 0)
        {
            _setAvatarNum = _userCostomizeData.AvatarMesh.Length - 1;
        }
        else
        {
            _setAvatarNum -= 1;
        }
        _skinnedMeshRenderer.sharedMesh = _userCostomizeData.AvatarMesh[_setAvatarNum];

        if (_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
        _avatarName.text = _userCostomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCostomizeData.AvatarValue[_setAvatarNum].ToString();
    }

    void RightAvatarButton()
    {
        if (_setAvatarNum == _userCostomizeData.AvatarMesh.Length - 1)
        {
            _setAvatarNum = 0;
        }
        else
        {
            _setAvatarNum += 1;
        }

        _skinnedMeshRenderer.sharedMesh = _userCostomizeData.AvatarMesh[_setAvatarNum];

        if (_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
        _avatarName.text = _userCostomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCostomizeData.AvatarValue[_setAvatarNum].ToString();
    }



    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);

        _purchaseButton.onClick.RemoveListener(PurchaseButton);
    }
}


