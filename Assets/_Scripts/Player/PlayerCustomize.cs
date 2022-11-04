using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using System;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCustomize : MonoBehaviourPunCallbacks
{
    public static int IsFemale = 0;

    [SerializeField] UserCustomizeData _femaleData;
    [SerializeField] UserCustomizeData _maleData;
    [SerializeField] UserCustomizeData _userData;
    private int _setAvatarNum;
    private int _setMaterialNum;
    private bool _isLoadData;
    private CustomizeData _materialData;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private PlayerNetworking _playerInfo;
 
    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        _playerInfo = GetComponentInParent<PlayerNetworking>();

        if (!_isLoadData)
        {
            if (bool.Parse(MySqlSetting.GetValueByBase(Asset.EaccountdbColumns.Nickname, name, Asset.EaccountdbColumns.HaveCharacter)))
            {
                LoadAvatarData();
            }
            else
            {
                MakeAvatarData();
            }
        }

        photonView.RPC("AvatarSetting", RpcTarget.All);
    }


    public void MakeAvatarData()
    {

        if(IsFemale == 0)
        {
            _userData = _maleData;
           
        }
        else
        {
            _userData = _femaleData;
            
        }

        _setAvatarNum = 0;
        _setMaterialNum = 0;

        _isLoadData = true;

    }

  
    private void LoadAvatarData()
    {
        
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerInfo.MyNickname, Asset.EcharacterdbColumns.Gender));

        // 성별에 맞는 데이터를 불러옴
        if (_isFemale)
        {
            IsFemale = 1;
            _userData = _femaleData;
        }
        else
        {
            IsFemale = 0;
            _userData = _maleData;
        }

        // DB에 저장되어 있던 아바타 데이터를 불러옴
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerInfo.MyNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 불러온 데이터를 스크립터블 오브젝트에 넣어줌
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB에 저장되어 있던 아바타의 Material을 불러옴
        _userData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerInfo.MyNickname, Asset.EcharacterdbColumns.AvatarColor));

        // 아바타의 정보를 돌면서 장착중이던 아바타를 찾아냄.
        for (int i = 0; i < _userData.AvatarState.Length - 1; ++i)
        {
            if (_userData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                break;
            }
        }

        // 장착중이던 아이템과 Material을 적용시킴.
        _setMaterialNum = _userData.UserMaterial[0];

        _isLoadData = true;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("AvatarSetting", newPlayer, _setAvatarNum, _setMaterialNum, IsFemale);
    }


    [PunRPC]
    public void AvatarSetting(int avatarNum, int materialNum, int genderNum)
    {
        if(genderNum == 1)
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
