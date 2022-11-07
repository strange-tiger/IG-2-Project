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

    //[SerializeField]
    //private BoxCollider _boxCollider;
    [SerializeField]
    private MeshRenderer _objMeshRenderer;
    [SerializeField]
    private MeshCollider _objMeshCollider;

    private YieldInstruction _respawnCoolTime = new WaitForSeconds(2f);
    private SyncOVRGrabbable _syncGrabbable;
    private PhotonView _grabber = null;

    private void Start()
    {
        _objSpawnPos = transform.position;
        _audioSource = GetComponent<AudioSource>();
        _syncGrabbable = GetComponent<SyncOVRGrabbable>();
        _syncGrabbable.CallbackOnGrabBegin = OnGrabBegin;
        _syncGrabbable.CallbackOnGrabEnd = OnGrabEnd;
        _syncGrabbable.CallbackGrabberSetting = GrabberSetting;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isGrabbed == false)
        {
            return;
        }

        if(other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("음 플레이어가 들어왔구만");
        _objMeshCollider.isTrigger = true;

        if(_grabber == other.transform.root.gameObject.GetPhotonView())
        {
            return;
        }

        PhotonView otherPlayer = other.transform.root.gameObject.GetPhotonView();
        otherPlayer.RPC("OnDamageByBottle", RpcTarget.All);
        this.photonView.RPC("Crack", RpcTarget.All);

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("PlayerHand") == false)
    //    {
    //        return;
    //    }

    //    Debug.Log("플레이어 나갔다!");

    //    if(PhotonNetwork.IsMasterClient)
    //    {
    //        photonView.RPC("TurnOff", RpcTarget.All);
    //        photonView.RPC("Respawn", RpcTarget.All);
    //    }
    //}

    private void OnDestroy()
    {
        if(coRespawn != null)
        {
            StopCoroutine(coRespawn);
            coRespawn = null;
        }
    }

    [PunRPC]
    public void OnGrabBegin()
    {
        _isGrabbed = true;
        photonView.RPC("OnGrabBegin", RpcTarget.Others);
    }

    [PunRPC]
    public void OnGrabEnd()
    {
        _isGrabbed = false;
        _objMeshCollider.isTrigger = false;
        _grabber = null;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = _objSpawnPos;
        photonView.RPC("OnGrabEnd", RpcTarget.Others);
    }

    public void GrabberSetting(PhotonView photonView)
    {
        _grabber = photonView;
    }

    [PunRPC]
    public void Crack()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _audioSource.Play();
        }
        TurnOff();
        Respawn();
    }

    public void TurnOff()
    {
        //_boxCollider.enabled = false;
        _objMeshRenderer.enabled = false;
        _objMeshCollider.enabled = false;
    }

    public void TurnOn()
    {
        //_boxCollider.enabled = true;
        _objMeshRenderer.enabled = true;
        _objMeshCollider.enabled = true;
    }

    private Coroutine coRespawn = null;
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
       // _objMeshCollider.isTrigger = false;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = _objSpawnPos;
        //photonView.RPC("OnOtherPlayerGrabEnd", RpcTarget.All);

        coRespawn = null;
        TurnOn();
    }
}
