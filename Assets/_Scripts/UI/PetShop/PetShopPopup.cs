using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using _UI = Defines.EPetUIIndex;

public class PetShopPopup : PopupUI
{
    [SerializeField] Button _purchaseButton;
    [SerializeField] Button _transformButton;


    [Header("UIManager")]
    [SerializeField] PetUIManager _ui;

    protected override void OnEnable()
    {
        base.OnEnable();

        _purchaseButton.onClick.RemoveListener(LoadPurchase);
        _purchaseButton.onClick.AddListener(LoadPurchase);

        _transformButton.onClick.RemoveListener(LoadTransform);
        _transformButton.onClick.AddListener(LoadTransform);
    }

    private void LoadPurchase() => _ui.LoadUI(_UI.PURCHASE);
    private void LoadTransform() => _ui.LoadUI(_UI.TRANSFORM);
    protected override void Close()
    {
        _ui.Npc.OnFocus();

        base.Close();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _purchaseButton.onClick.RemoveListener(LoadPurchase);
        _transformButton.onClick.RemoveListener(LoadTransform);
    }
}
