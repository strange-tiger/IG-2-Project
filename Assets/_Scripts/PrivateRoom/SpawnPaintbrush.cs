using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPun
{
    [SerializeField] GameObject _legalPad;
    [SerializeField] GameObject _pencil;

    private Transform _clientPlayer;
    
    /// <summary>
    /// 이 그림판을 소유한 클라이언트 플레이어의 위치를 저장한다.
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayerTransform(Transform player)
    {
        _clientPlayer = player;
    }

    /// <summary>
    /// 호출될 때마다 토글처럼 패드와 연필의 활성화 여부를 결정한다.
    /// _legalPad가 비활성화되어 있다면 Spawn을, 활성화되어 있다면 Despawn을 RPC로 호출한다.
    /// </summary>
    public void TogglePaintbrush()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(Spawn));
        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(Despawn));

        if (!_legalPad.activeSelf)
        {
            photonView.RPC(nameof(Spawn), RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC(nameof(Despawn), RpcTarget.AllBuffered);
        }
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 0f, 1.5f);
    /// <summary>
    /// 플레이어의 SPAWN_PAD_POSITION만큼 앞에 패드와 연필을 위치시키고 활성화한다.
    /// </summary>
    [PunRPC]
    private void Spawn()
    {
        if (photonView.IsMine)
        {
            transform.position = _clientPlayer.position + SPAWN_PAD_POSITION;
        }
        _legalPad.SetActive(true);
        _pencil.SetActive(true);
    }

    /// <summary>
    /// 패드와 연필을 비활성화한다.
    /// </summary>
    [PunRPC]
    private void Despawn()
    {
        _legalPad.SetActive(false);
        _pencil.SetActive(false);
    }
}
