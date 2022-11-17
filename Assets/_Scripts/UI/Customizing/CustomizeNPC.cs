using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class CustomizeNPC : InteracterableObject
{

    [SerializeField] GameObject _customizeNPCMenu;
    private MeshCollider _collider;

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
    }



    public override void Interact()
    {
        base.Interact();

        _customizeNPCMenu.SetActive(true);

        _collider.enabled = false;
    }

 
}
