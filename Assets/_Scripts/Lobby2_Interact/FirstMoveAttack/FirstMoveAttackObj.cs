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
        //SoundManager.Instance.PlaySFXSound("쨍그랑!"); //해당 사운드 //근데 이것도 동기화 해야 할 듯
        Invoke("Respawn", respawnTime);
        PhotonNetwork.Destroy(gameObject);
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate(gameObject.name, _objSpawnPos, Quaternion.identity);
    }
}
