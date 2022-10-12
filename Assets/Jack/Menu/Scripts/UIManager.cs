using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _socialButton;

    [Header("Panels")]
    [Header("Main Panel")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _socialPanel;

    [Header("ExtraPanels")]
    [SerializeField] private GameObject _checkPanel;
    [SerializeField] private GameObject _addFriendPanel;
    [SerializeField] private GameObject _blockUserPanel;
    private CheckPanelManager _checkPanelScript;

    private GameObject _currentShownPanel;

    private void Awake()
    {
        _checkPanelScript = _checkPanel.GetComponent<CheckPanelManager>();

        _currentShownPanel = _inventoryPanel;

        SettingButtons();
    }

    private void SettingButtons()
    {
        _inventoryButton.onClick.AddListener(() => { ShowPanel(_inventoryPanel); });
        _settingButton.onClick.AddListener(() => { ShowPanel(_settingPanel); });
        _socialButton.onClick.AddListener(() => { ShowPanel(_socialPanel); });
    }
    private void ShowPanel(GameObject panel)
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

    public void ShowMenuPanel()
    {
        _menuPanel.SetActive(true);
    }
}
