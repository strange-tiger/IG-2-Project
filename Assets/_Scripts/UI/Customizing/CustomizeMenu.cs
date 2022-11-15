using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Asset.MySql;
using TMPro;
using Photon.Pun;
public class CustomizeMenu : MonoBehaviourPun
{


    [SerializeField] Button _equipButton;
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _leftMaterialButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] Button _rightMaterialButton;
    [SerializeField] TextMeshProUGUI _avatarName;
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] GameObject _smMeshRendererObject;
    [SerializeField] GameObject _characterMeshRendererObject;


    public CustomizeData _customizeDatas;
    public UserCustomizeData _maleUserCustomizeData;
    public UserCustomizeData _femaleUserCustomizeData;
    public UserCustomizeData _userCustomizeData;

    private PlayerNetworking _playerNetworking;
    private Queue<int> _haveAvatar = new Queue<int>();
    private int _setAvatarNum;
    private int _equipNum;
    private int _setMaterialNum;
    private string _saveString;
    private bool _isFemale;
    private string _playerNickname;

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

        if(photonView.IsMine)
        {
            _playerNetworking = FindObjectOfType<PlayerNetworking>();
            _playerNickname = _playerNetworking.MyNickname;
        }
    }

    void Start()
    {

        MySqlSetting.Init();

        // 성별을 확인함.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

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
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');
        
        // 불러온 데이터를 스크립터블 오브젝트에 넣어줌
        for(int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB에 저장되어 있던 아바타의 Material을 불러옴
        _userCustomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));
        
        // 아바타의 정보를 돌면서 장착중이던 아바타를 찾아냄.
        for(int i = 0; i < _userCustomizeData.AvatarState.Length - 1; ++i)
        {

            if(_userCustomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                _equipNum = i;
                _haveAvatar.Enqueue(i);
            }
            else if(_userCustomizeData.AvatarState[i] == EAvatarState.HAVE)
            {
                _haveAvatar.Enqueue(i);
            }
        }

        RootSet();

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];

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

        RootSet();

        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
                _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
        }

        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor, _userCustomizeData.UserMaterial[0]);
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData, _saveString);
        _saveString = null;

        EventSystem.current.SetSelectedGameObject(null);

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

    private void Queueing()
    {
        _setAvatarNum = _haveAvatar.Peek();
        _haveAvatar.Enqueue(_setAvatarNum);
        _haveAvatar.Dequeue();
    }


    void LeftAvartarButton()
    {

        Queueing();

        RootSet();

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        EventSystem.current.SetSelectedGameObject(null);


    }

    void RightAvatarButton()
    {
        Queueing();

        RootSet();

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        EventSystem.current.SetSelectedGameObject(null);


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

        EventSystem.current.SetSelectedGameObject(null);

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

        EventSystem.current.SetSelectedGameObject(null);
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
