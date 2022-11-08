using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;

public class PetManager : MonoBehaviour
{
    [Header("PetData")]
    [SerializeField] Pet _pet;
    [SerializeField] PetData _petData;
    [SerializeField] GameObject _petObject;
    [SerializeField] int _petLevel;
    [SerializeField] int _petEXP;
    [SerializeField] float _petSize;
    private int _eqiupNum;

    void Awake()
    { 
        PetDataInitializeFromDB();
        PetDataApplied();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            PetDataUpdate();
        }
    }
    private void PetDataInitializeFromDB()
    {
        MySqlSetting.Init();
        _petData = MySqlSetting.GetPetInventoryData("Temp",_petData);
    }

    private void PetDataApplied()
    {
        _petObject = Instantiate(_petData.PetObject[_eqiupNum]);

        _petObject.transform.GetChild(_petData.PetAsset[_eqiupNum]).gameObject.SetActive(true);

        _pet = _petObject.GetComponent<Pet>();

        _petLevel = _petData.PetLevel[_eqiupNum];

        _petEXP = _petData.PetExp[_eqiupNum];
    }

    public void PetChange(int index)
    {
        _eqiupNum = index;
        PetDataApplied();
    }

   private void PetDataUpdate()
    {
        MySqlSetting.UpdatePetInventoryData("Temp", _petData);
    }
}
