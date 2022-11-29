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

        // ������ Ȯ����.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // ������ �´� �����͸� �ҷ���
        if(_isFemale)
        {
            _userCustomizeData = _femaleUserCustomizeData;
        }
        else
        {
            _userCustomizeData = _maleUserCustomizeData;
        }

        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ �����͸� �ҷ���
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');
        
        // �ҷ��� �����͸� ��ũ���ͺ� ������Ʈ�� �־���
        for(int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }

        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ�� Material�� �ҷ���
        _setMaterialNum = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));
        
        // �ƹ�Ÿ�� ������ ���鼭 �������̴� �ƹ�Ÿ�� ã�Ƴ�.
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

        // ���� �ƹ�Ÿ ������ �������̴� �����۰� Material�� �����Ŵ.
        _currentAvatarMaterialData = _userCustomizeData.AvatarMaterial[_setAvatarNum];
        _currentAvatarImage.sprite = _currentAvatarMaterialData.AvatarImage[_setMaterialNum];

        // �⺻ Ŀ���͸���¡ â���� ���� ���� �������� �����Ŵ.
        _avatarMaterialData = _currentAvatarMaterialData;
        _avatarImage.sprite = _currentAvatarImage.sprite;

        // Ŀ���͸���¡ â�� �ƹ�Ÿ �̸�, �г����� �����Ŵ.
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

        _messageText.text = "������ �Ϸ�Ǿ����ϴ�.";

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
        // ���� ��ư�� ������ ��, �ƹ�Ÿ ����Ʈ�� �ε����� �̿��Ͽ� �ƹ�Ÿ�� ������.
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

        // ó�� �ƹ�Ÿ�� ���� ������ ���� ��, �ؽ�Ʈ�� ���.
        if (_equipNum != _haveAvatarList[_startNum])
        {
            _messageText.text = "�ƹ�Ÿ�� ����Ǿ����ϴ�. ���� ��ư�� ������ �ݿ��˴ϴ�.";
        }
        else
        {
            _messageText.text = null;
        }

        // ���׸����� �ε����� �ʱ�ȭ �ϰ�, �ٲ� ����Ʈ�� �ε����� �̿��Ͽ� �ƹ�Ÿ ������ �ҷ���.
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
        // ������ ��ư�� ������ ��, �ƹ�Ÿ ����Ʈ�� �ε����� �̿��Ͽ� �ƹ�Ÿ�� ������.
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

        // ó�� �ƹ�Ÿ�� ���� ������ ���� ��, �ؽ�Ʈ�� ���.
        if (_equipNum != _haveAvatarList[_startNum])
        {
            _messageText.text = "�ƹ�Ÿ�� ����Ǿ����ϴ�. ���� ��ư�� ������ �ݿ��˴ϴ�.";
        }
        else
        {
            _messageText.text = null;
        }

        // ���׸����� �ε����� �ʱ�ȭ �ϰ�, �ٲ� ����Ʈ�� �ε����� �̿��Ͽ� �ƹ�Ÿ ������ �ҷ���.
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
        // ���� �÷� ��ư�� ������ ��, ���׸����� �ε����� ��ȭ��Ŵ.
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _avatarMaterialData.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

        // ���� �÷��� ������ Ui�� ����.
        _avatarMaterialNum.text = $"�÷� {_setMaterialNum + 1}";

        // �ƹ�Ÿ �̹����� ���׸��� �ε����� ���� �����Ŵ.
        _avatarImage.sprite = _avatarMaterialData.AvatarImage[_setMaterialNum];

        EventSystem.current.SetSelectedGameObject(null);
    }

    void RightMaterialButton()
    {
        // ������ �÷� ��ư�� ������ ��, ���׸����� �ε����� ��ȭ��Ŵ.
        if (_setMaterialNum == _avatarMaterialData.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        // ���� �÷��� ������ UI�� ����.
        _avatarMaterialNum.text = $"�÷� {_setMaterialNum + 1}";

        // �ƹ�Ÿ �̹����� ���׸��� �ε����� ���� �����Ŵ.
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
