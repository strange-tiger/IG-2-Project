using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using TMPro;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;
using _CSV = Asset.ParseCSV.CSVParser;

public class PetShopUIManager : UIManager
{
    [SerializeField] Collider _npcCollider;
}
