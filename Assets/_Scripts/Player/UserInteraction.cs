using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteraction : InteracterableObject
{
    private PlayerNetworking _playerInfo;

    private string nickname;

    private void OnEnable()
    {
        _playerInfo = GetComponent<PlayerNetworking>();
        nickname = _playerInfo.MyNickname;
    }

    public override void Interact()
    {
        MenuUIManager.Instance.ShowSocialUI(nickname);
    }
}
