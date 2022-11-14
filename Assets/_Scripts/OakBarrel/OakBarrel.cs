using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrel : InteracterableObject
{
    public UnityEvent CoveredOakBarrel = new UnityEvent();
    
    private float _oakBarrelReturnTime = 20f;

    public override void Interact()
    {
        base.Interact();

        CoveredOakBarrel.Invoke();

        Invoke("SetOakBarrelOriginalPosition", _oakBarrelReturnTime);

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, false);

        
    }

    [PunRPC]
    public void SomeoneInteractedOakBarrel(bool isTrueFalse)
    {
        gameObject.SetActive(isTrueFalse);
    }

    private void SetOakBarrelOriginalPosition()
    {
        base.Interact();

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}