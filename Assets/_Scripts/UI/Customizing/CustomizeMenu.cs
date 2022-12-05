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
    // 커스터마이징 장착 / 변환 UI에서 사용되는 버튼들.
    [Header("Button")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _leftMaterialButton;
    [SerializeField] private Button _rightMaterialButton;
    [SerializeField] private Button _leftAvatarButton;
    [SerializeField] private Button _rightAvatarButton;

    // 변환할 아바타의 메테리얼 데이터 스크립터블 오브젝트와 이미지.
    [Header("Change Avatar")]
    [SerializeField] private AvatarMaterialData _avatarMaterialData;
    [SerializeField] private Image _avatarImage;

    // 변환할 아바타의 이름 등의 정보.
    [Header("Change Avatar Info")]
    [SerializeField] private TextMeshProUGUI _avatarName;
    [SerializeField] private TextMeshProUGUI _avatarNickname;
    [SerializeField] private TextMeshProUGUI _avatarMaterialNum;
    [SerializeField] private TextMeshProUGUI _avatarInfoText;
    [SerializeField] private TextMeshProUGUI _messageText;

    // 현재 내 아바타의 메테리얼 데이터와 이미지.
    [Header("Current Avatar")]
    [SerializeField] private AvatarMaterialData _currentAvatarMaterialData;
    [SerializeField] private Image _currentAvatarImage;

    // 현재 나의 성별에 따라 커스터마이즈 데이터 스크립터블 오브젝트.
    [Header("Avatar Data")]
    [SerializeField] private UserCustomizeData _maleUserCustomizeData;
    [SerializeField] private UserCustomizeData _femaleUserCustomizeData;
    [SerializeField] private UserCustomizeData _userCustomizeData;

    public bool IsCustomizeChanged;

    // 가지고 있는 아바타의 인덱스 리스트.
    private List<int> _haveAvatarList = new List<int>();

    // 플레이어의 커스터마이징을 적용할 스크립트.
    private PlayerCustomize _playerCustomize;

    // UI와 상호작용할 플레이어를 찾을 때 필요한 PlayerNetworking과 닉네임.
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;
    private string _playerNickname;

    private YieldInstruction _fadeTextTime = new WaitForSeconds(0.5f);

    // 저장할 정보를 DB에 저장할 문자열.
    private string _saveString;

    private string _saveCompleteText = "저장이 완료되었습니다.";
    private string _changeExistText = "아바타가 변경되었습니다. 저장 버튼을 누르면 반영됩니다.";

    // 적용할 아바타와 메테리얼 인덱스.
    private int _setAvatarNum;
    private int _setMaterialNum;

    // 현재 착용중인 아바타와 메테리얼 인덱스.
    private int _equipNum;
    private int _equipMaterialNum;

    // 처음 변환창에 설정되어 있는 아바타의 인덱스.
    private int _startNum;

    // 플레이어의 성별.
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

        // PlayerNetworking 중, PhotonView.IsMine인 것을 찾아
        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        // 닉네임을 받아옴.
        _playerNickname = _playerNetworking.MyNickname;

        IsCustomizeChanged = false;

        // 현재 아바타의 정보를 DB에서 불러와 커스터마이징 UI를 초기화함.
        AvatarMenuInit();
    }

    /// <summary>
    /// 커스터마이징 UI를 초기화 하는 메서드.
    /// </summary>
    private void AvatarMenuInit()
    {
        // 성별을 확인함.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // 성별에 맞는 데이터를 불러옴.
        if(_isFemale)
        {
            _userCustomizeData = _femaleUserCustomizeData;
        }
        else
        {
            _userCustomizeData = _maleUserCustomizeData;
        }

        // DB에 저장되어 있던 아바타 데이터를 불러옴.
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');
        
        // 불러온 데이터를 스크립터블 오브젝트에 넣어줌.
        for(int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }

        // DB에 저장되어 있던 아바타의 Material을 불러옴.
        _setMaterialNum = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        // 장착중인 메테리얼의 인덱스를 저장함.
        _equipMaterialNum = _setMaterialNum;

        // 아바타의 정보를 돌면서 장착중이던 아바타를 찾아냄.
        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
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

        // 가지고 있는 아바타의 리스트에서 장착중인 아바타의 인덱스를 가져옴.
        _startNum = _haveAvatarList.IndexOf(_equipNum);

        // 변환창에 띄울 아바타는 현재 장착중인 아바타.
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
        _avatarInfoText.text = _userCustomizeData.AvatarInfo[_setAvatarNum];

    }


    /// <summary>
    /// 변경된 아바타의 정보를 플레이어 모델에 적용 시키고, DB에 저장함.
    /// </summary>
    void SaveButton()
    {
        // 기존에 착용중인 아바타는 Have상태로 바꾸고, 선택되어 있는 아바타의 상태를 EQUIPED 상태로 바꿈.
        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.HAVE)
        {
            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;
        }

        // 현재 아바타 창에 적용된 아바타의 메테리얼 데이터와 이미지를 적용시킴.
        _currentAvatarMaterialData = _avatarMaterialData;
        _currentAvatarImage.sprite = _currentAvatarMaterialData.AvatarImage[_setMaterialNum];

        // 커스터마이즈 데이터의 상태를 불러와 SaveString에 저장.
        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
            _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
        }

        // 메테리얼의 인덱스를 받아 DB에 저장.
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor, _setMaterialNum);

        // 커스터마이즈 데이터의 상태를 SaveString을 이용하여 DB에 저장.
        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData, _saveString);

        // SaveString 초기화.
        _saveString = null;

        // 변경된 아바타의 정보를 내 캐릭터에 적용시키는 RPC 메서드를 호출함.
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

        // 저장이 완료되었음을 알리는 안내 텍스트.
        _messageText.text = _saveCompleteText;

        // 저장완료 텍스트를 지워줄 코루틴.
        StartCoroutine(TextFade());

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 저장이 완료되었음을 알리는 텍스트를 시간이 지나면 지워주는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TextFade()
    {
        yield return _fadeTextTime;

        _messageText.text = null;

    }

    /// <summary>
    /// 왼쪽 버튼을 눌렀을 때, 아바타 리스트의 인덱스를 이용하여 아바타를 변경함.
    /// </summary>
    private void LeftAvartarButton()
    {
        // 리스트의 시작점이라면 리스트의 끝 인덱스로 넘어감.
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
        if (_setMaterialNum == _equipMaterialNum && _equipNum == _setAvatarNum)
        {
            _messageText.text = null;
        }
        else
        {
            _messageText.text = _changeExistText;
        }

        // 메테리얼의 인덱스를 초기화 하고, 바뀐 리스트의 인덱스를 이용하여 아바타 정보를 불러옴.
        _setMaterialNum = 0;
        _avatarMaterialData = _userCustomizeData.AvatarMaterial[_setAvatarNum];
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarNickname.text = _userCustomizeData.AvatarNickname[_setAvatarNum];
        _avatarInfoText.text = _userCustomizeData.AvatarInfo[_setAvatarNum];

        _avatarMaterialNum.text = $"컬러 {_setMaterialNum + 1}";

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 오른쪽 버튼을 눌렀을 때, 아바타 리스트의 인덱스를 이용하여 아바타를 변경함.
    /// </summary>
    void RightAvatarButton()
    {
        // 리스트의 끝이라면 리스트의 시작점으로 인덱스가 넘어감.
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
        if (_setMaterialNum == _equipMaterialNum && _equipNum == _setAvatarNum)
        {
            _messageText.text = null;
        }
        else
        {
            _messageText.text = _changeExistText;
        }

        // 메테리얼의 인덱스를 초기화 하고, 바뀐 리스트의 인덱스를 이용하여 아바타 정보를 불러옴.
        _setMaterialNum = 0;
        _avatarMaterialData = _userCustomizeData.AvatarMaterial[_setAvatarNum];
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarNickname.text = _userCustomizeData.AvatarNickname[_setAvatarNum];
        _avatarInfoText.text = _userCustomizeData.AvatarInfo[_setAvatarNum];

        _avatarMaterialNum.text = $"컬러 {_setMaterialNum + 1}";

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 왼쪽 컬러 버튼을 눌렀을 때, 메테리얼의 인덱스를 변화시킴.
    /// </summary>
    void LeftMaterialButton()
    {
        // 리스트의 시작이라면 리스트의 끝으로 인덱스가 넘어감.
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _avatarMaterialData.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

        // 현재의 메테리얼 인덱스와 초기 착용중이던 메테리얼 인덱스, 그리고 초기 아바타와 현재 아바타 인덱스가 같지 않으면 변경사항이 존재한다는 텍스트를 띄움.
        if (_setMaterialNum == _equipMaterialNum && _equipNum == _setAvatarNum)
        {
            _messageText.text = null;
        }
        else
        {
            _messageText.text = _changeExistText;
        }

        // 현재 컬러의 정보를 Ui에 적용.
        _avatarMaterialNum.text = $"컬러 {_setMaterialNum + 1}";

        // 아바타 이미지를 메테리얼 인덱스에 맞춰 변경시킴.
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 오른쪽 컬러 버튼을 눌렀을 때, 메테리얼의 인덱스를 변화시킴.
    /// </summary>
    void RightMaterialButton()
    {
        //리스트의 끝이라면 리스트의 시작점으로 인덱스가 넘어감.
        if (_setMaterialNum == _avatarMaterialData.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        // 현재의 메테리얼 인덱스와 초기 착용중이던 메테리얼 인덱스, 그리고 초기 아바타와 현재 아바타 인덱스가 같지 않으면 변경사항이 존재한다는 텍스트를 띄움.
        if (_setMaterialNum == _equipMaterialNum && _equipNum == _setAvatarNum)
        {
            _messageText.text = null;
        }
        else
        {
            _messageText.text = _changeExistText;
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

        // 가지고 있던 아바타의 리스트를 비워준다.
        _haveAvatarList.Clear();

        
        _playerNetworking = null;
        _playerCustomize = null;
    }
}
