using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;

public class PlayerInteractionSencer : MonoBehaviour
{
    private static bool _isNearInteraction;
    public bool IsNearInteraction
    {
        get => _isNearInteraction;
        set
        {
            _isNearInteraction = value;
        }
    }

    public PlayerInput Input { get; set; }
    public SyncOVRGrabber[] Grabbers { get; set; }

    protected string _nickname = "";

    public virtual void GetGold(int gold)
    {
        Debug.Log("Gold ¹ÞÀ½ " + gold);

        if (_nickname == "")
        {
            _nickname = GetComponent<BasicPlayerNetworking>().MyNickname;
        }

        MySqlSetting.EarnGold(_nickname, gold);
        IsNearInteraction = false;
    }
}
