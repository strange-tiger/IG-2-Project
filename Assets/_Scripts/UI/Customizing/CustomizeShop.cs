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

    [Header("Button")]
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] Button[] _avatarInfoButton;

    [Header("PopUp")]
    [SerializeField] GameObject _purchasePopUp;
    [SerializeField] Button _purchasePopUpCloseButton;
    [SerializeField] Button _purchaseButton;
    [SerializeField] TextMeshProUGUI _haveGoldText;
    [SerializeField] TextMeshProUGUI _askPurchaseAvatarText;
    [SerializeField] TextMeshProUGUI _purchasePopUpCloseButtonText;

    [Header("Purchase Complete PopUp")]
    [SerializeField] GameObject _purchaseCompletePopUp;
    [SerializeField] Button _purchaseCompletePopUpCloseButton;

    [Header("Current Avatar Info")]
    [SerializeField] TextMeshProUGUI _currentAvatarName;
    [SerializeField] TextMeshProUGUI _currentAvatarValue;

    [Header("Current Avatar")]
    [SerializeField] SkinnedMeshRenderer _currentSkinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _currentSmMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _currentCharacterMeshRenderer;
    [SerializeField] GameObject _currentSmMeshRendererObject;
    [SerializeField] GameObject _currentCharacterMeshRendererObject;

    [Header("Avatar Info")]
    [SerializeField] TextMeshProUGUI[] _avatarName;
    [SerializeField] TextMeshProUGUI[] _avatarValue;
    [SerializeField] GameObject[] _avatarPanel;

    [Header("Avatar")]
    [SerializeField] SkinnedMeshRenderer[] _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer[] _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer[] _characterMeshRenderer;
    [SerializeField] GameObject[] _smMeshRendererObject;
    [SerializeField] GameObject[] _characterMeshRendererObject;

    [Header("Purchase Avatar Info")]
    [SerializeField] TextMeshProUGUI _purchaseAvatarName;

    [Header("Purchase Avatar")]
    [SerializeField] SkinnedMeshRenderer _purchaseSkinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _purchaseSmMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _purchaseCharacterMeshRenderer;
    [SerializeField] GameObject _purchaseSmMeshRendererObject;
    [SerializeField] GameObject _purchaseCharacterMeshRendererObject;

    [Header("Avatar Data")]
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


        _avatarInfoButton[0].onClick.RemoveListener(FirstAvatarInfo);
        _avatarInfoButton[0].onClick.AddListener(FirstAvatarInfo);

        _avatarInfoButton[1].onClick.RemoveListener(SecondAvatarInfo);
        _avatarInfoButton[1].onClick.AddListener(SecondAvatarInfo);

        _avatarInfoButton[2].onClick.RemoveListener(ThirdAvatarInfo);
        _avatarInfoButton[2].onClick.AddListener(ThirdAvatarInfo);

        _purchasePopUpCloseButton.onClick.RemoveListener(PopUpClose);
        _purchasePopUpCloseButton.onClick.AddListener(PopUpClose);

        _purchaseCompletePopUpCloseButton.onClick.RemoveListener(PurchaseCompletePopUpClose);
        _purchaseCompletePopUpCloseButton.onClick.AddListener(PurchaseCompletePopUpClose);


        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        _playerNickname = "aaa";


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

        ShopIndexRootSet();

        _setAvatarNum = _notHaveAvatarList[_startNum];

        InitRootSet();
        // 현재 아바타 정보 저장
        _setMaterialNum = _userCustomizeData.UserMaterial;
        _currentSkinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_equipNum];
        _currentAvatarName.text = _userCustomizeData.AvatarName[_equipNum];
        _currentAvatarValue.text = _userCustomizeData.AvatarValue[_equipNum].ToString();


        for (int i = 0; i < _skinnedMeshRenderer.Length; ++i)
        {
            _skinnedMeshRenderer[i].sharedMesh = _userCustomizeData.AvatarMesh[_notHaveAvatarList[_startNum + i]];
            _avatarName[i].text = _userCustomizeData.AvatarName[_notHaveAvatarList[_startNum + i]];
            _avatarValue[i].text = _userCustomizeData.AvatarValue[_notHaveAvatarList[_startNum + i]].ToString();
        }
    }

    void PurchaseButton()
    {

        if (_playerNetworking.GetComponent<PhotonView>().IsMine)
        {
            MySqlSetting.UseGold(_playerNickname, _userCustomizeData.AvatarValue[_setAvatarNum]);

            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;

            // _notHaveAvatarList.Remove(_setAvatarNum);

            ShopIndexRootSet();

            for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
            {
                _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
            }

            MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData, _saveString);

            _saveString = null;

            _playerCustomize = _playerNetworking.GetComponentInChildren<PlayerCustomize>();
            _playerCustomize.photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, _isFemale);

            _purchaseCompletePopUp.SetActive(true);
            _purchasePopUp.SetActive(false);

            if (_notHaveAvatarList.Count == 0)
            {
                return;
            }

            _notHaveAvatarList.Clear();

            for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
            {
                if (_userCustomizeData.AvatarState[i] == EAvatarState.NONE)
                {
                    _notHaveAvatarList.Add(i);
                }
            }
        }

        EventSystem.current.SetSelectedGameObject(null);
    }


    private void AvatarShopPage()
    {

        for (int i = 0; i < _skinnedMeshRenderer.Length; ++i)
        {

            if (_startNum + i <= _notHaveAvatarList.Count - 1)
            {
                _avatarPanel[i].SetActive(true);
                _skinnedMeshRenderer[i].sharedMesh = _userCustomizeData.AvatarMesh[_notHaveAvatarList[_startNum + i]];
                _avatarName[i].text = _userCustomizeData.AvatarName[_notHaveAvatarList[_startNum + i]];
                _avatarValue[i].text = _userCustomizeData.AvatarValue[_notHaveAvatarList[_startNum + i]].ToString();
            }
            else
            {
                _avatarPanel[i].SetActive(false);
            }
        }
    }



    private void ShopIndexRootSet()
    {

        // 상점 인덱스의 RootSet
        for (int i = 0; i < _skinnedMeshRenderer.Length; ++i)
        {
            if (_startNum + i <= _notHaveAvatarList.Count - 1)
            {
                if (_notHaveAvatarList[_startNum + i] <= 9 && _notHaveAvatarList[_startNum + i] >= 7)
                {
                    _smMeshRendererObject[i].SetActive(true);
                    _characterMeshRendererObject[i].SetActive(false);
                    _skinnedMeshRenderer = _smMeshRenderer;
                }
                else
                {
                    _smMeshRendererObject[i].SetActive(false);
                    _characterMeshRendererObject[i].SetActive(true);
                    _skinnedMeshRenderer = _characterMeshRenderer;
                }
            }
        }

    }

    private void InitRootSet()
    {

        // 현재 아바타의 RootSet
        if (_equipNum <= 9 && _equipNum >= 7)
        {
            _currentSmMeshRendererObject.SetActive(true);
            _currentCharacterMeshRendererObject.SetActive(false);
            _currentSkinnedMeshRenderer = _currentSmMeshRenderer;
        }
        else
        {
            _currentSmMeshRendererObject.SetActive(false);
            _currentCharacterMeshRendererObject.SetActive(true);
            _currentSkinnedMeshRenderer = _currentCharacterMeshRenderer;
        }

    }

    void LeftAvartarButton()
    {

        if (_startNum == 0)
        {
            _startNum = ((_notHaveAvatarList.Count - 1) / 3) * 3;
        }
        else
        {
            _startNum -= 3;
        }

        Debug.Log(_startNum);

        ShopIndexRootSet();
        AvatarShopPage();




        EventSystem.current.SetSelectedGameObject(null);

    }

    void RightAvatarButton()
    {
        if (_startNum >= _notHaveAvatarList.Count - 1)
        {
            _startNum = 0;
        }
        else
        {
            _startNum += 3;
        }

        Debug.Log(_startNum);

        ShopIndexRootSet();

        AvatarShopPage();


        EventSystem.current.SetSelectedGameObject(null);

    }

    private void AvatarInfo(int index)
    {

        if (_notHaveAvatarList[index] <= 9 && _notHaveAvatarList[index] >= 7)
        {
            _purchaseSmMeshRendererObject.SetActive(true);
            _purchaseCharacterMeshRendererObject.SetActive(false);
            _purchaseSkinnedMeshRenderer = _purchaseSmMeshRenderer;
        }
        else
        {
            _purchaseSmMeshRendererObject.SetActive(false);
            _purchaseCharacterMeshRendererObject.SetActive(true);
            _purchaseSkinnedMeshRenderer = _purchaseCharacterMeshRenderer;
        }

        _haveGoldText.text = "Gold : " + MySqlSetting.CheckHaveGold(_playerNickname).ToString();

        if (MySqlSetting.CheckHaveGold(_playerNickname) >= _userCustomizeData.AvatarValue[_notHaveAvatarList[index]])
        {
            _purchaseButton.image.color = _enoughGoldColor;
            _purchaseButton.interactable = true;
            _askPurchaseAvatarText.text = $"가격은 {_userCustomizeData.AvatarValue[_notHaveAvatarList[index]]}입니다. 구매하시겠습니까?";
            _purchasePopUpCloseButtonText.text = "조금만 더 둘러볼게요.";

        }
        else
        {
            _purchaseButton.image.color = _notEnoughGoldColor;
            _purchaseButton.interactable = false;
            _askPurchaseAvatarText.text = $"돈이 부족하여 구매할 수 없습니다.";
            _purchasePopUpCloseButtonText.text = "돌아가기";

        }


        _purchaseSkinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_notHaveAvatarList[index]];
        _purchaseAvatarName.text = _userCustomizeData.AvatarName[_notHaveAvatarList[index]];

        _setAvatarNum = _notHaveAvatarList[index];

        _purchasePopUp.SetActive(true);
    }

    private void FirstAvatarInfo() => AvatarInfo(_startNum);
    private void SecondAvatarInfo() => AvatarInfo(_startNum + 1);
    private void ThirdAvatarInfo() => AvatarInfo(_startNum + 2);

    private void PopUpClose() => _purchasePopUp.SetActive(false);
    private void PurchaseCompletePopUpClose() => _purchaseCompletePopUp.SetActive(false);
    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _purchaseButton.onClick.RemoveListener(PurchaseButton);
        _avatarInfoButton[0].onClick.RemoveListener(FirstAvatarInfo);
        _avatarInfoButton[1].onClick.RemoveListener(SecondAvatarInfo);
        _avatarInfoButton[2].onClick.RemoveListener(ThirdAvatarInfo);
        _purchasePopUpCloseButton.onClick.RemoveListener(PopUpClose);
        _purchaseCompletePopUpCloseButton.onClick.RemoveListener(PurchaseCompletePopUpClose);

        _notHaveAvatarList.Clear();

        _playerNetworking = null;
        _playerCustomize = null;
    }
}


