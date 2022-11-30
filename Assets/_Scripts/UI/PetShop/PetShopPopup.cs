using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetShopUIIndex;

public class PetShopPopup : PopupUI
{
    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _equipButton;


    [Header("UIManager")]
    [SerializeField] PetShopUIManager _ui;

    protected override void OnEnable()
    {
        base.OnEnable();

        _purchaseButton.onClick.RemoveListener(LoadPurchase);
        _purchaseButton.onClick.AddListener(LoadPurchase);

        _equipButton.onClick.RemoveListener(LoadEquip);
        _equipButton.onClick.AddListener(LoadEquip);
    }

    private void LoadPurchase() => _ui.LoadUI(_UI.BUY);
    private void LoadEquip() => _ui.LoadUI(_UI.EQUIP);

    protected override void OnDisable()
    {
        base.OnDisable();
        _purchaseButton.onClick.RemoveListener(LoadPurchase);
        _equipButton.onClick.RemoveListener(LoadEquip);
    }
}
