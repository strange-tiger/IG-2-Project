using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;

public class CostomizeMenu : MonoBehaviour
{

    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _equipButton;
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _leftMaterialButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] Button _rightMaterialButton;
    [SerializeField] GameObject _noneLight;
    [SerializeField] Material _noneMaterial;
    public CostomizeData _costomizeDatas;
    public UserCostomizeData _userCostomizeData;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private int _setAvatarNum;
    private int _equipNum;
    private int _setMaterialNum;
    private string _saveString;

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

        _purchaseButton.onClick.RemoveListener(PurchaseButton);
        _purchaseButton.onClick.AddListener(PurchaseButton);

        _equipButton.onClick.RemoveListener(EquipButton);
        _equipButton.onClick.AddListener(EquipButton);

      
    }

    void Start()
    {

        MySqlSetting.Init();
        //MySqlSetting.AddNewCharacter("name", "1");
        
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname,"name",Asset.EcharacterdbColumns.AvatarData).Split(',');
        for(int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCostomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
            Debug.Log(avatarData[i] + $"{i}");
            Debug.Log(avatarData.Length);
        }
        _userCostomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor));
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        
        for(int i = 0; i < _userCostomizeData.AvatarState.Length - 1; ++i)
        {
            if(_userCostomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                _equipNum = i;
                break;
            }
        }
        _setMaterialNum = _userCostomizeData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setAvatarNum];
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

    void EquipButton()
    {
        if (_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.HAVE)
        {
            _userCostomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCostomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;
            _userCostomizeData.UserMaterial[0] = _setMaterialNum;
        }
       

        for (int i = 0; i < _userCostomizeData.AvatarState.Length; ++i)
        {
            
                _saveString += _userCostomizeData.AvatarState[i].ToString() + ',';
            
        }
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor, _userCostomizeData.UserMaterial[0]);
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname,"name",Asset.EcharacterdbColumns.AvatarData, _saveString);
        _saveString = null;
    }

    void LeftAvartarButton()
    {
        if(_setAvatarNum == 0)
        {
            _setAvatarNum = _costomizeDatas.AvatarGameObject.Length - 1;
        }
        else
        {
            _setAvatarNum -= 1;
        }
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setAvatarNum];
        
        if(_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
    }

    void RightAvatarButton()
    {
        if (_setAvatarNum == _costomizeDatas.AvatarGameObject.Length - 1)
        {
            _setAvatarNum = 0;
        }
        else
        {
            _setAvatarNum += 1;
        }
        
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setAvatarNum];

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
    }

    void LeftMaterialButton()
    {
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _costomizeDatas.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

         if (_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
            {
                _skinnedMeshRenderer.material = _noneMaterial;
            }
            else
            {
                _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            }
    }
    void RightMaterialButton()
    {
        if (_setMaterialNum == _costomizeDatas.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        if (_userCostomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
        }
    }

    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _purchaseButton.onClick.RemoveListener(PurchaseButton);
        _equipButton.onClick.RemoveListener(EquipButton);
    }
}
