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

    [SerializeField] GameObject _demoOne;
    [SerializeField] GameObject _demoTwo;

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
        }

        public GameObject Prefab { get; private set; }
        public string Name { get; private set; }
        public EGrade Grade { get; private set; }
        public string Explanation { get; private set; }
        public int Price { get; private set; }
        public bool IsHave { get; private set; }

        public void SetPrefab(GameObject prefab) 
        { 
            Prefab = prefab;
            Prefab.SetActive(false);
        }
        public void SetName(string name = "Temp") { Name = name; }
        public void SetGrade(EGrade grade = EGrade.A) { Grade = grade; }
        public void SetExplanation(string explain = "Temp") { Explanation = explain; }
        public void SetPrice(int price = 0) { Price = price; }
        public void SetIsHave(bool isHave = false) { IsHave = isHave; }
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
        Debug.Log(PetList[0]);
    }

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
#if debug
        PetList = new PetProfile[_DB.GetPetInventoryList("Temp").Count];
#else
        PetList = new Pet[_DB.GetPetInventoryList(PlayerNetworkingInPet.MyNickname).Count];
#endif
        
        for (int i = 0; i < PetList.Length; ++i)
        {
            PetList[i] = new PetProfile();

            if (i % 2 == 0)
            {
                PetList[i].SetPrefab(_demoOne);
            }
            else
            {
                PetList[i].SetPrefab(_demoTwo);
            }

#if debug
            PetList[i].SetIsHave(_DB.GetPetInventoryList("Temp")[i]["PetStatus"] == "Have");
#else
            PetList[i].SetIsHave(_DB.GetPetInventoryList(PlayerNetworkingInPet.MyNickname)[i]["PetIndex"] == "Have");
#endif
        }
    }
}
