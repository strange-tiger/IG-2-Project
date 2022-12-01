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
    [SerializeField] PetData _petData;
    [SerializeField] PetShopList _petShopList;
    [SerializeField] GameObject _npcChat;

    public static PetSpawner PlayerPetSpawner { get; set; }
    public String PlayerNickname { get; private set; }

    public BasicPlayerNetworking PlayerNetworking { get => _playerNetworking; }
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;

    public PetProfile[] PetList { get; private set; }

    private void Awake()
    {
        ShutPetUI();
    }

    public void ShutPetUI()
    {
        ShutUI();
        _npcCollider.enabled = true;
    }

    private void OnEnable()
    {
        InitializePetShop();

        StartCoroutine(SetPlayerNetworking());
    }

    private static readonly WaitForSeconds PLAYERNETWORKING_DELAY = new WaitForSeconds(3f);
    private IEnumerator SetPlayerNetworking()
    {
        yield return PLAYERNETWORKING_DELAY;

        _playerNetworkings = FindObjectsOfType<BasicPlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.photonView.IsMine)
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

    public void LoadInfo()
    {
        _ui[(int)_UI.INFO].SetActive(true);
    }

    private void InitializePetShop()
    {
        PetList = new PetProfile[_petData.Object.Length];

        _petShopList = _CSV.ParseCSV("PetTextScripts", _petShopList);

        for (int i = 0; i < PetList.Length; ++i)
        {
            PetList[i] = new PetProfile();

            PetList[i].SetImage(_petData.Image[i]);

            PetList[i].SetName(_petShopList.Name[i]);

            PetList[i].SetGrade(_petShopList.Grade[i]);

            PetList[i].SetExplanation(_petShopList.Explanation[i]);

            PetList[i].SetPrice(_petShopList.Price[i]);

            PetList[i].SetStatus(_petData.Status[i]);

            PetList[i].SetEvolCount(_petData.MaxExp[i]);

            PetList[i].SetSize(_petData.Size[i]);

            PetList[i].SetAssetIndex(_petData.ChildIndex[i]);

            PetList[i].SetLevel(_petData.Level[i]);

            Debug.Log($"[PET] {PetList[i].Name} {PetList[i].Grade}");
        }
    }

    private const string AIR_NAME = "공기";
    private const string AIR_EXPLAIN = "우리 곁에 있는 소중한 친구.\n들이쉬고 내쉬다 보면\n질릴 정도로 건강해질 겁니다! 예!!!";

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
            SetEvolCount();
            SetSize();
            SetAssetIndex();
            SetLevel();
        }

        public Sprite Image { get; private set; }
        public string Name { get; private set; }
        public EGrade Grade { get; private set; }
        public string Explanation { get; private set; }
        public int Price { get; private set; }
        public EPetStatus Status { get; private set; }
        public EPetMaxExp EvolCount { get; private set; }
        public float Size { get; private set; }
        public int AssetIndex { get; private set; }
        public int Level { get; private set; }

        public void SetImage(Sprite image = null) => Image = image;
        public void SetName(string name = AIR_NAME) => Name = name;
        public void SetGrade(EGrade grade = EGrade.C) => Grade = grade;
        public void SetExplanation(string explain = AIR_EXPLAIN) => Explanation = explain;
        public void SetPrice(int price = 0) => Price = price;
        public void SetStatus(EPetStatus status = EPetStatus.NONE) => Status = status;
        public void SetEvolCount(EPetMaxExp evolCount = EPetMaxExp.NONE) => EvolCount = evolCount;
        public void SetSize(float size = 1f) => Size = size;
        public void SetAssetIndex(int assetIndex = 0) => AssetIndex = assetIndex;
        public void SetLevel(int level = 0) => Level = level;
    }
}
