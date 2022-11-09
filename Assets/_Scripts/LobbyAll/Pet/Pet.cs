using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using Photon.Pun;
using Photon.Realtime;

public class Pet : MonoBehaviourPunCallbacks
{
    [SerializeField] PetData _petData;
    [SerializeField] EPetMaxExp _petMaxExpType;
    [SerializeField] int _petLevel;
    [SerializeField] int _petExp;
    [SerializeField] int _petMaxExp;
    [SerializeField] float _petSize;

    private static readonly YieldInstruction _gainExpTime = new WaitForSeconds(60f);
    private IEnumerator _gainExpCoroutine;
    private int _eqiupNum;

    void Awake()
    {
        _gainExpCoroutine = PetExpIncrease();

        PetDataInitializeFromPetData();

        photonView.RPC("PetDataApplied", RpcTarget.All, _eqiupNum);

        if (_petMaxExpType != EPetMaxExp.NONE)
        {
            PetGainExp();
        }
    }

    private void PetDataInitializeFromPetData()
    {
        for (int i = 0; i < _petData.Status.Length; ++i)
        {
            if (_petData.Object[i] == gameObject)
            {
                _eqiupNum = i;
                break;
            }
        }
    }

    [PunRPC]
    public void PetDataApplied(int index)
    {
        transform.GetChild(_petData.ChildIndex[index]).gameObject.SetActive(true);

        _petSize = _petData.Size[index];

        transform.GetChild(_petData.ChildIndex[index]).gameObject.transform.localScale = new Vector3(transform.localScale.x * _petSize, transform.localScale.y * _petSize, transform.localScale.z * _petSize);

        _petLevel = _petData.Level[index];

        _petExp = _petData.Exp[index];

        _petMaxExpType = _petData.MaxExp[index];

        switch (_petMaxExpType)
        {
            case EPetMaxExp.ONEHOUR:
                _petMaxExp = 60;
                return;

            case EPetMaxExp.THREEHOUR:
                _petMaxExp = 180;
                return;

            case EPetMaxExp.SECONDARYEVOL:
                if (_petLevel == 0)
                    _petMaxExp = 120;
                else
                    _petMaxExp = 240;
                return;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("PetDataApplied", newPlayer, _eqiupNum);
    }

    private IEnumerator PetExpIncrease()
    {
        while (true)
        {
            yield return _gainExpTime;

            _petExp++;

            _petData.Exp[_eqiupNum] = _petExp;

            PetDataUpdate("Temp");

            if (_petExp == _petMaxExp)
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
        if (_petExp == _petMaxExp)
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
