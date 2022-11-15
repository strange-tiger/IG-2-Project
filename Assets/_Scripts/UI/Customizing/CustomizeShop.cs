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
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] GameObject _smMeshRendererObject;
    [SerializeField] GameObject _characterMeshRendererObject;


    public CustomizeData _costomizeDatas;
    public UserCustomizeData _maleUserCostomizeData;
    public UserCustomizeData _femaleUserCostomizeData;
    public UserCustomizeData _userCostomizeData;

    private string _playerNickname;
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

        MySqlSetting.Init();

        // ������ Ȯ����.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // ������ Ȯ���Ͽ� �´� �����͸� �ҷ���
        if (_isFemale)
        {
            _userCostomizeData = _femaleUserCostomizeData;
        }
        else
        {
            _userCostomizeData = _maleUserCostomizeData;
        }

        // �ش� ������ �ƹ�Ÿ �����͸� �ҷ���
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // �ҷ��� �ƹ�Ÿ �����͸� ��ũ���ͺ������Ʈ�� �־���.
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCostomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // ������ �� �����͸� �ҷ���
        _userCostomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        // �������̾��� �ƹ�Ÿ�� �����͸� �ҷ���.
        for (int i = 0; i < _userCostomizeData.AvatarState.Length - 1; ++i)
        {
            if (_userCostomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                break;
            }
        }
        // Material�� ������ �ƹ�Ÿ �����͸� Ŀ���͸����� â�� �����Ŵ
        _setMaterialNum = _userCostomizeData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _userCostomizeData.AvatarMesh[_setAvatarNum];

        // �������� �ƹ�Ÿ�� �̸��� ������ �����Ŵ.
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


    private void RootSet()
    {
        if (_setAvatarNum <= 9 && _setAvatarNum >= 7)
        {
            _smMeshRendererObject.SetActive(true);
            _characterMeshRendererObject.SetActive(false);
            _skinnedMeshRenderer = _smMeshRenderer;
        }
        else
        {
            _smMeshRendererObject.SetActive(false);
            _characterMeshRendererObject.SetActive(true);
            _skinnedMeshRenderer = _characterMeshRenderer;
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


