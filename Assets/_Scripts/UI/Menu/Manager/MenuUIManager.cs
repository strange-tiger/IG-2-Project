using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : GlobalInstance<MenuUIManager>
{
    [SerializeField] private InventoryUIManager _inventoryUIManager;
    [SerializeField] private SocialUIManager _socialUIManager;
    [SerializeField] private CheckPanelManager _checkPanelManager;
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;
    [SerializeField] private GameObject[] _moveStopUIs;

    public bool IsUIOn
    {
        get
        {
            foreach (GameObject ui in _moveStopUIs)
            {
                if (ui.activeSelf)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public void ShowInventory()
    {
        _inventoryUIManager.ShowInventoryUI();
    }
    public void HideInventory()
    {
        _inventoryUIManager.HideInventoryUI();
    }
    public void ShowSocialUI(UserInteraction targetUserNickname)
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
