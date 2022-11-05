using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;
    private bool _grabbed = false;
    private PhotonView _grabberPhotonView;

    private void Awake()
    {
        _objSpawnPos = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<FirstMoveAttackPlayer>() == null)
        {
            return;
        }

        if(_grabbed)
        {
            if(other.gameObject.GetPhotonView() == _grabberPhotonView)
            {
                return;
            }

            photonView.RPC("Crack", RpcTarget.All, 2f);
            FirstMoveAttackPlayer _player = other.GetComponent<FirstMoveAttackPlayer>();
            _player.photonView.RPC("OnDamage", RpcTarget.All);
        }
        else
        {
            _grabberPhotonView = other.gameObject.GetPhotonView();
            _grabbed= true;
        }
        
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
        _grabbed = false;
        _grabberPhotonView = null;
    }
}
