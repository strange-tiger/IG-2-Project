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
    private int _setAvatarNum;
    private int _setMaterialNum;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private PlayerNetworking _playerInfo;
    private string _playerNickname;
 
    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();


        if(SceneManager.GetActiveScene().name != "MakeCharacterRoom")
        {
            if (SceneManager.GetActiveScene().name == "StartRoom")
            {
                _playerNickname = TempAccountDB.Nickname;

            }
            else
            {
                _playerInfo = GetComponentInParent<PlayerNetworking>();
                _playerNickname = _playerInfo.MyNickname;
            }

            LoadAvatarData();
            
        }
        
        

    }


    public void MakeAvatarData()
    {

        if(IsFemale == false)
        {
            _userData = _maleData;
           
        }
        else
        {
            _userData = _femaleData;
            
        }

        _setAvatarNum = 0;
        _setMaterialNum = 0;
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];


    }


    private void LoadAvatarData()
    {
        
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        // 성별에 맞는 데이터를 불러옴
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

        // DB에 저장되어 있던 아바타 데이터를 불러옴
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 불러온 데이터를 스크립터블 오브젝트에 넣어줌
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB에 저장되어 있던 아바타의 Material을 불러옴
        _userData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));

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
        if(SceneManager.GetActiveScene().name != "StartRoom")
        {
            photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, IsFemale);
        }
        else
        {
            _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
            _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];
        }



    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("AvatarSetting", newPlayer, _setAvatarNum, _setMaterialNum, IsFemale);
    }


    [PunRPC]
    public void AvatarSetting(int avatarNum, int materialNum, bool genderNum)
    {
        
            if(genderNum == true)
            {
                _userData = _femaleData;
            }
            else
            {
                _userData = _maleData;
            }
            _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[avatarNum];
            _skinnedMeshRenderer.material = _materialData.AvatarMaterial[materialNum];
        

        Debug.Log(_userData);

    }

}
