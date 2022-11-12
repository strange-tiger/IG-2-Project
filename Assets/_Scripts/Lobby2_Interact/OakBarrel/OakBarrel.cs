using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrel : InteracterableObject
{
    public UnityEvent CoveredOakBarrel = new UnityEvent();

    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(120f);

    public override void Interact()
    {
        base.Interact();

        CoveredOakBarrel.Invoke();

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, false);

        StartCoroutine(SetOakBarrelOriginalPosition());
    }

    [PunRPC]
    private void SomeoneInteractedOakBarrel(bool isTrueFalse)
    {
        gameObject.SetActive(isTrueFalse);
    }

    private IEnumerator SetOakBarrelOriginalPosition()
    {
        yield return _oakBarrelReturnTime;

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}
