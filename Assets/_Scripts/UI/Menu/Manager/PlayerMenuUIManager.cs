#define _DEV_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Asset.MySql;

namespace Defines
{
    public enum EPanelType
    {
        MenuPanel,
        FriendPanel,
    }
}

public class PlayerMenuUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _mapButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _socialButton;

    [Header("Inventory Panels")]
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _socialPanel;

    [Header("Gold")]
    [SerializeField] private TextMeshProUGUI _goldText;
    private string _myNickname;

    /// <summary>
    /// Inventory UI가 켜져있는지 여부
    /// </summary>
    public bool IsInventoryUIOn { get { return _menuUI.activeSelf; } }

    private GameObject _currentShownPanel;

    private void Awake()
    {
        _currentShownPanel = _mapPanel;

        SettingButtons();

        _myNickname = PhotonNetwork.NickName;
    }

    private void SettingButtons()
    {
        _mapButton.onClick.AddListener(() => { ShowMenuPanel(_mapPanel); });
        _settingButton.onClick.AddListener(() => { ShowMenuPanel(_settingPanel); });
        _socialButton.onClick.AddListener(() => { ShowMenuPanel(_socialPanel); });
    }
    private void ShowMenuPanel(GameObject panel)
    {
        _currentShownPanel.SetActive(false);
        panel.SetActive(true);
        _currentShownPanel = panel;

        UISimplePopup uiSimplePopup = GetComponent<UISimplePopup>();
        uiSimplePopup?.SetOn(true);
    }
    public void ExitPanel(GameObject exitPanel)
    {
        Debug.Log(exitPanel.name + " is now closed");
        exitPanel.SetActive(false);

        UISimplePopup uiSimplePopup = GetComponent<UISimplePopup>();
        uiSimplePopup?.SetOn(false);
    }

    /// <summary>
    /// Inventory UI 보여줌
    /// </summary>
    public void ShowMenuUI()
    {
        _goldText.text =
            MySqlSetting.CheckHaveGold(_myNickname).ToString();
        _menuUI.SetActive(true);

        UISimplePopup uiSimplePopup = GetComponent<UISimplePopup>();
        uiSimplePopup?.SetOn(true);
    }
    /// <summary>
    /// Invantory UI 안보여줌
    /// </summary>
    public void HideMenuUI()
    {
        _menuUI.SetActive(false);
    }
}
