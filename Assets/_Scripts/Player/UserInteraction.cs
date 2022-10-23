using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteraction : InteracterableObject
{
    private PlayerNetworking _playerInfo;
    private string _nickname;

    private bool _hasNickname;

    public override void Interact()
    {
        if(!_hasNickname)
        {
            _playerInfo = GetComponent<PlayerNetworking>();
            _nickname = _playerInfo.MyNickname;
            _hasNickname = true;
        }
        Debug.Log("[UI] " + _nickname);
        MenuUIManager.Instance.ShowSocialUI(_nickname);
    }
}
