using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomizeUIManager : UIManager
{
    [Header("CustomizeMenu Button")]
    [SerializeField] Button _customizeMenuOnButton;
    [SerializeField] Button _customizeMenuCloseButton;
    [SerializeField] Button _customizeMenuOffButton;

    [Header("CustomizeShop Button")]
    [SerializeField] Button _customizeShopOnButton;
    [SerializeField] Button _customizeShopCloseButton;
    [SerializeField] Button _customizeShopOffButton;

    [Header("CustomizeNPC Button")]
    [SerializeField] Button _customizeNPCCloseButton;

    [Header("Panel")]
    [SerializeField] CustomizeMenu _customizeMenu;
    [SerializeField] GameObject _customizeShop;
    [SerializeField] GameObject _customizeNPCMenu;

    [Header("NPC Collider")]
    [SerializeField] MeshCollider _collider;


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

        _customizeShopOffButton.onClick.RemoveListener(CustomizeShopOff);
        _customizeShopOffButton.onClick.AddListener(CustomizeShopOff);

        _customizeMenuOffButton.onClick.RemoveListener(CustomizeMenuOff);
        _customizeMenuOffButton.onClick.AddListener(CustomizeMenuOff);
    }

    private void MenuOn()
    {
        _customizeMenu.gameObject.SetActive(true);
        _customizeNPCMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void MenuClose()
    {
        _customizeMenu.gameObject.SetActive(false);
        _customizeNPCMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ShopOn()
    {
        _customizeShop.SetActive(true);
        _customizeNPCMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void ShopClose()
    {
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void NPCMenuClose()
    {
        _customizeNPCMenu.gameObject.SetActive(false);

        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void CustomizeShopOff()
    {
        _customizeMenu.gameObject.SetActive(false);
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(false);
        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void CustomizeMenuOff()
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

        _customizeShopOffButton.onClick.RemoveListener(CustomizeShopOff);

        _customizeMenuOffButton.onClick.RemoveListener(CustomizeShopOff);

    }
}
