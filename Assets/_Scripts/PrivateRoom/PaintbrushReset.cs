using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PaintbrushReset : MonoBehaviourPun
{
    [SerializeField] Button _resetButton;

    public event Action OnReset;

    private void OnEnable()
    {
        _resetButton.onClick.RemoveListener(ResetPad);
        _resetButton.onClick.AddListener(ResetPad);
    }

    private void OnDisable()
    {
        _resetButton.onClick.RemoveListener(ResetPad);
        photonView.RPC("ResetDraw", RpcTarget.AllBuffered);
    }

    private void ResetPad()
    {
        photonView.RPC("ResetDraw", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ResetDraw()
    {
        for (int i = 1; i < transform.childCount; ++i)
        {
            PhotonNetwork.Destroy(transform.GetChild(i).gameObject);
        }
        OnReset.Invoke();
    }
}
