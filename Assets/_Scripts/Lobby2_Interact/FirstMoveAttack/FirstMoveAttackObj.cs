using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;

    private bool _isGrabbed = false;
    private bool _isMine = false;

    [SerializeField]
    private BoxCollider _boxCollider;
    [SerializeField]
    private MeshRenderer _objMeshRenderer;
    [SerializeField]
    private MeshCollider _objMeshCollider;

    private YieldInstruction _respawnCoolTime = new WaitForSeconds(2f);
    private SyncOVRGrabbable _syncGrabbable;

    private void Awake()
    {
        _objSpawnPos = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _syncGrabbable = GetComponent<SyncOVRGrabbable>();
        _syncGrabbable.CallbackOnGrabBegin = OnGrabBegin;
        _syncGrabbable.CallbackOnGrabEnd = OnGrabEnd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isGrabbed == false || _isMine == false )
        {
            return;
        }

        PlayerNetworking player = other.GetComponentInParent<PlayerNetworking>();
        if(player == null)
        {
            return;
        }

        PhotonView photonView = other.GetComponent<PhotonView>();
        if(photonView == null)
        {
            return;
        }
        
        player.photonView.RPC("OnDamageByBottle", RpcTarget.All, player.photonView.ViewID);
        this.photonView.RPC("Crack", RpcTarget.All);
    }

    private void OnDestroy()
    {
        if(coRespawn != null)
        {
            StopCoroutine(coRespawn);
            coRespawn = null;
        }
    }

    public void OnGrabBegin()
    {
        _isGrabbed = true;
        _isMine = true;
        photonView.RPC("OnOtherPlayerGrabBegin", RpcTarget.Others);
    }

    public void OnGrabEnd()
    {
        _isGrabbed = false;
        _isMine = false;
        photonView.RPC("OnOtherPlayerGrabEnd", RpcTarget.Others);
    }

    [PunRPC]
    private void OnOtherPlayerGrabBegin()
    {
        _isGrabbed = true;
        _isMine = false;
    }

    [PunRPC]
    private void OnOtherPlayerGrabEnd()
    {
        _isGrabbed = false;
        _isMine = false;
    }

    [PunRPC]
    public void Crack()
    {
        _audioSource.Play();
        TurnOff();
        Respawn();
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
    public void Respawn()
    {
        Debug.Log("Respawn");
        if(coRespawn != null)
        {
            // 코루틴이 도는 중간에 들어옴
            return;
        }
        coRespawn = StartCoroutine(RespawnHelper());
    }

    IEnumerator RespawnHelper()
    {
        yield return _respawnCoolTime;

        gameObject.transform.position = _objSpawnPos;
        photonView.RPC("OnOtherPlayerGrabEnd", RpcTarget.All);

        coRespawn = null;
        TurnOn();
    }
}
