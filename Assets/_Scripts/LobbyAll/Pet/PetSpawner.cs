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

        if (photonView.IsMine)
        {
            if (_havePet)
            {
                photonView.RPC(nameof(PetInstantiate), RpcTarget.All, _eqiupNum);
            }

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
        if (!photonView.IsMine)
        {
            return;
        }

            if (_petObject != null)
        {
            PhotonNetwork.Destroy(_petObject);
        }

        if (index < 0 || index >= _petData.Object.Length)
        {
            return;
        }

        _eqiupNum = index;

        _petObject = PhotonNetwork.Instantiate($"Pets\\{_petData.Object[index].name}", transform.position, Quaternion.identity);
        _petObject.transform.GetChild(_petData.ChildIndex[index]).GetComponent<PetMove>().SetTarget(photonView.ViewID, transform.root);
    }

    public void PetChange(int index)
    {
        _eqiupNum = index;
        photonView.RPC(nameof(PetInstantiate), RpcTarget.All, _eqiupNum);
    }

    private void PetDataUpdate(string nickname)
    {
        MySqlSetting.UpdatePetInventoryData(nickname, _petData);
    }
}
