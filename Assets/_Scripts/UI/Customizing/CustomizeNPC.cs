using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CustomizeNPC : InteracterableObject
{

    [SerializeField] GameObject _customizeNPCMenu;
    [SerializeField] PlayerNetworking _playerNetworking;
    [SerializeField] Collider _collider;

    public override void Interact()
    {
        base.Interact();

        if(_collider.gameObject.GetComponent<PhotonView>().IsMine)
        {
            _playerNetworking =_collider.gameObject.GetComponent<PlayerNetworking>();
        }

        _customizeNPCMenu.SetActive(true);
    }
}
