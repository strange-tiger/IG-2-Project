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

    public void OnTriggerExit(Collider other) // ���Ĺ��� ���, ���� �� �������� �ʿ� ����
    {
        if (other.CompareTag("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
            Respawn();
        }
    }

    public void Crack()
    {
        //SoundManager.Instance.PlaySFXSound("¸�׶�!"); //�ش� ���� //�ٵ� �̰͵� ����ȭ �ؾ� �� ��
        Invoke("Respawn", 2f); // fade-in delay Ÿ�� ���� ��
        PhotonNetwork.Destroy(gameObject);
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate(gameObject.name, _objSpawnPos, Quaternion.identity);
    }
}
