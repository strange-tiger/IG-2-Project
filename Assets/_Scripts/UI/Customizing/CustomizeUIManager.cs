using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomizeUIManager : UIManager
{
    // 변환/장착 창과 관련된 버튼
    [Header("CustomizeMenu Button")]
    [SerializeField] private Button _customizeMenuOnButton;
    [SerializeField] private Button _customizeMenuCloseButton;
    [SerializeField] private Button _customizeMenuOffButton;

    // 상점과 관련된 버튼
    [Header("CustomizeShop Button")]
    [SerializeField] private Button _customizeShopOnButton;
    [SerializeField] private Button _customizeShopCloseButton;
    [SerializeField] private Button _customizeShopOffButton;

    // 초기 NPC UI와 관련된 버튼
    [Header("CustomizeNPC Button")]
    [SerializeField] private Button _customizeNPCCloseButton;

    // 각 UI의 Panel
    [Header("Panel")]
    [SerializeField] private CustomizeMenu _customizeMenu;
    [SerializeField] private GameObject _customizeShop;
    [SerializeField] private GameObject _customizeNPCMenu;

    // NPC의 Collider
    [Header("NPC Collider")]
    [SerializeField] private MeshCollider _collider;


    private void OnEnable()
    {
        _customizeMenuOnButton.onClick.RemoveListener(MenuOn);
        _customizeMenuOnButton.onClick.AddListener(MenuOn);

        _customizeMenuCloseButton.onClick.RemoveListener(MenuClose);
        _customizeMenuCloseButton.onClick.AddListener(MenuClose);

        _customizeShopOnButton.onClick.RemoveListener(ShopOn);
        _customizeShopOnButton.onClick.AddListener(ShopOn);

        _customizeShopCloseButton.onClick.RemoveListener(ShopClose);
        _customizeShopCloseButton.onClick.AddListener(ShopClose);

        _customizeNPCCloseButton.onClick.RemoveListener(NPCMenuClose);
        _customizeNPCCloseButton.onClick.AddListener(NPCMenuClose);

        _customizeShopOffButton.onClick.RemoveListener(ShopOff);
        _customizeShopOffButton.onClick.AddListener(ShopOff);

        _customizeMenuOffButton.onClick.RemoveListener(MenuOff);
        _customizeMenuOffButton.onClick.AddListener(MenuOff);
    }

    // 초기 NPC UI를 닫는 버튼
    private void NPCMenuClose()
    {
        _customizeNPCMenu.gameObject.SetActive(false);

        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 변환/장착 UI를 키는 버튼
    private void MenuOn()
    {
        _customizeMenu.gameObject.SetActive(true);
        _customizeNPCMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 변환/장착 UI를 끄는 버튼
    private void MenuOff()
    {
        _customizeMenu.gameObject.SetActive(false);
        _customizeNPCMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 변환/장착 UI에서 모든 UI를 닫는 버튼
    private void MenuClose()
    {
        _customizeMenu.gameObject.SetActive(false);
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(false);

        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 상점 UI를 키는 버튼
    private void ShopOn()
    {
        _customizeShop.SetActive(true);
        _customizeNPCMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 상점 UI를 끄는 버튼
    private void ShopOff()
    {
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 상점 UI에서 모든 UI를 닫는 버튼
    private void ShopClose()
    {
        _customizeMenu.gameObject.SetActive(false);
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(false);
        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }


    private void OnDisable()
    {
        _customizeMenuOnButton.onClick.RemoveListener(MenuOn);

        _customizeMenuCloseButton.onClick.RemoveListener(MenuClose);

        _customizeShopOnButton.onClick.RemoveListener(ShopOn);

        _customizeShopCloseButton.onClick.RemoveListener(ShopClose);

        _customizeNPCCloseButton.onClick.RemoveListener(NPCMenuClose);

        _customizeShopOffButton.onClick.RemoveListener(ShopOff);

        _customizeMenuOffButton.onClick.RemoveListener(ShopOff);
    }
}
