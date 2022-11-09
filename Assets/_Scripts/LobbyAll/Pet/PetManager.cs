using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using Photon.Pun;
using Photon.Realtime;

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
    private bool _havePet;

    void Awake()
    {
        _gainExpCoroutine = PetExpIncrease();

        PetDataInitializeFromDB();

        if(_havePet)
        {
            photonView.RPC("PetDataApplied", RpcTarget.All);

            if(_petMaxExpType != EPetMaxExp.NONE)
            {
                PetGainExp();
            }
        }

    }


    private void PetDataInitializeFromDB()
    {
        MySqlSetting.Init();
        _petData = MySqlSetting.GetPetInventoryData("Temp",_petData);

        for(int i = 0; i < _petData.Status.Length; ++i)
        {
            if(_petData.Status[i] == EPetStatus.EQUIPED)
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
    public void PetDataApplied()
    {
        _petObject = Instantiate(_petData.Object[_eqiupNum],gameObject.transform);

        _petObject.transform.GetChild(_petData.ChildIndex[_eqiupNum]).gameObject.SetActive(true);

        _petObject.transform.GetChild(_petData.ChildIndex[_eqiupNum]).GetComponent<PetMove>().SetTarget(transform);

        _petLevel = _petData.Level[_eqiupNum];

        _petExp = _petData.Exp[_eqiupNum];

        _petMaxExpType = _petData.MaxExp[_eqiupNum];

        _petSize = _petData.Size[_eqiupNum];

        _petObject.transform.localScale = new Vector3(_petObject.transform.localScale.x * _petSize, _petObject.transform.localScale.y * _petSize, _petObject.transform.localScale.z * _petSize);

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("PetDataApplied", newPlayer);
    }

    /// <summary>
    /// Index를 받아 PetData를 바꿔줌. 만약 이미 펫이 존재했으면, Destroy해주고, PetPrefab 자식의 SetActive를 False로 바꿔줌.
    /// </summary>
    /// <param name="index"></param>
    public void PetChange(int index)
    {
        PetGainExpStop();

        if (_petObject != null)
        {
            Destroy(_petObject);
            _petObject.transform.GetChild(_petData.ChildIndex[_eqiupNum]).gameObject.SetActive(false);
        }

        _eqiupNum = index;

        photonView.RPC("PetDataApplied", RpcTarget.All);


        PetGainExp();
    }

    private IEnumerator PetExpIncrease()
    {
        while (true)
        {
            yield return _gainExpTime;

            _petExp++;

            _petData.Exp[_eqiupNum] = _petExp;

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

        _petData.Level[_eqiupNum] = _petLevel;

        if (_petMaxExpType == EPetMaxExp.SECONDARYEVOL && _petLevel < 2)
        {
            _petData.Exp[_eqiupNum] = 0;
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
