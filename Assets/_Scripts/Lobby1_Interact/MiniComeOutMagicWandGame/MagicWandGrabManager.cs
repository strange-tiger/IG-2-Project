using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MagicWandGrabManager : MonoBehaviourPun
{
    [SerializeField]
    private MagicWand _magicWand;

    private void Start()
    {
        if (photonView.IsMine)
        {
            _magicWand.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _magicWand.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _magicWand.enabled = false;
        }
    }

}
