using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;

public class PetManager : MonoBehaviour
{
    [Header("PetData")]
    [SerializeField] PetData _petData;
    [SerializeField] GameObject _petObject;
    [SerializeField] int _petLevel;
    [SerializeField] int _petEXP;
    [SerializeField] float _petSize;
    private List<Dictionary<string, string>> _petDataList = new List<Dictionary<string, string>>();
    private int _eqiupNum;

    void Start()
    {
        _petDataList = MySqlSetting.GetPetInventoryList(name);

        for(int i = 0; i < _petDataList.Count; ++i)
        {
            _petData.PetStatus[i] = (EPetStatus)Enum.Parse(typeof(EPetStatus),_petDataList[i]["PetStatus"]);
            _petData.PetLevel[i] = int.Parse(_petDataList[i]["PetLevel"]);
            _petData.PetEXP[i] = int.Parse(_petDataList[i]["PetEXP"]);
            _petData.PetAsset[i] = int.Parse(_petDataList[i]["PetAsset"]);

            if (_petData.PetStatus[i] == EPetStatus.EQUIPED)
            {
                _eqiupNum = i;
            }
        }

        _petObject = Instantiate(_petData.PetObject[_eqiupNum]);

        _petObject.transform.GetChild(_petData.PetAsset[_eqiupNum]).gameObject.SetActive(true);

        _petLevel = _petData.PetLevel[_eqiupNum];

        _petEXP = _petData.PetEXP[_eqiupNum];

    }


    void Update()
    {

    }
}
