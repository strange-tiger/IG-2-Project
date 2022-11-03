using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using System;
using Photon.Pun;

public class PlayerCustomize : MonoBehaviourPun,IPunObservable
{
    public static int IsFemale = 0;

    [SerializeField] UserCustomizeData _femaleData;
    [SerializeField] UserCustomizeData _maleData;
    [SerializeField] UserCustomizeData _userData;
    private int _setAvatarNum;
    private int _setMaterialNum;
    private CustomizeData _materialData;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(IsFemale);
            stream.SendNext(_setAvatarNum);
            stream.SendNext(_setMaterialNum);
        }
        else if(stream.IsReading)
        {
            if ((int)stream.ReceiveNext() == 0)
            {
                _userData = _maleData;
            }
            else
            {
                _userData = _femaleData;
            }
            _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[(int)stream.ReceiveNext()];
            _skinnedMeshRenderer.material = _materialData.AvatarMaterial[(int)stream.ReceiveNext()];

        }
    }
    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        _setAvatarNum = 0;
        _setMaterialNum = 0;


        AvatarInit();

        //if(bool.Parse(MySqlSetting.GetValueByBase(Asset.EaccountdbColumns.Nickname,name,Asset.EaccountdbColumns.HaveCharacter)))
        //{
        //    AvatarSetting();
        //}
        //else
        //{

        //    AvatarInit();
        //}
    }


    public void AvatarInit()
    {

        if(IsFemale == 0)
        {
            _userData = _maleData;
           
        }
        else
        {
            _userData = _femaleData;
            
        }
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];


    }

    private void AvatarSetting()
    {
        
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.Gender));

        // 성별에 맞는 데이터를 불러옴
        if (_isFemale)
        {
            _userData = _femaleData;
        }
        else
        {
            _userData = _maleData;
        }

        // DB에 저장되어 있던 아바타 데이터를 불러옴
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarData).Split(',');

        // 불러온 데이터를 스크립터블 오브젝트에 넣어줌
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB에 저장되어 있던 아바타의 Material을 불러옴
        _userData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor));

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
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];

    }



}
