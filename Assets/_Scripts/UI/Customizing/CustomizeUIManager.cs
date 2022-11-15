using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUIManager : UIManager
{
    [SerializeField] Button _customizeMenuOnButton;
    [SerializeField] Button _customizeMenuOffButton;
    [SerializeField] Button _customizeShopOnButton;
    [SerializeField] Button _customizeShopOffButton;
    [SerializeField] Button _customizeNPCOffButton;
    [SerializeField] GameObject _customizeMenu;
    [SerializeField] GameObject _customizeShop;
    [SerializeField] GameObject _customizeNPCMenu;

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
    }

    private void MenuOn()
    {
        _customizeMenu.SetActive(true);
        _customizeNPCMenu.SetActive(false);
    }

    private void MenuOff()
    {
        _customizeMenu.SetActive(false);
        _customizeNPCMenu.SetActive(true);
    }

    private void ShopOn()
    {
        _customizeShop.SetActive(true);
        _customizeNPCMenu.SetActive(false);
    }
    private void ShopOff()
    {
        _customizeShop.SetActive(false);
        _customizeNPCMenu.SetActive(true);
    }
    private void NPCMenuOff() => _customizeNPCMenu.SetActive(false);

    private void OnDisable()
    {
        _customizeMenuOnButton.onClick.RemoveListener(MenuOn);

        _customizeMenuOffButton.onClick.RemoveListener(MenuOff);

        _customizeShopOnButton.onClick.RemoveListener(ShopOn);

        _customizeShopOffButton.onClick.RemoveListener(ShopOff);

        _customizeNPCOffButton.onClick.RemoveListener(NPCMenuOff);

    }
}
