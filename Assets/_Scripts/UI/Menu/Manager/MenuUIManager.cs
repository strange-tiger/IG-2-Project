using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : GlobalInstance<MenuUIManager>
{
    [SerializeField] private InventoryUIManager _inventoryUIManager;
    [SerializeField] private SocialUIManager _socialUIManager;
    [SerializeField] private CheckPanelManager _checkPanelManager;
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;

    public bool IsUIOn 
    { 
        get 
        {
            return _inventoryUIManager.gameObject.activeSelf ||
                _socialUIManager.gameObject.activeSelf ||
                _checkPanelManager.gameObject.activeSelf ||
                _confirmPanelManager.gameObject.activeSelf;
        } 
    }

    public void ShowInventory()
    {
        _inventoryUIManager.ShowInventoryUI();
    }
    public void ShowSocialUI(string targetUserNickname)
    {
        _socialUIManager.ShowFriendPanel(targetUserNickname);
    }
    public void ShowCheckPanel(string message,
        CheckPanelManager.CheckButtonAction acceptButtonAction,
        CheckPanelManager.CheckButtonAction denyButtonAction)
    {
        _checkPanelManager.ShowCheckPanel(message, acceptButtonAction, denyButtonAction);
    }
    public void ShowConfirmPanel(string message)
    {
        _confirmPanelManager.ShowConfirmPanel(message);
    }
} 
