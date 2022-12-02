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

    private WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(60f);

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

        Debug.Log($"{photonView.IsMine}오크통과 상호작용");
        photonView.RPC(nameof(SomeoneInteractedOakBarrel), RpcTarget.AllBuffered, false);
    }

    [PunRPC]
    private void SomeoneInteractedOakBarrel(bool value)
    {
        _oakBarrelMeshRenderer.enabled = value;
        _oakBarrelMeshCollider.enabled = value;
        Debug.Log($"{photonView.IsMine}오크통과 상호작용 RPC 뿌리기");
    }

    private IEnumerator SetOakBarrelOriginalPosition()
    {
        yield return _oakBarrelReturnTime;
        photonView.RPC(nameof(SomeoneInteractedOakBarrel), RpcTarget.AllBuffered, true);
    }
}