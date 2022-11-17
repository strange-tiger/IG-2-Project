#define _DEV_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Defines
{
    public enum EPanelType
    {
        MenuPanel,
        FriendPanel,
    }
}

public class InventoryUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _socialButton;

    [Header("Inventory Panels")]
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _socialPanel;

    /// <summary>
    /// Inventory UI가 켜져있는지 여부
    /// </summary>
    public bool IsInventoryUIOn { get { return _inventoryUI.activeSelf; } }

    private GameObject _currentShownPanel;

    private void Awake()
    {
        _currentShownPanel = _inventoryPanel;

        SettingButtons();
    }

    private void SettingButtons()
    {
        _inventoryButton.onClick.AddListener(() => { ShowMenuPanel(_inventoryPanel); });
        _settingButton.onClick.AddListener(() => { ShowMenuPanel(_settingPanel); });
        _socialButton.onClick.AddListener(() => { ShowMenuPanel(_socialPanel); });
    }
    private void ShowMenuPanel(GameObject panel)
    {
        _currentShownPanel.SetActive(false);
        panel.SetActive(true);
        _currentShownPanel = panel;
    }
    public void ExitPanel(GameObject exitPanel)
    {
        Debug.Log(exitPanel.name + " is now closed");
        exitPanel.SetActive(false);
    }

    /// <summary>
    /// Inventory UI 보여줌
    /// </summary>
    public void ShowInventoryUI()
    {
        _inventoryUI.SetActive(true);
    }

    public void HideInventoryUI()
    {
        _inventoryUI.SetActive(false);
    }
}
