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
    [SerializeField] private Button _leftAvatarButton;
    [SerializeField] private Button _rightAvatarButton;
    [SerializeField] private Button[] _avatarInfoButton;

    [Header("PopUp")]
    [SerializeField] private GameObject _purchasePopUp;
    [SerializeField] private Button _purchasePopUpCloseButton;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private TextMeshProUGUI _haveGoldText;
    [SerializeField] private TextMeshProUGUI _askPurchaseAvatarText;
    [SerializeField] private TextMeshProUGUI _purchasePopUpCloseButtonText;

    [Header("Purchase Complete PopUp")]
    [SerializeField] private GameObject _purchaseCompletePopUp;
    [SerializeField] private Button _purchaseCompletePopUpCloseButton;

    [Header("Current Avatar Info")]
    [SerializeField] private TextMeshProUGUI _currentAvatarName;
    [SerializeField] private TextMeshProUGUI _currentAvatarNickname;
    [SerializeField] private TextMeshProUGUI _currentGold;

    [Header("Current Avatar")]
    [SerializeField] private AvatarMaterialData _currentAvatarMaterialData;
    [SerializeField] private Image _currentAvatarImage;

    [Header("Avatar Info")]
    [SerializeField] private TextMeshProUGUI[] _avatarName;
    [SerializeField] private TextMeshProUGUI[] _avatarNickname;
    [SerializeField] private TextMeshProUGUI[] _avatarPrice;
    [SerializeField] private GameObject[] _avatarPanel;

    [Header("Avatar")]
    [SerializeField] private AvatarMaterialData[] _avatarMaterialData;
    [SerializeField] private Image[] _avatarImage;

    [Header("Purchase Avatar Info")]
    [SerializeField] private TextMeshProUGUI _purchaseAvatarName;
    [SerializeField] private TextMeshProUGUI _purchaseAvatarNickname;
    [SerializeField] private TextMeshProUGUI _purchaseAvatarInfo;

    [Header("Purchase Avatar")]
    [SerializeField] private AvatarMaterialData _purchaseAvatarMaterialData;
    [SerializeField] private Image _purchaseAvatarImage;

    [Header("Avatar Data")]
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
    private int _equipMaterialNum;
    private int _startNum;
    private int _playerGold;

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

        _playerNickname = _playerNetworking.MyNickname;

        _playerGold = MySqlSetting.CheckHaveGold(_playerNickname);

        AvatarShopInit();
    }

    private void AvatarShopInit()
    {

        //MySqlSetting.Init();

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
        _equipMaterialNum = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));


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

        // 현재 아바타 정보 저장

        // Material 저장 스크립터블 오브젝트
        _currentAvatarMaterialData = _userCustomizeData.AvatarMaterial[_equipNum];
        // 현재 아바타 보여줄 스프라이트
        _currentAvatarImage.sprite = _currentAvatarMaterialData.AvatarImage[_equipMaterialNum];
        // 현재 아바타의 이름
        _currentAvatarName.text = _userCustomizeData.AvatarName[_equipNum];
        // 현재 아바타의 닉네임
        _currentAvatarNickname.text = _userCustomizeData.AvatarNickname[_equipNum];
        // 현재 아바타의 가격
        _currentGold.text = _playerGold.ToString();


        _setMaterialNum = 0;

        for (int i = 0; i < _avatarMaterialData.Length; ++i)
        {
            _avatarMaterialData[i] = _userCustomizeData.AvatarMaterial[_notHaveAvatarList[_startNum + i]];
            _avatarImage[i].sprite = _avatarMaterialData[i].AvatarImage[0];
            _avatarName[i].text = _userCustomizeData.AvatarName[_notHaveAvatarList[_startNum + i]];
            _avatarNickname[i].text = _userCustomizeData.AvatarNickname[_notHaveAvatarList[_startNum + i]];
            _avatarPrice[i].text = _userCustomizeData.AvatarValue[_notHaveAvatarList[_startNum + i]].ToString();
        }
    }



    private void AvatarShopPage()
    {

        for (int i = 0; i < _avatarMaterialData.Length; ++i)
        {

            if (_startNum + i <= _notHaveAvatarList.Count - 1)
            {
                _avatarPanel[i].SetActive(true);
                _avatarMaterialData[i] = _userCustomizeData.AvatarMaterial[_notHaveAvatarList[_startNum + i]];
                _avatarImage[i].sprite = _avatarMaterialData[i].AvatarImage[0];
                _avatarName[i].text = _userCustomizeData.AvatarName[_notHaveAvatarList[_startNum + i]];
                _avatarNickname[i].text = _userCustomizeData.AvatarNickname[_notHaveAvatarList[_startNum + i]];
                _avatarPrice[i].text = _userCustomizeData.AvatarValue[_notHaveAvatarList[_startNum + i]].ToString();
            }
            else
            {
                _avatarPanel[i].SetActive(false);
            }
        }
    }

    private void LeftAvartarButton()
    {

        if (_startNum == 0)
        {
            _startNum = ((_notHaveAvatarList.Count - 1) / 3) * 3;
        }
        else
        {
            _startNum -= 3;
        }

        AvatarShopPage();

        EventSystem.current.SetSelectedGameObject(null);

    }

    private void RightAvatarButton()
    {
        if (_startNum >= _notHaveAvatarList.Count - 1)
        {
            _startNum = 0;
        }
        else
        {
            _startNum += 3;
        }

        AvatarShopPage();

        EventSystem.current.SetSelectedGameObject(null);

    }

    private void AvatarInfo(int index)
    {

        _haveGoldText.text = "Gold : " + _playerGold.ToString();
        _purchaseAvatarMaterialData = _userCustomizeData.AvatarMaterial[_notHaveAvatarList[index]];
        _purchaseAvatarImage.sprite = _purchaseAvatarMaterialData.AvatarImage[0];
        _purchaseAvatarName.text = _userCustomizeData.AvatarName[_notHaveAvatarList[index]];
        _purchaseAvatarNickname.text = _userCustomizeData.AvatarNickname[_notHaveAvatarList[index]];
        _purchaseAvatarInfo.text = _userCustomizeData.AvatarInfo[_notHaveAvatarList[index]];
        _setAvatarNum = _notHaveAvatarList[index];


        if (_playerGold >= _userCustomizeData.AvatarValue[_notHaveAvatarList[index]])
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


        _purchasePopUp.SetActive(true);
    }

    private void FirstAvatarInfo() => AvatarInfo(_startNum);
    private void SecondAvatarInfo() => AvatarInfo(_startNum + 1);
    private void ThirdAvatarInfo() => AvatarInfo(_startNum + 2);

    private void PurchaseButton()
    {
        if (_playerNetworking.GetComponent<PhotonView>().IsMine)
        {
            MySqlSetting.UseGold(_playerNickname, _userCustomizeData.AvatarValue[_setAvatarNum]);

            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;

            // _notHaveAvatarList.Remove(_setAvatarNum);

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

        _playerGold = MySqlSetting.CheckHaveGold(_playerNickname);

        _currentGold.text = _playerGold.ToString();

        EventSystem.current.SetSelectedGameObject(null);
    }

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


