using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UserInteraction : InteracterableObject
{
    private PlayerNetworking _playerInfo;
    public GameObject RequestAlarmImage { private get; set; }

    public string Nickname { get; private set; }

    private bool _hasNickname;

    public override void Interact()
    {
        if(!_hasNickname)
        {
            _playerInfo = GetComponent<PlayerNetworking>();
            Nickname = _playerInfo.MyNickname;
            _hasNickname = true;
        }
        MenuUIManager.Instance.ShowSocialUI(this);
    }

    [PunRPC]
    public void SendRequest()
    {
        Debug.Log("¿Ö");
        if(photonView.IsMine)
        {
            if(RequestAlarmImage.activeSelf)
            {
                return;
            }

            RequestAlarmImage.SetActive(true);
        }
    }
}
