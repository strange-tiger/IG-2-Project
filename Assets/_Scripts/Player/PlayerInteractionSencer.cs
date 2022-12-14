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

    public bool IsGrabbing
    {
        get
        {
            foreach(SyncOVRGrabber grabber in Grabbers)
            {
                if(grabber.grabbedObject)
                {
                    return true;
                }
            }

            return false;
        }
    }

    [SerializeField] private PlayerInput _input;
    public PlayerInput Input { get => _input; set => _input = value; }

    [SerializeField] private SyncOVRGrabber[] _grabbers;
    public SyncOVRGrabber[] Grabbers { get => _grabbers; set => _grabbers = value; }

    protected string _nickname = "";

    public virtual void GetGold(int gold)
    {
        Debug.Log("Gold ???? " + gold);

        if (_nickname == "")
        {
            _nickname = transform.root.GetComponent<BasicPlayerNetworking>().MyNickname;
        }

        MySqlSetting.EarnGold(_nickname, gold);
        IsNearInteraction = false;
    }
}
