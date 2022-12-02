using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Asset.MySql;
using System;
using Photon.Pun;
using Photon.Realtime;

using SceneType = Defines.ESceneNumber;

/* DB에서 정보를 받아 플레이어의 커스터마이징을 모델에 적용시켜주는 스크립트
 * 캐릭터 커스터마이징은 정해진 모델에 Mesh와 Material만 변경시켜주는 방식으로 사용함.
 * 정해진 아바타의 Mesh가 모델의 Root에 모두 맞지 않아서 두가지의 Root를 가지고 아바타에 알맞은 Object를 사용함.
 */ 
public class PlayerCustomize : MonoBehaviourPunCallbacks
{
    // 플레이어의 성별에 따라 커스터마이징 정보를 DB에서 받아와 저장할 스크립터블 오브젝트
    [Header("Avatar Data")]
    [SerializeField] private UserCustomizeData _userData;
    [SerializeField] private UserCustomizeData _maleData;
    [SerializeField] private UserCustomizeData _femaleData;

    // 플레이어의 메테리얼 정보를 담은 메테리얼 데이터
    [Header("Material Data")]
    [SerializeField] private AvatarMaterialData _materialData;

    // 플레이어의 메쉬에 따라 달라지는 MeshRenderer와 Root를 가지고 있는 GameObject
    [Header("Avatar")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _smMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _characterMeshRenderer;
    [SerializeField] private GameObject _smMeshRendererObject;
    [SerializeField] private GameObject _characterMeshRendererObject;

    // 플레이어의 성별
    public bool IsFemale { get; set; }

    // SmRoot를 사용하는 메쉬의 인덳스
    private int[] _smRootMeshIndex = { 0, 7, 8, 9 };

    // 착용할 아바타의 인덱스와 메테리얼 인덱스
    private int _setAvatarNum;
    private int _setMaterialNum;

    // 플레이어의 닉네임
    private string _playerNickname;

    void Start()
    {
        // 캐릭터 생성씬을 제외하고, 로그인 시 PhotonNetwork에 저장한 닉네임을 받아와 적용한다.
        if (SceneManager.GetActiveScene().name != "MakeCharacterRoom")
        {
            if (photonView.IsMine)
            {
                _playerNickname = PhotonNetwork.NickName;

                // 닉네임을 사용하여 DB에서 아바타 정보를 불러와 모델에 적용시킨다.
                LoadAvatarData();
            }
        }
    }

    // 캐릭터 생성씬에서 캐릭터를 만들때 아바타 데이터를 적용시키는 메서드
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
        _materialData = _userData.AvatarMaterial[_setAvatarNum];
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];
    }

    // 이미 캐릭터를 생성 했다면, DB에 저장된 아바타 데이터를 불러와 모델에 적용시킴.
    private void LoadAvatarData()
    {
        // 성별을 DB에서 불러옴.
        bool _isFemale = bool.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.Gender));

        IsFemale = _isFemale;

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

        // 아바타의 착용, 소지여부를 DB에 불러와 커스터마이즈 데이터에 적용
        for (int i = 0; i < avatarData.Length - 1; ++i)
        {
            _userData.AvatarState[i] = (EAvatarState)Enum.Parse(typeof(EAvatarState), avatarData[i]);
        }

        // 불러온 커스터마이즈 데이터를 돌면서 착용한 아바타의 인덱스를 저장함
        for (int i = 0; i < _userData.AvatarState.Length - 1; ++i)
        {
            if (_userData.AvatarState[i] == EAvatarState.EQUIPED)
            {
                _setAvatarNum = i;
                break;
            }
        }

        // 메테리얼 인덱스를 불러와 적용
        _setMaterialNum = int.Parse(MySqlSetting.GetValueByBase(Asset.EcharacterdbColumns.Nickname, _playerNickname, Asset.EcharacterdbColumns.AvatarColor));


        // 시작의 방을 제외한 씬에서는 자신의 정보들을 이용하여 다른 사람들이 내 커스터마이징을 볼 수 있도록 RPC 함수를 호출하여 동기화
        if (SceneManager.GetActiveScene().name != "StartRoom")
        {
            photonView.RPC("AvatarSetting", RpcTarget.All, _setAvatarNum, _setMaterialNum, IsFemale);
        }
        else
        {
            // 시작의 방에서는 내가 볼 수 있도록만 아바타정보를 적용함.
            RootSet(_setAvatarNum);
            _materialData = _userData.AvatarMaterial[_setAvatarNum];
            _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[_setAvatarNum];
            _skinnedMeshRenderer.material = _materialData.AvatarMaterial[_setMaterialNum];
        }
    }

    // 아바타 세팅을 적용하여 다른 유저가 볼 수 있도록 동기화 시켜주는 RPC 함수
    [PunRPC]
    public void AvatarSetting(int avatarNum, int materialNum, bool genderNum)
    {
        // 아바타의 인덱스를 받아 모델의 Root를 세팅함.
        RootSet(avatarNum);

        // 성별에 따라 커스터마이즈 데이터를 적용 시킴.
        if (genderNum == true)
        {
            _userData = _femaleData;
        }
        else
        {
            _userData = _maleData;
        }

        // 아바타 정보와 메테리얼 정보를 초기화 시킴.
        _skinnedMeshRenderer.sharedMesh = _userData.AvatarMesh[avatarNum];
        _materialData = _userData.AvatarMaterial[avatarNum];
        _skinnedMeshRenderer.material = _materialData.AvatarMaterial[materialNum];
    }

    // 아바타의 Root를 바꿔줌
    private void RootSet(int avatarNum)
    {
        for (int i = 0; i < _smRootMeshIndex.Length; ++i)
        {
            // 정해진 인덱스에 맞는 MeshRenderer, Root를 세팅하여 적용 시킴;
            if (avatarNum == _smRootMeshIndex[i])
            {
                _smMeshRendererObject.SetActive(true);
                _characterMeshRendererObject.SetActive(false);
                _skinnedMeshRenderer = _smMeshRenderer;
                break;
            }
            else
            {
                _smMeshRendererObject.SetActive(false);
                _characterMeshRendererObject.SetActive(true);
                _skinnedMeshRenderer = _characterMeshRenderer;
            }

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("AvatarSetting", newPlayer, _setAvatarNum, _setMaterialNum, IsFemale);
        }
    }

}
