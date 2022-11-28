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
    [SerializeField] Button _equipButton;
    [SerializeField] Button _leftMaterialButton;
    [SerializeField] Button _rightMaterialButton;
    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _rightAvatarButton;

    [Header("Change Avatar")]
    [SerializeField] TextMeshProUGUI _avatarName;
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] GameObject _smMeshRendererObject;
    [SerializeField] GameObject _characterMeshRendererObject;

    [Header("Current Avatar")]
    [SerializeField] TextMeshProUGUI _currentAvatarName;
    [SerializeField] SkinnedMeshRenderer _currentSkinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _currentSmMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _currentCharacterMeshRenderer;
    [SerializeField] GameObject _currentSmMeshRendererObject;
    [SerializeField] GameObject _currentCharacterMeshRendererObject;

    [Header("Avatar Data")]
    [SerializeField] TextMeshProUGUI _materialNum;
    [SerializeField] TextMeshProUGUI _avatarInfoText;
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] AvatarMaterialData _avatarMaterialData;
    [SerializeField] UserCustomizeData _maleUserCustomizeData;
    [SerializeField] UserCustomizeData _femaleUserCustomizeData;
    [SerializeField] UserCustomizeData _userCustomizeData;

    public bool IsCustomizeChanged;

    private PlayerCustomize _playerCustomize;
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;
    private List<int> _haveAvatarList = new List<int>();
    private YieldInstruction _fadeTextTime = new WaitForSeconds(0.5f);

    private int _setAvatarNum;
    private int _equipNum;
    private int _startNum;
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
        _userCustomizeData.UserMaterial = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));
        
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

        InitRootSet();


        // 장착중이던 아이템과 Material을 적용시킴.
        _currentAvatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _setMaterialNum = _userCustomizeData.UserMaterial;
        _currentSkinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];
        _currentSkinnedMeshRenderer.material = _avatarMaterialData.AvatarMaterial[_setMaterialNum];


        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _setMaterialNum = 0;
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_haveAvatarList[0]];
        _skinnedMeshRenderer.material = _avatarMaterialData.AvatarMaterial[_setMaterialNum];


    }



    void EquipButton()
    {
        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.HAVE)
        {
            _userCustomizeData.AvatarState[_equipNum] = EAvatarState.HAVE;
            _equipNum = _setAvatarNum;
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.EQUIPED;
            _userCustomizeData.UserMaterial = _setMaterialNum;
        }

        RootSet();

        for (int i = 0; i < _userCustomizeData.AvatarState.Length; ++i)
        {
            _saveString += _userCustomizeData.AvatarState[i].ToString() + ',';
        }

        MySqlSetting.UpdateValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor, _userCustomizeData.UserMaterial);
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

    private void InitRootSet()
    {
        if (_setAvatarNum <= 9 && _setAvatarNum >= 7)
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
        RootSet(); 
    }


    void LeftAvartarButton()
    {

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
        if (_equipNum != _haveAvatarList[_startNum])
        {
            _messageText.text = "아바타가 변경되었습니다. 저장 버튼을 누르면 반영됩니다.";
        }
        else
        {
            _messageText.text = null;
        }

        RootSet();

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        EventSystem.current.SetSelectedGameObject(null);


    }

    void RightAvatarButton()
    {
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

        if (_equipNum != _haveAvatarList[_startNum])
        {
            _messageText.text = "아바타가 변경되었습니다. 저장 버튼을 누르면 반영됩니다.";
        }
        else
        {
            _messageText.text = null;
        }

        RootSet();

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        EventSystem.current.SetSelectedGameObject(null);


    }

    void LeftMaterialButton()
    {
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _avatarMaterialData.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

        _materialNum.text = $"컬러 {_setMaterialNum + 1}";

        _skinnedMeshRenderer.material = _avatarMaterialData.AvatarMaterial[_setMaterialNum];


        EventSystem.current.SetSelectedGameObject(null);

    }

    void RightMaterialButton()
    {
        if (_setMaterialNum == _avatarMaterialData.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        _materialNum.text = $"컬러 {_setMaterialNum + 1}";

        _skinnedMeshRenderer.material = _avatarMaterialData.AvatarMaterial[_setMaterialNum];


        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _equipButton.onClick.RemoveListener(EquipButton);

        _haveAvatarList.Clear();

        _playerNetworking = null;
        _playerCustomize = null;
    }
}
