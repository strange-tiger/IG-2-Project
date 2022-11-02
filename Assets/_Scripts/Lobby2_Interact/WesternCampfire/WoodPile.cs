#define _Photon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WoodPile : InteracterableObject
{
    [SerializeField] GameObject _wood;
    private static readonly YieldInstruction INTERACT_COOLTIME = new WaitForSeconds(5f);
    private static readonly Vector3[] SPAWN_DIRECTION = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    private const float SPAWN_WOOD_FORCE = 1.5f;
    private bool _onCooltime = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
            stream.SendNext(_onCooltime);
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
            _onCooltime = (bool)stream.ReceiveNext();
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (_onCooltime)
        {
            return;
        }
#if _Photon
        photonView.RPC("SpawnWood", RpcTarget.All);
#else
        Vector3 spawnDirection = Vector3.up + SPAWN_DIRECTION[Random.Range(0, 4)];

        GameObject wood = Instantiate(_wood, Vector3.zero, Quaternion.identity);

        wood?.GetComponent<Rigidbody>().AddForce(SPAWN_WOOD_FORCE * spawnDirection, ForceMode.Impulse);

        StartCoroutine(CalculateCooltime());
#endif
    }

    [PunRPC]
    private void SpawnWood()
    {
        Vector3 spawnDirection = 2f * Vector3.up + SPAWN_DIRECTION[Random.Range(0, 4)];

        GameObject wood = PhotonNetwork.Instantiate("Wood", transform.position, transform.rotation);

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
