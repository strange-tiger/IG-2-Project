using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;

public class PetManager : MonoBehaviourPunCallbacks
{
    [Header("PetData")]
    [SerializeField] PetData _petData;
    [SerializeField] GameObject _petObject;
    [SerializeField] EPetMaxExp _petMaxExpType;
    [SerializeField] int _petLevel;
    [SerializeField] int _petExp;
    [SerializeField] int _petMaxExp;
    [SerializeField] float _petSize;

    private YieldInstruction _gainExpTime = new WaitForSeconds(60f);
    private IEnumerator _gainExpCoroutine;
    private int _eqiupNum;
    private int _testNum = -1;
    private bool _havePet = false;
    void Awake()
    {
        _gainExpCoroutine = PetExpIncrease();

        PetDataInitializeFromDB();

        if(_havePet)
        {
            photonView.RPC("PetDataApplied", RpcTarget.All, _eqiupNum);
        }

        if(_petMaxExpType != EPetMaxExp.NONE)
        {
            PetGainExp();
        }
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                if(_testNum == 15)
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


    private void PetDataInitializeFromDB()
    {
        MySqlSetting.Init();
        _petData = MySqlSetting.GetPetInventoryData("Temp",_petData);

        for(int i = 0; i < _petData.PetStatus.Length; ++i)
        {
            if(_petData.PetStatus[i] == EPetStatus.EQUIPED)
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
    public void PetDataApplied(int index)
    {
        if (_petObject != null)
        {
            PhotonNetwork.Destroy(_petObject);
            _petObject.transform.GetChild(_petData.PetAsset[index]).gameObject.SetActive(false);
        }
        if(photonView.IsMine)
        {
            _petObject = PhotonNetwork.Instantiate($"Pets\\{_petData.PetObject[index].name}",transform.position,Quaternion.identity);
            
            _petLevel = _petData.PetLevel[index];

            _petExp = _petData.PetExp[index];

            _petMaxExpType = _petData.PetMaxExp[index];

            switch(_petMaxExpType)
            {
                case EPetMaxExp.ONEHOUR:
                    _petMaxExp = 60;
                    return;

                case EPetMaxExp.THREEHOUR:
                    _petMaxExp = 180;
                    return;

                case EPetMaxExp.SECONDARYEVOL:
                    if(_petLevel == 0)
                    _petMaxExp = 120;
                    else
                    _petMaxExp = 240;
                    return;
            }
        }

        photonView.RPC("ShowPetDataApplied", RpcTarget.All, _petObject, index);
    }

    [PunRPC]
    private void ShowPetDataApplied(GameObject petObject, int index)
    {
        petObject.transform.GetChild(_petData.PetAsset[index]).gameObject.SetActive(true);

        petObject.transform.GetChild(_petData.PetAsset[index]).GetComponent<PetMove>().SetPetManager(this);

        _petSize = _petData.PetSize[index];

        petObject.transform.localScale = new Vector3(_petObject.transform.localScale.x * _petSize, _petObject.transform.localScale.y * _petSize, _petObject.transform.localScale.z * _petSize);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (_havePet)
        {
            photonView.RPC("PetDataApplied", newPlayer, _eqiupNum);
        }
    }

    /// <summary>
    /// Index�� �޾� PetData�� �ٲ���. ���� �̹� ���� ����������, Destroy���ְ�, PetPrefab �ڽ��� SetActive�� False�� �ٲ���.
    /// </summary>
    /// <param name="index"></param>
    public void PetChange(int index)
    {
        PetGainExpStop();

        

        _eqiupNum = index;

        photonView.RPC("PetDataApplied", RpcTarget.All, _eqiupNum);


        PetGainExp();
    }

    private IEnumerator PetExpIncrease()
    {
        while (true)
        {
            yield return _gainExpTime;

            _petExp++;

            _petData.PetExp[_eqiupNum] = _petExp;

            PetDataUpdate("Temp");

            if(_petExp == _petMaxExp)
            {
                PetLevelUp();
            }
            
        }
    }

    private void PetLevelUp()
    {
        PetGainExpStop();

        _petLevel++;

        _petData.PetLevel[_eqiupNum] = _petLevel;

        if (_petMaxExpType == EPetMaxExp.SECONDARYEVOL && _petLevel < 2)
        {
            _petData.PetExp[_eqiupNum] = 0;
            _petMaxExp *= 2;
            PetGainExp();
        }

    }

    private void PetGainExp()
    {
        if(_petExp == _petMaxExp)
        {
            return;
        }


        StartCoroutine(_gainExpCoroutine);
    }

    private void PetGainExpStop()
    {
        StopCoroutine(_gainExpCoroutine);
    }

   private void PetDataUpdate(string nickname)
    {
        MySqlSetting.UpdatePetInventoryData(nickname, _petData);
    }
}
