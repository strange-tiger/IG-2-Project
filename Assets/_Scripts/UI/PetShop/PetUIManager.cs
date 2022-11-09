#define debug
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;
using _DB = Asset.MySql.MySqlSetting;

public class PetUIManager : UIManager
{
    [SerializeField] Collider _npcCollider;

    [SerializeField] PetData _petData;

    public PlayerNetworking PlayerNetworkingInPet { get; private set; }

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
            SetIsHave();
            SetSize();
            SetAssetIndex();
            SetLevel();
        }

        public GameObject PetObject { get; private set; }
        public string Name { get; private set; }
        public EGrade Grade { get; private set; }
        public string Explanation { get; private set; }
        public int Price { get; private set; }
        public EPetStatus IsHave { get; private set; }
        public float Size { get; private set; }
        public int AssetIndex { get; private set; }
        public int Level { get; private set; }

        public void SetPrefab(GameObject prefab)
        {
            PetObject = Instantiate(prefab);
            PetObject.transform.GetChild(AssetIndex).gameObject.SetActive(true);
            PetObject.SetActive(false);
        }
        public void SetName(string name = "Temp") { Name = name; }
        public void SetGrade(EGrade grade = EGrade.A) { Grade = grade; }
        public void SetExplanation(string explain = "Temp") { Explanation = explain; }
        public void SetPrice(int price = 0) { Price = price; }
        public void SetIsHave(EPetStatus isHave = EPetStatus.NONE) { IsHave = isHave; }
        public void SetSize(float size = 0.3f) { Size = size; }
        public void SetAssetIndex(int assetIndex = 0) { AssetIndex = assetIndex; }
        public void SetLevel(int level = 0) { Level = level; }
    }

    public PetProfile[] PetList { get; private set; }

    private void Awake()
    {
        ShutPetUI();
        _DB.Init();
    }

    private void OnEnable()
    {
#if !debug
        PlayerNetworkingInPet = FindObjectOfType<PlayerNetworking>();
#endif
        InitializePetInventory();
    }

    public PetData GetPetData() => _petData;

    public void LoadUI(_UI ui)
    {
#if !debug
        _npcCollider.enabled = false;
#endif
        LoadUI((int)ui);
    }

    public void ShutPetUI()
    {
        ShutUI();
#if !debug
        _npcCollider.enabled = true;
#endif
    }

    private void InitializePetInventory()
    {
        PetList = new PetProfile[_petData.PetObject.Length];
        
        for (int i = 0; i < PetList.Length; ++i)
        {
            PetList[i] = new PetProfile();

            PetList[i].SetPrefab(_petData.PetObject[i]);

            PetList[i].SetIsHave(_petData.PetStatus[i]);
        }
    }
}
