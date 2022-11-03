using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;

    private void Awake()
    {
        _objSpawnPos = transform.position;
    }

    [PunRPC]
    public void Crack(float respawnTime)
    {
        SoundManager.Instance.PlaySFXSound("Â¸±×¶û!");
        Invoke("Respawn", respawnTime);
        PhotonNetwork.Destroy(gameObject);
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate(gameObject.name, _objSpawnPos, Quaternion.identity);
    }
}
