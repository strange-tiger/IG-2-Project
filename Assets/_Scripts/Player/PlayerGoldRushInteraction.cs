using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldRushInteraction : PlayerInteractionSencer
{
    private bool _isNearGoldRush;
    public bool IsNearGoldRush
    {
        get
        {
            return IsNearInteraction || _isNearGoldRush;
        }
        set
        {
            Debug.Log($"[GoldRush] Player {value}");
            _isNearGoldRush = value;
            IsNearInteraction = value;
        }
    }

    public bool HasInteract { get; private set; }
    private void Update()
    {
        HasInteract = Input.InputADown;
    }

    public override void GetGold(int gold)
    {
        base.GetGold(gold);
        _isNearGoldRush = false;
    }
}
