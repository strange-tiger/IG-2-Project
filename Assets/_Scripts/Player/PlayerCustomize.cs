using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Asset.MySql;
using System;
using Photon.Pun;
using Photon.Realtime;

using SceneType = Defines.ESceneNumder;

public class PlayerCustomize : MonoBehaviourPunCallbacks
{
    public bool IsFemale { get; set; }

    [SerializeField] UserCustomizeData _femaleData;
    [SerializeField] UserCustomizeData _maleData;
    [SerializeField] UserCustomizeData _userData;
    [SerializeField] CustomizeData _materialData;
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] GameObject _smMeshRendererObject;
    [SerializeField] GameObject _characterMeshRendererObject;
    private int _setAvatarNum;
    private int _setMaterialNum;
    private string _playerNickname;

    void Start()
    {



        if (SceneManager.GetActiveScene().name != "MakeCharacterRoom")
        {
            if (photonView.IsMine)
            {
                if (SceneManager.GetActiveScene().name != "Login")
                {
                    _playerNickname = PhotonNetwork.NickName;

                }
                LoadAvatarData();
            }
        }



    }


    public void MakeAvatarData()
    {

        if (IsFemale == false)
        {
            _userData = _maleData;

        }
        else
        {
            _userData = _femaleData;

        }

        _setAvatarNum = 0;
        _setMaterialNum = 0;

        RootSet(_setAvatarNum);

        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];


    }


    private void LoadAvatarData()
    {

        Debug.Log(_playerNickname);
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // ������ �´� �����͸� �ҷ���
        if (_isFemale)
        {
            IsFemale = _isFemale;
            _userData = _femaleData;
        }
        else
        {
            IsFemale = _isFemale;
            _userData = _maleData;
        }

        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ �����͸� �ҷ���
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // �ҷ��� �����͸� ��ũ���ͺ� ������Ʈ�� �־���
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ�� Material�� �ҷ���
        _userData.UserMaterial = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        // �ƹ�Ÿ�� ������ ���鼭 �������̴� �ƹ�Ÿ�� ã�Ƴ�.
        for (int i = 0; i < _userData.AvatarState.Length - 1; ++i)
        {
            if (_userData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                break;
            }
        }

        // �������̴� �����۰� Material�� �����Ŵ.
        _setMaterialNum = _userData.UserMaterial;
        if (SceneManager.GetActiveScene().name != "StartRoom")
        {
            photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, IsFemale);
        }
        else
        {
            RootSet(_setAvatarNum);
            _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
            _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];
        }



    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("AvatarSetting", newPlayer, _setAvatarNum, _setMaterialNum, IsFemale);
        }
    }

    private void RootSet(int avatarNum)
    {
        if (avatarNum <= 9 && avatarNum >= 7)
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

    [PunRPC]
    public void AvatarSetting(int avatarNum, int materialNum, bool genderNum)
    {
        RootSet(avatarNum);

        if (genderNum == true)
        {
            _userData = _femaleData;
        }
        else
        {
            _userData = _maleData;
        }

        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[avatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[materialNum];

    }

}
