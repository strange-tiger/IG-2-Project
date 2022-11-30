using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EPOOutline;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrel : InteracterableObject
{
    [SerializeField] private bool _isPlayerHave;

    private MeshRenderer _oakBarrelMeshRenderer;
    private MeshCollider _oakBarrelMeshCollider;

    private WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(30f);


    protected override void OnEnable()
    {
        if (gameObject.transform.root.tag.Contains("Player"))
        {
            _isPlayerHave = true;
        }
        else
        {
            _isPlayerHave = false;
        }
    }

    private void Start()
    {
        _oakBarrelMeshRenderer = GetComponent<MeshRenderer>();
        _oakBarrelMeshCollider = GetComponent<MeshCollider>();
    }

    public override void Interact()
    {
        base.Interact();

        if (_isPlayerHave == false)
        {
            StartCoroutine(SetOakBarrelOriginalPosition());
        }

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, false);
    }

    [PunRPC]
    public void SomeoneInteractedOakBarrel(bool value)
    {
        _oakBarrelMeshRenderer.enabled = value;
        _oakBarrelMeshCollider.enabled = value;
    }

    private IEnumerator SetOakBarrelOriginalPosition()
    {
        yield return _oakBarrelReturnTime;

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}