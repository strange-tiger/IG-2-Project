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
    [Header("Avatar Data")]
    [SerializeField] UserCustomizeData _userData;
    [SerializeField] UserCustomizeData _maleData;
    [SerializeField] UserCustomizeData _femaleData;

    [Header("Material Data")]
    [SerializeField] AvatarMaterialData _materialData;

    [Header("Avatar")]
    [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] GameObject _smMeshRendererObject;
    [SerializeField] GameObject _characterMeshRendererObject;

    public bool IsFemale { get; set; }

    private int _setAvatarNum;
    private int _setMaterialNum;
    private string _playerNickname;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MakeCharacterRoom")
        {
            if (photonView.IsMine)
            {
                _playerNickname = PhotonNetwork.NickName;

                LoadAvatarData();
            }
        }
    }


    public void MakeAvatarData()
    {
        // 캐릭터 생성 씬에서 성별 선택에 따라 커스터마이즈 데이터를 적용시킴.
        if (IsFemale == false)
        {
            _userData = _maleData;
        }
        else
        {
            _userData = _femaleData;
        }

        // 아바타와 메테리얼 인덱스를 초기화
        _setAvatarNum = 0;
        _setMaterialNum = 0;

        // 아바타에 맞는 루트셋팅.
        RootSet(_setAvatarNum);

        // 아바타와 메테리얼을 적용시킴.
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];
    }


    private void LoadAvatarData()
    {
        // 성별을 DB에서 불러옴.
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // 성별에 따라 커스터마이즈 데이터 적용.
        if (_isFemale)
        {
            _userData = _femaleData;
        }
        else
        {
            _userData = _maleData;
        }

        // DB에서 아바타 데이터를 불러옴.
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }

        _setMaterialNum = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

        for (int i = 0; i < _userData.AvatarState.Length - 1; ++i)
        {
            if (_userData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                break;
            }
        }

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("AvatarSetting", newPlayer, _setAvatarNum, _setMaterialNum, IsFemale);
        }
    }

}
