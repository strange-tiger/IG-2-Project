using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomizeUIManager : UIManager
{
    [SerializeField] Button _customizeMenuOnButton;
    [SerializeField] Button _customizeMenuOffButton;
    [SerializeField] Button _customizeShopOnButton;
    [SerializeField] Button _customizeShopOffButton;
    [SerializeField] Button _customizeShopOffCanvasButton;
    [SerializeField] Button _customizeNPCOffButton;
    [SerializeField] GameObject _customizeMenu;
    [SerializeField] GameObject _customizeShop;
    [SerializeField] GameObject _customizeNPCMenu;
    [SerializeField] MeshCollider _collider;


    private void OnEnable()
    {
        _customizeMenuOnButton.onClick.RemoveListener(MenuOn);
        _customizeMenuOnButton.onClick.AddListener(MenuOn);

        _customizeMenuOffButton.onClick.RemoveListener(MenuOff);
        _customizeMenuOffButton.onClick.AddListener(MenuOff);

        _customizeShopOnButton.onClick.RemoveListener(ShopOn);
        _customizeShopOnButton.onClick.AddListener(ShopOn);

        _customizeShopOffButton.onClick.RemoveListener(ShopOff);
        _customizeShopOffButton.onClick.AddListener(ShopOff);

        _customizeNPCOffButton.onClick.RemoveListener(NPCMenuOff);
        _customizeNPCOffButton.onClick.AddListener(NPCMenuOff);

        _customizeShopOffCanvasButton.onClick.RemoveListener(CustomizeCanvasOff);
        _customizeShopOffCanvasButton.onClick.AddListener(CustomizeCanvasOff);

    }

    private void MenuOn()
    {
        _customizeMenu.SetActive(true);
        _customizeNPCMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void MenuOff()
    {
        _customizeMenu.SetActive(false);
        _customizeNPCMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ShopOn()
    {
        _customizeShop.SetActive(true);
        _customizeNPCMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void ShopOff()
    {
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }
    private void NPCMenuOff()
    {
        _customizeNPCMenu.SetActive(false);

        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void CustomizeCanvasOff()
    {
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(true);
        _collider.enabled = true;

        EventSystem.current.SetSelectedGameObject(null);

    }

    private void OnDisable()
    {
        _customizeMenuOnButton.onClick.RemoveListener(MenuOn);

        _customizeMenuOffButton.onClick.RemoveListener(MenuOff);

        _customizeShopOnButton.onClick.RemoveListener(ShopOn);

        _customizeShopOffButton.onClick.RemoveListener(ShopOff);

        _customizeNPCOffButton.onClick.RemoveListener(NPCMenuOff);

        _customizeShopOffCanvasButton.onClick.RemoveListener(CustomizeCanvasOff);

    }
}
