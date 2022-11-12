using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;
public class CustomizeMenu : MonoBehaviour
{


    [SerializeField] Button _equipButton;
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _leftMaterialButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] Button _rightMaterialButton;
    [SerializeField] GameObject _noneLight;
    [SerializeField] Material _noneMaterial;

    public CustomizeData _customizeDatas;
    public UserCustomizeData _maleUserCustomizeData;
    public UserCustomizeData _femaleUserCustomizeData;
    public UserCustomizeData _userCustomizeData;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private int _setAvatarNum;
    private int _equipNum;
    private int _setMaterialNum;
    private string _saveString;
    private bool _isFemale;

    private void OnEnable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _leftAvatarButton.onClick.AddListener(LeftAvartarButton);

        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _rightAvatarButton.onClick.AddListener(RightAvatarButton);

        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _leftMaterialButton.onClick.AddListener(LeftMaterialButton);

        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _rightMaterialButton.onClick.AddListener(RightMaterialButton);

        _equipButton.onClick.RemoveListener(EquipButton);
        _equipButton.onClick.AddListener(EquipButton);

    }

    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        MySqlSetting.Init();
        //MySqlSetting.AddNewCharacter("name", "1");

        // 성별을 확인함.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.Gender));

        // 성별에 맞는 데이터를 불러옴
        if(_isFemale)
        {
            _userCustomizeData = _femaleUserCustomizeData;
        }
        else
        {
            _userCustomizeData = _maleUserCustomizeData;
        }

        // DB에 저장되어 있던 아바타 데이터를 불러옴
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname,"name",Asset.EcharacterdbColumns.AvatarData).Split(',');
        
        // 불러온 데이터를 스크립터블 오브젝트에 넣어줌
        for(int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB에 저장되어 있던 아바타의 Material을 불러옴
        _userCustomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor));
        
        // 아바타의 정보를 돌면서 장착중이던 아바타를 찾아냄.
        for(int i = 0; i < _userCustomizeData.AvatarState.Length - 1; ++i)
        {
            if(_userCustomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                _equipNum = i;
                break;
            }
        }

        // 장착중이던 아이템과 Material을 적용시킴.
        _setMaterialNum = _userCustomizeData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];

    }



    void EquipButton()
    {
        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.HAVE)
        {
            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;
            _userCustomizeData.UserMaterial[0] = _setMaterialNum;
        }
       

        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
            
                _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
            
        }
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor, _userCustomizeData.UserMaterial[0]);
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname,"name",Asset.EcharacterdbColumns.AvatarData, _saveString);
        _saveString = null;
    }

    void LeftAvartarButton()
    {
        if(_setAvatarNum == 0)
        {
            _setAvatarNum = _userCustomizeData.AvatarMesh.Length - 1;
        }
        else
        {
            _setAvatarNum -= 1;
        }
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];
        
        if(_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
    }

    void RightAvatarButton()
    {
        if (_setAvatarNum == _userCustomizeData.AvatarMesh.Length - 1)
        {
            _setAvatarNum = 0;
        }
        else
        {
            _setAvatarNum += 1;
        }
        
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
    }

    void LeftMaterialButton()
    {
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _customizeDatas.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

         if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
            {
                _skinnedMeshRenderer.material = _noneMaterial;
            }
            else
            {
                _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];
            }
    }
    void RightMaterialButton()
    {
        if (_setMaterialNum == _customizeDatas.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
        }
        else
        {
            _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];
        }
    }

    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _equipButton.onClick.RemoveListener(EquipButton);
    }
}
