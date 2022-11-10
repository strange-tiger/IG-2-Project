using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : MonoBehaviourPun
{
    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;

    private bool _isGrabbed = false;

    [SerializeField]
    private BoxCollider _objCollider;
    [SerializeField]
    private MeshRenderer _objMeshRenderer;

    private SyncOVRGrabber _grabber = null;
    private SyncOVRGrabbable _syncGrabbable;
    private PhotonView _grabberPhotonView = null;

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
        // 그랩 전까진 모든 Enter 무시
        if (_isGrabbed == false)
        {
            return;
        }

        _objCollider.isTrigger = true;

        // 그랩 후 플레이어 태그를 가진 오브젝트만 인식
        if(other.CompareTag("PlayerBody") == false)
        {
            return;
        }

        //플레이어 태그가 인식되면 현재 잡고있는 사람의 photonView와 비교
        if (_grabberPhotonView == other.transform.root.gameObject.GetPhotonView())
        {
            return;
        }

        // 일치하지 않으면 병이 깨지고 타격을 받음
        this.photonView.RPC("Crack", RpcTarget.All);
        
        FirstMoveAttackPlayer player = other.transform.root.GetComponentInChildren<FirstMoveAttackPlayer>();
        player.photonView.RPC("OnDamageByBottle", RpcTarget.All);
    }


    [PunRPC]
    public void OnGrabBegin()
    {
        Debug.Log("OnGrabBegin");
        _isGrabbed = true;
        if(photonView.IsMine)
        {
            photonView.RPC("OnGrabBegin", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void OnGrabEnd()
    {
        Debug.Log("OnGrabEnd");
        _isGrabbed = false;
        _objCollider.isTrigger = false;
        _grabberPhotonView = null;
        ObjPosReset();

        if (photonView.IsMine)
        {
            photonView.RPC("OnGrabEnd", RpcTarget.Others);
        }
    }

    public void GrabberSetting(PhotonView grabberPhotonView, SyncOVRGrabber grabber)
    {
        _grabberPhotonView = grabberPhotonView;
        _grabber = grabber;
    }

    [PunRPC]
    public void Crack()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _audioSource.Play();
        }
        _grabber?.GrabEnd();
        TurnOff();
        Invoke("Respawn", 2f);
    }

    private void TurnOff()
    {
        _objMeshRenderer.enabled = false;
        _objCollider.enabled = false;
    }
    public void Respawn()
    {
        ObjPosReset();
        TurnOn();
    }
    private void ObjPosReset()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = _objSpawnPos;
    }

    private void TurnOn()
    {
        _objMeshRenderer.enabled = true;
        _objCollider.enabled = true;
    }

}
