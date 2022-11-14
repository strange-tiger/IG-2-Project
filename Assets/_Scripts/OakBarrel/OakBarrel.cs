using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrel : InteracterableObject
{
    private Outlinable _outlinable;
    public UnityEvent CoveredOakBarrel = new UnityEvent();
    
    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(120f);

    private void Start()
    {
        _outlinable = GetComponent<Outlinable>();
    }

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
        if (photonView.IsMine)
        {
            if (_outlinable.enabled == true)
            {
                gameObject.SetActive(isTrueFalse);
            }
            else
            {
                gameObject.SetActive(isTrueFalse);
            }
        }
    }

    private IEnumerator SetOakBarrelOriginalPosition()
    {
        base.Interact();

        CoveredOakBarrel.Invoke();

        yield return _oakBarrelReturnTime;

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}