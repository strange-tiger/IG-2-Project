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

    private WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(30f);

    private void OnEnable()
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Interact();
        }
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
    public void SomeoneInteractedOakBarrel(bool isTrueFalse)
    {
        _oakBarrelMeshRenderer.enabled = isTrueFalse;
    }

    private IEnumerator SetOakBarrelOriginalPosition()
    {
        yield return _oakBarrelReturnTime;

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}