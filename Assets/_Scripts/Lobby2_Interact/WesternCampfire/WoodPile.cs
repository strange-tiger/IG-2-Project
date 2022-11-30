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

    private IEnumerator CalculateCooltime()
    {
        _onCooltime = true;

        yield return INTERACT_COOLTIME;

        _onCooltime = false;
    }
}
