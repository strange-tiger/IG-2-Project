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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
            stream.SendNext(transform.childCount);


            for (int i = 1; i < transform.childCount; ++i)
            {
                stream.SendNext(transform.GetChild(i).gameObject);
            }
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
            int count = (int)stream.ReceiveNext();

            for (int i = 1; i < count; ++i)
            {
                Instantiate((GameObject)stream.ReceiveNext()).transform.parent = this.transform;
            }
        }
    }

    private void ResetPad()
    {
        photonView.RPC("ResetDraw", RpcTarget.All);
    }

    [PunRPC]
    private void ResetDraw()
    {
        for(int i = 1; i < transform.childCount; ++i)
        {
            PhotonNetwork.Destroy(transform.GetChild(i).gameObject);
        }
        OnReset.Invoke();
    }
}
