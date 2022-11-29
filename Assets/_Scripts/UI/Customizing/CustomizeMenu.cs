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

    [Header("Button")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _leftMaterialButton;
    [SerializeField] private Button _rightMaterialButton;
    [SerializeField] private Button _leftAvatarButton;
    [SerializeField] private Button _rightAvatarButton;

    [Header("Change Avatar")]
    [SerializeField] private AvatarMaterialData _avatarMaterialData;
    [SerializeField] private Image _avatarImage;


    [Header("Change Avatar Info")]
    [SerializeField] private TextMeshProUGUI _avatarName;
    [SerializeField] private TextMeshProUGUI _avatarNickname;
    [SerializeField] private TextMeshProUGUI _avatarMaterialNum;
    [SerializeField] private TextMeshProUGUI _avatarInfoText;
    [SerializeField] private TextMeshProUGUI _messageText;

    [Header("Current Avatar")]
    [SerializeField] private AvatarMaterialData _currentAvatarMaterialData;
    [SerializeField] private Image _currentAvatarImage;

    [Header("Avatar Data")]
    [SerializeField] private UserCustomizeData _maleUserCustomizeData;
    [SerializeField] private UserCustomizeData _femaleUserCustomizeData;
    [SerializeField] private UserCustomizeData _userCustomizeData;

    public bool IsCustomizeChanged;

    private List<int> _haveAvatarList = new List<int>();

    private PlayerCustomize _playerCustomize;
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;

    private YieldInstruction _fadeTextTime = new WaitForSeconds(0.5f);

    private string _saveString;
    private string _playerNickname;

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

        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _leftMaterialButton.onClick.AddListener(LeftMaterialButton);

        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _rightMaterialButton.onClick.AddListener(RightMaterialButton);

        _saveButton.onClick.RemoveListener(SaveButton);
        _saveButton.onClick.AddListener(SaveButton);

        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        _playerNickname = _playerNetworking.MyNickname;

        IsCustomizeChanged = false;

        AvatarMenuInit();
    }

    private void AvatarMenuInit()
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
        _setMaterialNum = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));
        
        // 아바타의 정보를 돌면서 장착중이던 아바타를 찾아냄.
        for(int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
            if(_userCustomizeData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                _equipNum = i;
                _haveAvatarList.Add(i);
            }
            else if(_userCustomizeData.AvatarState[i] == EAvatarState.HAVE)
            {
                _haveAvatarList.Add(i);
            }
        }

        _startNum = _haveAvatarList.IndexOf(_equipNum);

        _setAvatarNum = _haveAvatarList[_startNum];

        // 현재 아바타 정보에 장착중이던 아이템과 Material을 적용시킴.
        _currentAvatarMaterialData = _userCustomizeData.AvatarMaterial[_setAvatarNum];
        _currentAvatarImage.sprite = _currentAvatarMaterialData.AvatarImage[_setMaterialNum];

        // 기본 커스터마이징 창에도 현재 장착 아이템을 적용시킴.
        _avatarMaterialData = _currentAvatarMaterialData;
        _avatarImage.sprite = _currentAvatarImage.sprite;

        // 커스터마이징 창의 아바타 이름, 닉네임을 적용시킴.
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarNickname.text = _userCustomizeData.AvatarNickname[_setAvatarNum];
    }



    void SaveButton()
    {
        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.HAVE)
        {
            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;
        }


        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
            _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
        }

        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor, _setMaterialNum);
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData, _saveString);

        _saveString = null;

        if(_playerNetworking.GetComponent<PhotonView>().IsMine)
        {
            _playerCustomize = _playerNetworking.GetComponentInChildren<PlayerCustomize>();
            _playerCustomize.photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, _isFemale);
        }

        if(_messageText.text != null)
        {
            IsCustomizeChanged = true;
        }
        else
        {
            IsCustomizeChanged = false;
        }

        _messageText.text = "저장이 완료되었습니다.";

        StartCoroutine(TextFade());

        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator TextFade()
    {
        yield return _fadeTextTime;

        _messageText.text = null;

    }




    void LeftAvartarButton()
    {
        // 왼쪽 버튼을 눌렀을 때, 아바타 리스트의 인덱스를 이용하여 아바타를 변경함.
        if(_startNum == 0)
        {
            _startNum = _haveAvatarList.Count - 1;
            _setAvatarNum = _haveAvatarList[_startNum];
        }
        else
        {
            _startNum--;
            _setAvatarNum = _haveAvatarList[_startNum];
        }

        // 처음 아바타와 변경 사항이 있을 때, 텍스트를 띄움.
        if (_equipNum != _haveAvatarList[_startNum])
        {
            _messageText.text = "아바타가 변경되었습니다. 저장 버튼을 누르면 반영됩니다.";
        }
        else
        {
            _messageText.text = null;
        }

        // 메테리얼의 인덱스를 초기화 하고, 바뀐 리스트의 인덱스를 이용하여 아바타 정보를 불러옴.
        _setMaterialNum = 0;
        _avatarMaterialData = _userCustomizeData.AvatarMaterial[_setAvatarNum];
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarNickname.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarInfoText.text = _userCustomizeData.AvatarInfo[_setAvatarNum];

        EventSystem.current.SetSelectedGameObject(null);
    }

    void RightAvatarButton()
    {
        // 오른쪽 버튼을 눌렀을 때, 아바타 리스트의 인덱스를 이용하여 아바타를 변경함.
        if (_startNum == _haveAvatarList.Count - 1)
        {
            _startNum = 0;
            _setAvatarNum = _haveAvatarList[_startNum];
        }
        else
        {
            _startNum++;
            _setAvatarNum = _haveAvatarList[_startNum];
        }

        // 처음 아바타와 변경 사항이 있을 때, 텍스트를 띄움.
        if (_equipNum != _haveAvatarList[_startNum])
        {
            _messageText.text = "아바타가 변경되었습니다. 저장 버튼을 누르면 반영됩니다.";
        }
        else
        {
            _messageText.text = null;
        }

        // 메테리얼의 인덱스를 초기화 하고, 바뀐 리스트의 인덱스를 이용하여 아바타 정보를 불러옴.
        _setMaterialNum = 0;
        _avatarMaterialData = _userCustomizeData.AvatarMaterial[_setAvatarNum];
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarNickname.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarInfoText.text = _userCustomizeData.AvatarInfo[_setAvatarNum];

        EventSystem.current.SetSelectedGameObject(null);
    }

    void LeftMaterialButton()
    {
        // 왼쪽 컬러 버튼을 눌렀을 때, 메테리얼의 인덱스를 변화시킴.
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _avatarMaterialData.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

        // 현재 컬러의 정보를 Ui에 적용.
        _avatarMaterialNum.text = $"컬러 {_setMaterialNum + 1}";

        // 아바타 이미지를 메테리얼 인덱스에 맞춰 변경시킴.
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];

        EventSystem.current.SetSelectedGameObject(null);
    }

    void RightMaterialButton()
    {
        // 오른쪽 컬러 버튼을 눌렀을 때, 메테리얼의 인덱스를 변화시킴.
        if (_setMaterialNum == _avatarMaterialData.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        // 현재 컬러의 정보를 UI에 적용.
        _avatarMaterialNum.text = $"컬러 {_setMaterialNum + 1}";

        // 아바타 이미지를 메테리얼 인덱스에 맞춰 변경시킴.
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _saveButton.onClick.RemoveListener(SaveButton);

        _haveAvatarList.Clear();

        _playerNetworking = null;
        _playerCustomize = null;
    }
}
