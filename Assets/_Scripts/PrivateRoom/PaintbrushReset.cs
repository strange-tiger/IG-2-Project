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
        ResetPad();
    }

    /// <summary>
    /// 생성된 선을 모두 삭제한다.
    /// ResetDraw를 RPC로 호출한다.
    /// 호출 전에 ResetDraw의 축적된 캐시를 삭제한다.
    /// </summary>
    private void ResetPad()
    {
        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(ResetDraw));
        photonView.RPC(nameof(ResetDraw), RpcTarget.AllBuffered);
    }

    /// <summary>
    /// 이 오브젝트의 자식으로 생성된 선 오브젝트를 모두 삭제한다.
    /// OnReset 이벤트를 통보한다.
    /// </summary>
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
