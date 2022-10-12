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
    [SerializeField] private GameObject _InventoryUI;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _socialPanel;

    /// <summary>
    /// Inventory UI가 켜져있는지 여부
    /// </summary>
    public bool IsInventoryUIOn { get { return _InventoryUI.activeSelf; } }

    [Header("Friend Panel")]
    [SerializeField] private GameObject _friendPanel;
    private TextMeshProUGUI _targetFriendName;

    private GameObject _currentShownPanel;

    private void Awake()
    {
        _currentShownPanel = _inventoryPanel;
        _targetFriendName = _friendPanel.GetComponentInChildren<TextMeshProUGUI>();

        SettingButtons();
        ShowFriendPanel("맘보");
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
        _InventoryUI.SetActive(true);
    }
    /// <summary>
    /// 친구 추가 패널을 보여줌
    /// </summary>
    /// <param name="targetUserName"> 타겟 유저 이름 </param>
    public void ShowFriendPanel(string targetUserName)
    {
#if _DEV_MODE_
        Debug.Assert(targetUserName != null, "유저명 없음");
#else
        targetUserName = "오류";
#endif
        _targetFriendName.text = targetUserName;
        _friendPanel.SetActive(true);
    }
}
