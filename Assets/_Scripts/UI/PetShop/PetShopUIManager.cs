using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using TMPro;

using _UI = Defines.EPetShopUIIndex;
using _DB = Asset.MySql.MySqlSetting;
using _CSV = Asset.ParseCSV.CSVParser;

public class PetShopUIManager : UIManager
{
    [SerializeField] Collider _npcCollider;

    public static PetSpawner PlayerPetSpawner { get; set; }
    public String PlayerNickname { get; private set; }

    private void Awake()
    {
        ShutPetUI();
        _DB.Init();
    }

    public void ShutPetUI()
    {
        ShutUI();
        _npcCollider.enabled = true;
    }

    private void OnEnable()
    {
        
        
        PlayerNickname = PhotonNetwork.NickName;
    }

    public void LoadUI(_UI ui)
    {
        _npcCollider.enabled = false;
        LoadUI((int)ui);
    }
}
