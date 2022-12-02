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
    // 아바타 상점의 인덱스와 아바타를 선택하는 버튼
    [Header("Button")]
    [SerializeField] private Button _leftAvatarButton;
    [SerializeField] private Button _rightAvatarButton;
    [SerializeField] private Button[] _avatarInfoButton;

    // 아바타 상점의 아바타 버튼을 누르면 나오는 아바타 정보 팝업
    [Header("PopUp")]
    [SerializeField] private GameObject _purchasePopUp;
    [SerializeField] private Button _purchasePopUpCloseButton;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private TextMeshProUGUI _haveGoldText;
    [SerializeField] private TextMeshProUGUI _askPurchaseAvatarText;
    [SerializeField] private TextMeshProUGUI _purchasePopUpCloseButtonText;

    // 구매를 성공했을 때 출력되는 팝업
    [Header("Purchase Complete PopUp")]
    [SerializeField] private GameObject _purchaseCompletePopUp;
    [SerializeField] private Button _purchaseCompletePopUpCloseButton;

    // 현재 아바타의 정보와 유저의 소지 골드
    [Header("Current Avatar Info")]
    [SerializeField] private TextMeshProUGUI _currentAvatarName;
    [SerializeField] private TextMeshProUGUI _currentAvatarNickname;
    [SerializeField] private TextMeshProUGUI _currentGold;

    // 현재 아바타의 메테리얼 데이터와 아바타 이미지
    [Header("Current Avatar")]
    [SerializeField] private AvatarMaterialData _currentAvatarMaterialData;
    [SerializeField] private Image _currentAvatarImage;

    // 구매할 아바타의 정보
    [Header("Avatar Info")]
    [SerializeField] private TextMeshProUGUI[] _avatarName;
    [SerializeField] private TextMeshProUGUI[] _avatarNickname;
    [SerializeField] private TextMeshProUGUI[] _avatarPrice;
    [SerializeField] private GameObject[] _avatarPanel;

    // 구매할 아바타의 메테리얼 데이터와 이미지
    [Header("Avatar")]
    [SerializeField] private AvatarMaterialData[] _avatarMaterialData;
    [SerializeField] private Image[] _avatarImage;

    // 구매 정보 팝업의 아바타 정보
    [Header("Purchase Avatar Info")]
    [SerializeField] private TextMeshProUGUI _purchaseAvatarName;
    [SerializeField] private TextMeshProUGUI _purchaseAvatarNickname;
    [SerializeField] private TextMeshProUGUI _purchaseAvatarInfo;
    
    // 구매 정보 팝업의 아바타 메테리얼 데이터와 이미지
    [Header("Purchase Avatar")]
    [SerializeField] private AvatarMaterialData _purchaseAvatarMaterialData;
    [SerializeField] private Image _purchaseAvatarImage;

    // 유저에게 적용되는 아바타 커스터마이징 데이터
    [Header("Avatar Data")]
    public UserCustomizeData _maleUserCustomizeData;
    public UserCustomizeData _femaleUserCustomizeData;
    public UserCustomizeData _userCustomizeData;

    // 골드 소지량에 따른 버튼의 색
    private Color _enoughGoldColor = new Color(255, 212, 0);
    private Color _notEnoughGoldColor = new Color(128, 128, 128);

    // 소지 하지 않는 아바타의 리스트
    private List<int> _notHaveAvatarList = new List<int>();

    // UI를 사용하는 유저의 닉네임을 받아오는 PlayerNetworking과 커스텀 정보를 적용시키는 PlayerCustomize
    private PlayerCustomize _playerCustomize;
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;

    // 플레이어의 닉네임과 DB에 저장할 문자열
    private string _playerNickname;
    private string _saveString;

    // 구매할 아바타의 인덱스와 메테리얼 인덱스
    private int _setAvatarNum;
    private int _setMaterialNum;

    // 현재 장착중인 아바타 인덱스와 메테리얼 인덱스
    private int _equipNum;
    private int _equipMaterialNum;

    // 소지하지 않은 아바타 리스트에서 사용할 인덱스
    private int _startNum;

    // 플레이어의 소지 골드
    private int _playerGold;

    // 플레이어의 성별
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


        // UI와 상호작용하는 플레이어를 찾아
        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        // 닉네임을 적용시킴.
        _playerNickname = _playerNetworking.MyNickname;

        // 플레이어의 소지 골드를 DB에서 불러옴
        _playerGold = MySqlSetting.CheckHaveGold(_playerNickname);

        // 아바타 상점을 초기화
        AvatarShopInit();
    }

    /// <summary>
    /// 아바타 상점의 데이터를 초기화하는 메서드.
    /// </summary>
    private void AvatarShopInit()
    {
        // 성별을 확인함.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // 성별을 확인하여 맞는 데이터를 불러옴.
        if (_isFemale)
        {
            _userCustomizeData = _femaleUserCustomizeData;
        }
        else
        {
            _userCustomizeData = _maleUserCustomizeData;
        }

        // 해당 유저의 아바타 데이터를 불러옴.
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 불러온 아바타 데이터를 스크립터블오브젝트에 넣어줌.
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }

        // 유저의 색 데이터를 불러옴.
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


        // 현재 아바타 정보 저장.

        // Material 저장 스크립터블 오브젝트.
        _currentAvatarMaterialData = _userCustomizeData.AvatarMaterial[_equipNum];
        // 현재 아바타 보여줄 스프라이트.
        _currentAvatarImage.sprite = _currentAvatarMaterialData.AvatarImage[_equipMaterialNum];
        // 현재 아바타의 이름.
        _currentAvatarName.text = _userCustomizeData.AvatarName[_equipNum];
        // 현재 아바타의 닉네임.
        _currentAvatarNickname.text = _userCustomizeData.AvatarNickname[_equipNum];
        // 현재 아바타의 가격.
        _currentGold.text = _playerGold.ToString();

        // 리스트의 처음부터 시작함.
        _startNum = 0;

        // 아바타 인덱스를 리스트의 처음으로 초기화함.
        _setAvatarNum = _notHaveAvatarList[_startNum];

        // 가지고 있지 않은 아바타가 없다면 상점을 비우고
        if (_notHaveAvatarList.Count == 0)
        {
            for(int i = 0; i < _avatarPanel.Length; ++i)
            {
                _avatarPanel[i].SetActive(false);
            }

            // 상점 페이지 버튼을 비활성화 시킴.
            _leftAvatarButton.interactable = false;
            _rightAvatarButton.interactable = false;
        }
        else
        {
            _setMaterialNum = 0;

            // 상점 인덱스를 아바타 리스트로 초기화함.
            for (int i = 0; i < _avatarMaterialData.Length; ++i)
            {
                _avatarMaterialData[i] = _userCustomizeData.AvatarMaterial[_notHaveAvatarList[_startNum + i]];
                _avatarImage[i].sprite = _avatarMaterialData[i].AvatarImage[0];
                _avatarName[i].text = _userCustomizeData.AvatarName[_notHaveAvatarList[_startNum + i]];
                _avatarNickname[i].text = _userCustomizeData.AvatarNickname[_notHaveAvatarList[_startNum + i]];
                _avatarPrice[i].text = _userCustomizeData.AvatarValue[_notHaveAvatarList[_startNum + i]].ToString();
            }
        }
    }


    /// <summary>
    /// 아바타 상점의 버튼을 누르면 상점의 페이지를 이동시킴.
    /// </summary>
    private void AvatarShopPage()
    {
        // 한 페이지에 3개의 아바타를 적용시킴.
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
                // 리스트가 더 이상 존재하지 않으면 빈 창으로 출력됨.
                _avatarPanel[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 왼쪽 페이지 버튼을 눌렀을때 상점 페이지 인덱스를 바꿔줌.
    /// </summary>
    private void LeftAvartarButton()
    {
        // 인덱스가 0 이라면 가장 뒤쪽의 페이지로 넘어감.
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

    /// <summary>
    /// 오른쪽 페이지 버튼을 눌렀을 때 상점 페이저 인덱스를 바꿔줌.
    /// </summary>
    private void RightAvatarButton()
    {
        // 페이지의 끝이라면 처음으로 돌아감.
        if (_startNum >= _notHaveAvatarList.Count - 2)
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

    /// <summary>
    /// 아바타의 구매정보 팝업을 초기화해주는 메서드.
    /// </summary>
    /// <param name="index"> 구매정보를 불러올 아바타의 인덱스</param>
    private void AvatarInfo(int index)
    {
        // 플레이어의 소지 골드와 선택한 아바타의 인덱스를 활용하여 해당 아바타의 정보를 불러옴.
        _haveGoldText.text = "Gold : " + _playerGold.ToString();
        _purchaseAvatarMaterialData = _userCustomizeData.AvatarMaterial[_notHaveAvatarList[index]];
        _purchaseAvatarImage.sprite = _purchaseAvatarMaterialData.AvatarImage[0];
        _purchaseAvatarName.text = _userCustomizeData.AvatarName[_notHaveAvatarList[index]];
        _purchaseAvatarNickname.text = _userCustomizeData.AvatarNickname[_notHaveAvatarList[index]];
        _purchaseAvatarInfo.text = _userCustomizeData.AvatarInfo[_notHaveAvatarList[index]];
        _setAvatarNum = _notHaveAvatarList[index];

        // 플레이어의 소지 골드가 아바타의 가격보다 많으면 구매 가능하도록 버튼과 텍스트를 설정.
        if (_playerGold >= _userCustomizeData.AvatarValue[_notHaveAvatarList[index]])
        {
            _purchaseButton.image.color = _enoughGoldColor;
            _purchaseButton.interactable = true;
            _askPurchaseAvatarText.text = $"가격은 {_userCustomizeData.AvatarValue[_notHaveAvatarList[index]]}입니다. 구매하시겠습니까?";
            _purchasePopUpCloseButtonText.text = "조금만 더 둘러볼게요.";
        }
        else
        {
            // 아니라면 구매가 불가능하도록 설정.
            _purchaseButton.image.color = _notEnoughGoldColor;
            _purchaseButton.interactable = false;
            _askPurchaseAvatarText.text = $"돈이 부족하여 구매할 수 없습니다.";
            _purchasePopUpCloseButtonText.text = "돌아가기";
        }


        _purchasePopUp.SetActive(true);
    }

    /// <summary>
    /// 아바타의 순서대로 아바타를 누르면 아바타 구매 정보 팝업에 인덱스를 전달함.
    /// </summary>
    private void FirstAvatarInfo() => AvatarInfo(_startNum);
    private void SecondAvatarInfo() => AvatarInfo(_startNum + 1);
    private void ThirdAvatarInfo() => AvatarInfo(_startNum + 2);

    /// <summary>
    /// 구매 버튼을 눌렀을 때 구매를 적용시키는 메서드.
    /// </summary>
    private void PurchaseButton()
    {
        
        if (_playerNetworking.GetComponent<PhotonView>().IsMine)
        {
            // 아바타를 구매했으므로 플레이어의 소지 골드를 아바타의 가격만큼 줄여서 업데이트.
            MySqlSetting.UseGold(_playerNickname, _userCustomizeData.AvatarValue[_setAvatarNum]);

            // 구매한 아바타는 바로 착용되며, 착용하고 있던 아바타는 가지고 있음 상태로 전환.
            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;

            // 바뀐 데이터를 문자열로 저장하여
            for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
            {
                _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
            }

            // DB에 저장함.
            MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData, _saveString);

            _saveString = null;

            // 플레이어의 커스터마이징 정보를 적용시키는 RPC 함수.
            _playerCustomize = _playerNetworking.GetComponentInChildren<PlayerCustomize>();
            _playerCustomize.photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, _isFemale);

            // 구매정보 팝업을 닫고 구매 완료 팝업을 띄움.
            _purchaseCompletePopUp.SetActive(true);
            _purchasePopUp.SetActive(false);

            // 아바타 구매로 모든 아바타를 소지한다면 메서드 종료.
            if (_notHaveAvatarList.Count == 0)
            {
                return;
            }

            // 아니라면 리스트를 비우고
            _notHaveAvatarList.Clear();

            // 다시 가지고 있지 않은 리스트를 만들어냄.
            for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
            {
                if (_userCustomizeData.AvatarState[i] == EAvatarState.NONE)
                {
                    _notHaveAvatarList.Add(i);
                }
            }
        }

        // 플레이어의 골드를 다시 저장하고
        _playerGold = MySqlSetting.CheckHaveGold(_playerNickname);

        // UI에 적용시킴.
        _currentGold.text = _playerGold.ToString();

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 구매 정보 팝업을 닫음.
    /// </summary>
    private void PopUpClose() => _purchasePopUp.SetActive(false);

    /// <summary>
    /// 구매 완료 팝업을 닫음.
    /// </summary>
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


