using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrel : InteracterableObject
{
    private Outlinable _outlinable;

    public static UnityEvent CoveredOakBarrel = new UnityEvent();

    void Start()
    {
        _outlinable = GetComponent<Outlinable>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (_outlinable.enabled == true)
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    CoveredOakBarrel.Invoke();
                    
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    public override void Interact()
    {
        base.Interact();

        CoveredOakBarrel.Invoke();


    }
}
