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

    public void OnTriggerExit(Collider other) // 놓쳐버린 경우, 조금 더 디테일할 필요 있음
    {
        if (other.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
            Respawn();
        }
    }

    [PunRPC]
    public void Crack()
    {
        //SoundManager.Instance.PlaySFXSound("쨍그랑!"); //해당 사운드 //근데 이것도 동기화 해야 할 듯
        Invoke("Respawn", 2f); // fade-in delay 타임 넣을 것
        PhotonNetwork.Destroy(gameObject);
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate(gameObject.name, _objSpawnPos, Quaternion.identity);
    }
}
