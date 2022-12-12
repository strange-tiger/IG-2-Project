using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPun
{
    [SerializeField] GameObject _dice;

    private Transform _hostPlayer;

    /// <summary>
    /// 마스터 클라이언트의 플레이어 위치를 _hostPlayer에 저장한다.
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayerTransform(Transform player)
    {
        _hostPlayer = player;
    }

    /// <summary>
    /// 호출될 때마다 토글처럼 주사위의 활성화 여부를 결정한다.
    /// 주사위가 비활성화되어 있으면 Spawn을, 활성화되어 있으면 Despawn을 RPC로 호출한다.
    /// </summary>
    public void ToggleDice()
    {
        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(Spawn));
        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, nameof(Despawn));

        if (!_dice.activeSelf)
        {
            photonView.RPC(nameof(Spawn), RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC(nameof(Despawn), RpcTarget.AllBuffered);
        }
    }

    private static readonly Vector3 SPAWN_POSITION = new Vector3(-1f, 2f, 4.8f);
    /// <summary>
    /// 주사위의 위치를 SPAWN_POSITION으로 변경하고 활성화한다.
    /// </summary>
    [PunRPC]
    private void Spawn()
    {
        if (photonView.IsMine)
        {
            transform.position = SPAWN_POSITION;
        }
        _dice.SetActive(true);
    }

    /// <summary>
    /// 주사위를 비활성화한다.
    /// </summary>
    [PunRPC]
    private void Despawn()
    {
        _dice.SetActive(false);
    }
}
