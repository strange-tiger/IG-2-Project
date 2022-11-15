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


    public CustomizeData _customizeDatas;
    public UserCustomizeData _maleUserCustomizeData;
    public UserCustomizeData _femaleUserCustomizeData;
    public UserCustomizeData _userCustomizeData;

    private Queue<int> _haveAvatar = new Queue<int>();
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

        // 성별을 확인함.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // 성별을 확인하여 맞는 데이터를 불러옴
        if (_isFemale)
        {
            _userCustomizeData = _femaleUserCustomizeData;
        }
        else
        {
            _userCustomizeData = _maleUserCustomizeData;
        }

        // 해당 유저의 아바타 데이터를 불러옴
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 불러온 아바타 데이터를 스크립터블오브젝트에 넣어줌.
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // 유저의 색 데이터를 불러옴
        _userCustomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        // 착용중이었던 아바타의 데이터를 불러옴.
        for (int i = 0; i < _userCustomizeData.AvatarState.Length - 1; ++i)
        {
            if (_userCustomizeData.AvatarState[i] == EAvatarState.NONE)
            {
                _setAvatarNum = i;
                _haveAvatar.Enqueue(i);
            }

        }

        RootSet();

        // Material과 유저의 아바타 데이터를 커스터마이즈 창에 적용시킴
        _setMaterialNum = _userCustomizeData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        // 상점에서 아바타의 이름과 가격을 적용시킴.
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();
    }

    void PurchaseButton()
    {
        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.HAVE;
            _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
        else
        {
            return;
        }
    }

    private void Queueing()
    {
        _setAvatarNum = _haveAvatar.Peek();
        _haveAvatar.Enqueue(_setAvatarNum);
        _haveAvatar.Dequeue();
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
        Queueing();

        RootSet();

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();
    }

    void RightAvatarButton()
    {
        Queueing();

        RootSet();

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();
    }



    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _purchaseButton.onClick.RemoveListener(PurchaseButton);
    }
}


