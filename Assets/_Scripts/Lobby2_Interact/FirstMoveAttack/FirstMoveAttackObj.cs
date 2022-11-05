using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;
    private bool _grabbed = false;
    private int _grabberPhotonID;
    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private MeshRenderer _objMeshRenderer;
    [SerializeField]
    private MeshCollider _objMeshCollider;

    private void Awake()
    {
        _objSpawnPos = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        FirstMoveAttackPlayer _player = other.GetComponent<FirstMoveAttackPlayer>();

        if (_player == null)
        {
            return;
        }

        if(PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if(_grabbed)
        {
            if(_player.photonView.ViewID == _grabberPhotonID)
            {
                return;
            }

            photonView.RPC("Crack", RpcTarget.All, 2f);
            _player.photonView.RPC("OnDamage", RpcTarget.All);
        }
        else
        {
            _grabberPhotonID = _player.photonView.ViewID;
            _grabbed = true;
        }
        
    }

    private void OnDestroy()
    {
        if(coRespawn != null)
        {
            StopCoroutine(coRespawn);
            coRespawn = null;
        }
    }

    [PunRPC]
    public void Crack(float respawnTime)
    {
        Debug.Log("Crack");
        //_audioSource.volume = SoundManager.Instance.SFXVolume;
        _audioSource.Play();
        TurnOff();
        Respawn(respawnTime);
    }

    public void TurnOff()
    {
        _boxCollider.enabled = false;
        _objMeshRenderer.enabled = false;
        _objMeshCollider.enabled = false;
    }

    public void TurnOn()
    {
        _boxCollider.enabled = true;
        _objMeshRenderer.enabled = true;
        _objMeshCollider.enabled = true;
    }

    Coroutine coRespawn = null;
    public void Respawn(float delay)
    {
        Debug.Log("Respawn");
        if(coRespawn != null)
        {
            // 코루틴이 도는 중간에 들어옴
            return;
        }
        coRespawn = StartCoroutine(RespawnHelper(delay));
    }

    IEnumerator RespawnHelper(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.transform.position = _objSpawnPos;
        _grabbed = false;
        _grabberPhotonID = -1;

        coRespawn = null;
        TurnOn();
    }
}
