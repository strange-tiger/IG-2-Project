using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Asset.MySql;
using Photon.Pun;

public class CustomizeShop : MonoBehaviourPun
{

    [SerializeField] TextMeshProUGUI _avatarName;
    [SerializeField] TextMeshProUGUI _avatarValue;
    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] GameObject _smMeshRendererObject;
    [SerializeField] GameObject _characterMeshRendererObject;


    public CustomizeData _customizeDatas;
    public UserCustomizeData _maleUserCustomizeData;
    public UserCustomizeData _femaleUserCustomizeData;
    public UserCustomizeData _userCustomizeData;


    private Color _enoughGoldColor = new Color(255, 212, 0);
    private Color _notEnoughGoldColor = new Color(128, 128, 128);

    private List<int> _notHaveAvatarList = new List<int>();
    private PlayerCustomize _playerCustomize;
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;
    private string _playerNickname;
    private string _saveString;
    private int _setAvatarNum;
    private int _setMaterialNum;
    private int _equipNum;
    private int _startNum;
    private bool _isFemale;

    private void OnEnable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _leftAvatarButton.onClick.AddListener(LeftAvartarButton);

        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _rightAvatarButton.onClick.AddListener(RightAvatarButton);

        _purchaseButton.onClick.RemoveListener(PurchaseButton);
        _purchaseButton.onClick.AddListener(PurchaseButton);



        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        _playerNickname = _playerNetworking.MyNickname;


        AvatarShopInit();
    }

    private void AvatarShopInit()
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
        _userCustomizeData.UserMaterial = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        // 착용중이었던 아바타의 데이터를 불러옴.
        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
            if (_userCustomizeData.AvatarState[i] == EAvatarState.NONE)
            {
                _notHaveAvatarList.Add(i);
            }
            if (_userCustomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _equipNum = i;
            }
        }
        _startNum = 0;
         _setAvatarNum = _notHaveAvatarList[_startNum];

        RootSet();

        // Material과 유저의 아바타 데이터를 커스터마이즈 창에 적용시킴
        _setMaterialNum = _userCustomizeData.UserMaterial;
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        // 상점에서 아바타의 이름과 가격을 적용시킴.
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();
    }

    void PurchaseButton()
    {

        if (_playerNetworking.GetComponent<PhotonView>().IsMine)
        {
            MySqlSetting.UseGold(_playerNickname, _userCustomizeData.AvatarValue[_setAvatarNum]);

            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;

            _notHaveAvatarList.Remove(_setAvatarNum);

            RootSet();

            for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
            {
                _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
            }

            MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData, _saveString);

            _saveString = null;

            _playerCustomize = _playerNetworking.GetComponentInChildren<PlayerCustomize>();
            _playerCustomize.photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, _isFemale);

            if(_notHaveAvatarList.Count == 0)
            {
                return;
            }


            if (_startNum == _notHaveAvatarList.Count)
            {
                _startNum = 0;
                _setAvatarNum = _notHaveAvatarList[_startNum];
            }
            else
            {
                _startNum++;
                _setAvatarNum = _notHaveAvatarList[_startNum];
            }
        }

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

    void LeftAvartarButton()
    {
        if (_startNum == 0)
        {
            _startNum = _notHaveAvatarList.Count - 1;
            _setAvatarNum = _notHaveAvatarList[_startNum];
        }
        else
        {
            _startNum--;
            _setAvatarNum = _notHaveAvatarList[_startNum];
        }

        RootSet();

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();

        if (MySqlSetting.CheckHaveGold(_playerNickname) >= _userCustomizeData.AvatarValue[_setAvatarNum])
        {
            _purchaseButton.image.color = _enoughGoldColor;
            _purchaseButton.interactable = true;
        }
        else
        {
            _purchaseButton.image.color = _notEnoughGoldColor;
            _purchaseButton.interactable = false;

        }

        EventSystem.current.SetSelectedGameObject(null);

    }

    void RightAvatarButton()
    {
        if (_startNum == _notHaveAvatarList.Count - 1)
        {
            _startNum = 0;
            _setAvatarNum = _notHaveAvatarList[_startNum];
        }
        else
        {
            _startNum++;
            _setAvatarNum = _notHaveAvatarList[_startNum];
        }

        RootSet();

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();

        if(MySqlSetting.CheckHaveGold(_playerNickname) >= _userCustomizeData.AvatarValue[_setAvatarNum])
        {
            _purchaseButton.image.color = _enoughGoldColor;
            _purchaseButton.interactable = true;
        }
        else
        {
            _purchaseButton.image.color = _notEnoughGoldColor;
            _purchaseButton.interactable = false;

        }

        EventSystem.current.SetSelectedGameObject(null);

    }



    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _purchaseButton.onClick.RemoveListener(PurchaseButton);

        _notHaveAvatarList.Clear();

        _playerNetworking = null;
        _playerCustomize = null;
    }
}


