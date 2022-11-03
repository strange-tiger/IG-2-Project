using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;

    private void Awake()
    {
        _objSpawnPos = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    [PunRPC]
    public void Crack(float respawnTime)
    {
        _audioSource.volume = SoundManager.Instance.SFXVolume;
        _audioSource.Play();
        Invoke("Respawn", respawnTime);
        PhotonNetwork.Destroy(gameObject);
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate(gameObject.name, _objSpawnPos, Quaternion.identity);
    }
}
