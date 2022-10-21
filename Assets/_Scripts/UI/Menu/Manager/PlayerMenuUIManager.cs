using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuUIManager : GlobalInstance<PlayerMenuUIManager>
{
    [Header("Managers")]
    [SerializeField] InventoryUIManager _inventoryUIManager;
    [SerializeField] SocialUIManager _socialManager;
    [SerializeField] CheckPanelManager _checkPanelManager;
    [SerializeField] ConfirmPanelManager _confirmPanelManager;

    public bool IsThereUI
    {
        get
        {
            return _inventoryUIManager.gameObject.activeSelf ||
                _socialManager.gameObject.activeSelf ||
                _checkPanelManager.gameObject.activeSelf ||
                _confirmPanelManager.gameObject.activeSelf;
        }
    }

    public void ShowMenu()
    {
        _inventoryUIManager.ShowInventoryUI();
    }
    public void ShowSocial(string _targetUserName)
    {
        _socialManager.ShowFriendPanel(_targetUserName);
    }
    public void ShowCheckPanel(string checkMessage,
        CheckPanelManager.CheckButtonAction acceptButtonAction,
        CheckPanelManager.CheckButtonAction rejectButtonAction)
    {
        _checkPanelManager.ShowCheckPanel(checkMessage, acceptButtonAction, rejectButtonAction);
    }
    public void ShowConfirmPanel(string message)
    {
        _confirmPanelManager.ShowConfirmPanel(message);
    }
}
