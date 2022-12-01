using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WoodPile : InteracterableObject, IPunObservable
{
    [SerializeField] GameObject _wood;
    private static readonly YieldInstruction INTERACT_COOLTIME = new WaitForSeconds(5f);
    private static readonly Vector3[] SPAWN_DIRECTION = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    private const float SPAWN_WOOD_FORCE = 1f;
    private bool _onCooltime = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_onCooltime);
        }
        else if (stream.IsReading)
        {
            _onCooltime = (bool)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 상호작용하면 Wood를 SpawnWood 함수를 RPC로 호출한다.
    /// </summary>
    private const string SPAWN_WOOD = "SpawnWood";
    public override void Interact()
    {
        base.Interact();

        if (_onCooltime)
        {
            return;
        }
        photonView.RPC(SPAWN_WOOD, RpcTarget.All);
    }

    /// <summary>
    /// Wood 오브젝트를 생성한다.
    /// 생성된 오브젝트를 전후좌우 네 방향 중 랜덤으로 골라 그 방향으로 힘을 가해 튀어나가게 한다.
    /// 방향 벡터는 랜덤 방향 대각선 위쪽이다.
    /// 생성 쿨타임을 계산한다. 쿨타임은 5초이다.
    /// </summary>
    private const string WOOD = "Wood";
    [PunRPC]
    private void SpawnWood()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Vector3 spawnDirection = 2f * Vector3.up + SPAWN_DIRECTION[Random.Range(0, 4)];

        GameObject wood = PhotonNetwork.Instantiate(WOOD, transform.position + Vector3.up, transform.rotation);

        wood?.GetComponent<Rigidbody>().AddForce(SPAWN_WOOD_FORCE * spawnDirection, ForceMode.Impulse);

        StartCoroutine(CalculateCooltime());
    }

    /// <summary>
    /// 쿨타임을 계산한다.
    /// 현재 쿨타임인지를 _onCooltime 변수로 판단한다.
    /// 쿨타임은 5초이다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalculateCooltime()
    {
        _onCooltime = true;

        yield return INTERACT_COOLTIME;

        _onCooltime = false;
    }
}
