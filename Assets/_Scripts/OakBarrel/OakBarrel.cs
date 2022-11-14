using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrel : InteracterableObject
{
    public UnityEvent CoveredOakBarrel = new UnityEvent();
    
    private float _oakBarrelReturnTime = 10f;

    public override void Interact()
    {
        base.Interact();

        Debug.Log("들어가라고");

        Invoke("SetOakBarrelOriginalPosition", _oakBarrelReturnTime);

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, false);
    }

    [PunRPC]
    public void SomeoneInteractedOakBarrel(bool isTrueFalse)
    {
        gameObject.SetActive(isTrueFalse);

        CoveredOakBarrel.Invoke();
    }

    private void SetOakBarrelOriginalPosition()
    {
        base.Interact();

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}