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

    // 오크통이 돌아오는 시간
    private WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(60f);

    private void Start()
    {
        _oakBarrelMeshRenderer = GetComponent<MeshRenderer>();
        _oakBarrelMeshCollider = GetComponent<MeshCollider>();
    }

    public override void Interact()
    {
        base.Interact();

        // 플레이어의 오크통인지 아닌지
        if (_isPlayerHave == false)
        {
            StartCoroutine(SetOakBarrelOriginalPosition());
        }

        photonView.RPC(nameof(SomeoneInteractedOakBarrel), RpcTarget.AllBuffered, false);
    }

    [PunRPC]
    private void SomeoneInteractedOakBarrel(bool value)
    {
        _oakBarrelMeshRenderer.enabled = value;
        _oakBarrelMeshCollider.enabled = value;
    }

    // 야생의 오크통이면 일정시간 후 돌아오게 됨
    private IEnumerator SetOakBarrelOriginalPosition()
    {
        yield return _oakBarrelReturnTime;
        photonView.RPC(nameof(SomeoneInteractedOakBarrel), RpcTarget.AllBuffered, true);
    }
}