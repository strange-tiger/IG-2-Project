#define debug
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

    private int _eqiupNum;
    private int _testNum = -1;
    private bool _havePet = false;

    void Awake()
    {
        PetDataInitializeFromDB();

        if (_havePet)
        {
            photonView.RPC("PetInstantiate", RpcTarget.All, _eqiupNum);
        }
    }

#if debug
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (_testNum == 15)
                {
                    _testNum = 0;
                }
                else
                {
                    _testNum++;
                }
                PetChange(_testNum);
            }
        }
    }
#endif

    private void PetDataInitializeFromDB()
    {
        MySqlSetting.Init();
        _petData = MySqlSetting.GetPetInventoryData("Temp", _petData);

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

        if (photonView.IsMine)
        {
            _petObject = PhotonNetwork.Instantiate($"Pets\\{_petData.Object[index].name}", transform.position, Quaternion.identity);
            _petObject.transform.GetChild(_petData.ChildIndex[index]).GetComponent<PetMove>().SetTarget(transform);

        }
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
