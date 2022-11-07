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
        if (_isGrabbed == false || _isMine == false )
        {
            return;
        }
        Debug.Log("�׷�!");

        if(other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("�� �÷��̾ ���Ա���");
        _objMeshCollider.isTrigger = true;

        if(_grabber == other.transform.root.gameObject.GetPhotonView())
        {
            return;
        }

        PhotonView photonView = other.GetComponent<PhotonView>();

        PlayerNetworking player = other.GetComponentInParent<PlayerNetworking>();
        player.photonView.RPC("OnDamageByBottle", RpcTarget.All, player.photonView.ViewID);
        this.photonView.RPC("Crack", RpcTarget.All);

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("PlayerHand") == false)
    //    {
    //        return;
    //    }

    //    Debug.Log("�÷��̾� ������!");

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

    public void OnGrabBegin()
    {
        if (photonView.IsMine)
        {
            _isGrabbed = true;
            _isMine = true;
            photonView.RPC("OnOtherPlayerGrabBegin", RpcTarget.Others);
        }
    }

    public void OnGrabEnd()
    {
        if (photonView.IsMine)
        {
            _isGrabbed = false;
            _isMine = false;
            photonView.RPC("OnOtherPlayerGrabEnd", RpcTarget.Others);
        }
    }

    public void GrabberSetting(PhotonView photonView)
    {
        _grabber = photonView;
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

    Coroutine coRespawn = null;
    public void Respawn()
    {
        Debug.Log("Respawn");
        if(coRespawn != null)
        {
            // �ڷ�ƾ�� ���� �߰��� ����
            return;
        }
        coRespawn = StartCoroutine(RespawnHelper());
    }

    IEnumerator RespawnHelper()
    {
        yield return _respawnCoolTime;
        _objMeshCollider.isTrigger = false;
        gameObject.transform.position = _objSpawnPos;
        //photonView.RPC("OnOtherPlayerGrabEnd", RpcTarget.All);

        coRespawn = null;
        TurnOn();
    }
}
