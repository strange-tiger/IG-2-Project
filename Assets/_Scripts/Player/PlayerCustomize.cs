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
 
    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (!_isLoadData)
        {
            //if (bool.Parse(MySqlSetting.GetValueByBase(Asset.EaccountdbColumns.Nickname, name, Asset.EaccountdbColumns.HaveCharacter)))
            //{
            //    LoadAvatarData();
            //}
            //else
            //{
            //    MakeAvatarData();
            //}
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
        
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.Gender));

        // ������ �´� �����͸� �ҷ���
        if (_isFemale)
        {
            _userData = _femaleData;
        }
        else
        {
            _userData = _maleData;
        }

        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ �����͸� �ҷ���
        string[] avatarData = MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarData).Split(',');

        // �ҷ��� �����͸� ��ũ���ͺ� ������Ʈ�� �־���
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }
        // DB�� ����Ǿ� �ִ� �ƹ�Ÿ�� Material�� �ҷ���
        _userData.UserMaterial[0] = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, "name", Asset.EcharacterdbColumns.AvatarColor));

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
        _setMaterialNum = _userData.UserMaterial[0];

        _isLoadData = true;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("AvatarSetting", newPlayer, _setAvatarNum, _setMaterialNum);
    }


    [PunRPC]
    public void AvatarSetting()
    {
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];
    }

}
