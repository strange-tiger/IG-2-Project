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


    private void OnTriggerEnter(Collider other) // 상대 머리 가격한 경우
    {
        if(other.CompareTag("Player")) // 근데 이러면 그냥 내 손만 닿아도 깨질 것 같은데? 오브젝트 입장에서 나 자신을 어떻게 제외시켜야할까?
        {
            SoundManager.Instance.PlaySFXSound("쨍그랑!/"); //해당 사운드
            PhotonNetwork.Destroy(gameObject);
            Invoke("Respawn", 2f);
            Stun();
        }
    }

    public void OnTriggerExit(Collider other) // 놓쳐버린 경우
    {
        if (other.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
            Respawn();
        }
    }

    public void Stun()
    {
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
    }

    public void Respawn()
    {
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
        PhotonNetwork.Instantiate("bottle", _objSpawnPos, Quaternion.identity);
    }

}
