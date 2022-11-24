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

public class PetUIManager : UIManager
{
    [SerializeField] Collider _npcCollider;
    [SerializeField] PetData _petData;
    [SerializeField] PetShopList _petShopList;
    [SerializeField] GameObject _npcChat;


    [SerializeField] PetShopInteract _npc;
    public PetShopInteract Npc { get => _npc; }

    public string PlayerNickname { get; private set; }
    public static PetSpawner PlayerPetSpawner { get; set; }

    private const int MAX_PET_CHILD_COUNT = 20;
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;

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

        private NavMeshAgent[] _tempAgent = new NavMeshAgent[MAX_PET_CHILD_COUNT];
        public void SetPrefab(GameObject prefab)
        {
            PetObject = Instantiate(prefab);
            PetObject.transform.GetChild(AssetIndex).gameObject.SetActive(true);
            _tempAgent = PetObject.transform.GetComponentsInChildren<NavMeshAgent>();
            
            foreach (NavMeshAgent agent in _tempAgent)
            {
                if (agent != null)
                {
                    agent.enabled = false;
                }
                else
                {
                    break;
                }
            }

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
        yield return new WaitForSeconds(3f);

        _playerNetworkings = FindObjectsOfType<BasicPlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;

                break;
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

    private void Update()
    {
        if(OVRInput.Get(OVRInput.RawButton.A) && _npcChat.activeSelf)
        {
            LoadUI(_UI.POPUP);
        }
    }

    private void InitializePetInventory()
    {
        PetList = new PetProfile[_petData.Object.Length];

        _petShopList = _CSV.ParseCSV("PetTextScripts", _petShopList);
        for (int i = 0; i < _petData.Object.Length; ++i)
        {
            PetList[i] = new PetProfile();

            PetList[i].SetName(_petShopList.Name[i]);

            PetList[i].SetGrade(_petShopList.Grade[i]);

            PetList[i].SetExplanation(_petShopList.Explanation[i]);

            PetList[i].SetPrice(_petShopList.Price[i]);

            PetList[i].SetPrefab(_petData.Object[i]);

            PetList[i].SetStatus(_petData.Status[i]);

            PetList[i].SetSize(_petData.Size[i]);

            PetList[i].SetAssetIndex(_petData.ChildIndex[i]);

            PetList[i].SetLevel(_petData.Level[i]);
        }
    }
}