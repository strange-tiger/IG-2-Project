using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;
using _CSV = Asset.ParseCSV.CSVParser;
using Photon.Pun;

public class PetUIManager : UIManager
{
    [SerializeField] Collider _npcCollider;
    [SerializeField] PetData _petData;

    [SerializeField] PetShopInteract _npc;
    public PetShopInteract Npc { get => _npc; }

    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;
    public string PlayerNickname { get; private set; }
    public static PetSpawner PlayerPetSpawner { get; set; }

    public class PetProfile
    {
        public enum EGrade
        {
            C,
            B,
            A,
            S,
            MAX
        }

        public PetProfile()
        {
            SetName();
            SetGrade();
            SetExplanation();
            SetPrice();
            SetStatus();
            SetSize();
            SetAssetIndex();
            SetLevel();
        }

        public GameObject PetObject { get; private set; }
        public string Name { get; private set; }
        public EGrade Grade { get; private set; }
        public string Explanation { get; private set; }
        public int Price { get; private set; }
        public EPetStatus Status { get; private set; }
        public float Size { get; private set; }
        public int AssetIndex { get; private set; }
        public int Level { get; private set; }

        public void SetPrefab(GameObject prefab)
        {
            PetObject = Instantiate(prefab);
            PetObject.transform.GetChild(AssetIndex).gameObject.SetActive(true);
            PetObject.SetActive(false);
        }
        public void SetName(string name = "Temp") => Name = name;
        public void SetGrade(EGrade grade = EGrade.A) => Grade = grade;
        public void SetExplanation(string explain = "Temp") => Explanation = explain;
        public void SetPrice(int price = 0) => Price = price;
        public void SetStatus(EPetStatus status = EPetStatus.NONE) => Status = status;
        public void SetSize(float size = 1f) => Size = size;
        public void SetAssetIndex(int assetIndex = 0) => AssetIndex = assetIndex;
        public void SetLevel(int level = 0) => Level = level;
    }

    public PetProfile[] PetList { get; private set; }

    private void Awake()
    {
        ShutPetUI();
        _DB.Init();
    }

    private void OnEnable()
    {
        InitializePetInventory();

        StartCoroutine(SetPlayerNetworking());
    }

    private IEnumerator SetPlayerNetworking()
    {
        yield return new WaitForSeconds(1f);

        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }
        PlayerNickname = _playerNetworking.MyNickname;
    }

    public PetData GetPetData() => _petData;

    public void LoadUI(_UI ui)
    {
        _npcCollider.enabled = false;
        LoadUI((int)ui);
    }

    public void ShutPetUI()
    {
        ShutUI();
        _npcCollider.enabled = true;
    }

    private void InitializePetInventory()
    {
        PetList = new PetProfile[_petData.Object.Length];

        List<Dictionary<string, string>> csvData = _CSV.ParseCSV("PetTextScripts");

        for (int i = 0; i < PetList.Length; ++i)
        {
            PetList[i] = new PetProfile();

            PetList[i].SetName(csvData[i]["�̸�"]);

            PetList[i].SetGrade((PetProfile.EGrade) Enum.Parse(typeof(PetProfile.EGrade), csvData[i]["���"]));

            PetList[i].SetExplanation(csvData[i]["����"]);

            PetList[i].SetPrice(int.Parse(csvData[i]["����(���)"]));

            PetList[i].SetPrefab(_petData.Object[i]);

            PetList[i].SetStatus(_petData.Status[i]);

            PetList[i].SetSize(_petData.Size[i]);

            PetList[i].SetAssetIndex(_petData.ChildIndex[i]);

            PetList[i].SetLevel(_petData.Level[i]);
        }
    }
}