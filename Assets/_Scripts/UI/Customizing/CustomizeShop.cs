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
    private ColorBlock _enoughGoldColorBlock;
    private ColorBlock _notEnoughGoldColorBlock;

    private Queue<int> _haveAvatar = new Queue<int>();
    private PlayerNetworking _playerNetworking;
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

        _enoughGoldColorBlock.normalColor = _enoughGoldColor;
        _notEnoughGoldColorBlock.normalColor = _notEnoughGoldColor;

        if (photonView.IsMine)
        {
            _playerNetworking = FindObjectOfType<PlayerNetworking>();
            _playerNickname = _playerNetworking.MyNickname;
        }
    }

    void Start()
    {

        MySqlSetting.Init();

        // ������ Ȯ����.
        _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // ������ Ȯ���Ͽ� �´� �����͸� �ҷ���
        if (_isFemale)
        {
            _userCustomizeData = _femaleUserCustomizeData;
        }
        else
        {
            _userCustomizeData = _maleUserCustomizeData;
        }

        // �ش� ������ �ƹ�Ÿ �����͸� �ҷ���
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // �ҷ��� �ƹ�Ÿ �����͸� ��ũ���ͺ������Ʈ�� �־���.
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userCustomizeData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // ������ �� �����͸� �ҷ���
        _userCustomizeData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        // �������̾��� �ƹ�Ÿ�� �����͸� �ҷ���.
        for (int i = 0; i < _userCustomizeData.AvatarState.Length - 1; ++i)
        {
            if (_userCustomizeData.AvatarState[i] == EAvatarState.NONE)
            {
                _setAvatarNum = i;
                _haveAvatar.Enqueue(i);
            }

        }

        RootSet();

        // Material�� ������ �ƹ�Ÿ �����͸� Ŀ���͸����� â�� �����Ŵ
        _setMaterialNum = _userCustomizeData.UserMaterial[0];
        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        // �������� �ƹ�Ÿ�� �̸��� ������ �����Ŵ.
        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();
    }

    void PurchaseButton()
    {
        if (_userCustomizeData.AvatarState[_setAvatarNum] == EAvatarState.NONE)
        {
            _userCustomizeData.AvatarState[_setAvatarNum] = EAvatarState.HAVE;
            _skinnedMeshRenderer.material = _customizeDatas.AvatarMaterial[_setMaterialNum];
        }
        else
        {
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);
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

        if (MySqlSetting.CheckHaveGold(_playerNickname) >= _userCustomizeData.AvatarValue[_setAvatarNum])
        {
            _purchaseButton.colors = _enoughGoldColorBlock;
            _purchaseButton.interactable = true;
        }
        else
        {
            _purchaseButton.colors = _notEnoughGoldColorBlock;
            _purchaseButton.interactable = false;

        }

        EventSystem.current.SetSelectedGameObject(null);

    }

    void RightAvatarButton()
    {
        Queueing();

        RootSet();

        _skinnedMeshRenderer.sharedMesh = _userCustomizeData.AvatarMesh[_setAvatarNum];

        _avatarName.text = _userCustomizeData.AvatarName[_setAvatarNum];
        _avatarValue.text = _userCustomizeData.AvatarValue[_setAvatarNum].ToString();

        if(MySqlSetting.CheckHaveGold(_playerNickname) >= _userCustomizeData.AvatarValue[_setAvatarNum])
        {
            _purchaseButton.colors = _enoughGoldColorBlock;
            _purchaseButton.interactable = true;
        }
        else
        {
            _purchaseButton.colors = _notEnoughGoldColorBlock;
            _purchaseButton.interactable = false;

        }

        EventSystem.current.SetSelectedGameObject(null);

    }



    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _purchaseButton.onClick.RemoveListener(PurchaseButton);
    }
}


