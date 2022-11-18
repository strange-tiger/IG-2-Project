using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MagicWandGrabManager : MonoBehaviourPun
{
    [SerializeField]
    private MagicWand _magicWand;

    private bool[] _isReady = new bool[2];

    private void Start()
    {
        if (photonView.IsMine)
        {
            _magicWand.enabled = false;
        }
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < _isReady.Length; ++i)
            {
                if (_isReady[i] != true)
                {
                    _isReady[i] = true;
                    break;
                }
            }
            WandState();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 1; i >= 0; --i)
            {
                if (_isReady[i] != false)
                {
                    _isReady[i] = false;
                    break;
                }
            }
            WandState();
        }
    }

    private void WandState()
    {
        if (_isReady[0] == true || _isReady[1] == true)
        {
            _magicWand.enabled = true;
        }
        else
        {
            _magicWand.enabled = false;
        }
    }
}
