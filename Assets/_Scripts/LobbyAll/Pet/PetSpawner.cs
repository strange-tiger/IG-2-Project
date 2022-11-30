using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Asset.MySql;

public class PetSpawner : MonoBehaviourPunCallbacks
{
    [Header("PetData")]
    [SerializeField] PetData _petData;
    [SerializeField] GameObject _petObject;

    private static readonly WaitForSeconds DELAY_GET_NICKNAME = new WaitForSeconds(1f);
    private PlayerNetworking _player;

    public int EquipedNum { get => _eqiupNum; }
    private int _eqiupNum;
    private bool _havePet = false;

    void Awake()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(PetInitialize());
        }
    }

    private IEnumerator PetInitialize()
    {
        _player = transform.root.GetComponent<PlayerNetworking>();

        yield return DELAY_GET_NICKNAME;

        PetDataInitializeFromDB();

        if (_havePet)
        {
            photonView.RPC("PetInstantiate", RpcTarget.All, _eqiupNum);
        }

        if (photonView.IsMine)
        {
            PetShopUIManager.PlayerPetSpawner = this;
        }
    }

    private void PetDataInitializeFromDB()
    {
        MySqlSetting.Init();

        _petData = MySqlSetting.GetPetInventoryData(_player.MyNickname, _petData);

        for (int i = 0; i < _petData.Status.Length; ++i)
        {
            if (_petData.Status[i] == EPetStatus.EQUIPED)
            {
                _eqiupNum = i;
                _havePet = true;
                break;
            }
            else
            {
                _eqiupNum = -1;
                _havePet = false;
            }
        }
    }

    [PunRPC]
    public void PetInstantiate(int index)
    {
        if (_petObject != null)
        {
            PhotonNetwork.Destroy(_petObject);
        }

        _petObject = PhotonNetwork.Instantiate($"Pets\\{_petData.Object[index].name}", transform.position, Quaternion.identity);
        _petObject.transform.GetChild(_petData.ChildIndex[index]).GetComponent<PetMove>().SetTarget(transform);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (_petObject.activeSelf)
        {
            photonView.RPC("PetInstantiate", newPlayer, _eqiupNum);
        }
    }

    public void PetChange(int index)
    {
        _eqiupNum = index;
        photonView.RPC("PetInstantiate", RpcTarget.All, _eqiupNum);
    }

    private void PetDataUpdate(string nickname)
    {
        MySqlSetting.UpdatePetInventoryData(nickname, _petData);
    }
}
