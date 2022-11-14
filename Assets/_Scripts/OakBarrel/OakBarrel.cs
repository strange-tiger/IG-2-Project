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
    
    private float _oakBarrelReturnTime = 5f;

    private void Start()
    {
        _outlinable = GetComponent<Outlinable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("�ѿ־ȴ�");
            Interact();
        }
    }

    public override void Interact()
    {
        base.Interact();

        Debug.Log("�����");

        CoveredOakBarrel.Invoke();

        Invoke("SetOakBarrelOriginalPosition", _oakBarrelReturnTime);

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, false);
    }

    [PunRPC]
    public void SomeoneInteractedOakBarrel(bool isTrueFalse)
    {
        //if (photonView.IsMine)
        {
            Debug.Log("���Ǿ� �߽�!");
            gameObject.SetActive(isTrueFalse);
        }
    }

    private void SetOakBarrelOriginalPosition()
    {
        base.Interact();

        CoveredOakBarrel.Invoke();

        photonView.RPC("SomeoneInteractedOakBarrel", RpcTarget.All, true);
    }
}