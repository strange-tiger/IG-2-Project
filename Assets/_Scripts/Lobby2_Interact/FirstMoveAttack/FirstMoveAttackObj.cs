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
        // �׷� ������ ��� Enter ����
        if (_isGrabbed == false)
        {
            return;
        }

        _objCollider.isTrigger = true;

        // �׷� �� �÷��̾� �±׸� ���� ������Ʈ�� �ν�
        if(other.CompareTag("PlayerBody") == false)
        {
            return;
        }

        Debug.Log(_grabberPhotonView);
        Debug.Log(other.transform.root.gameObject.GetPhotonView());

        // �÷��̾� �±װ� �νĵǸ� ���� ����ִ� ����� photonView�� �� 
        if (_grabberPhotonView == other.transform.root.gameObject.GetPhotonView())
        {
            return;
        }

        // ��ġ���� ������ ���� ������ Ÿ���� ����
        PhotonView photonView = other.GetComponent<PhotonView>();

        this.photonView.RPC("Crack", RpcTarget.All);
        PlayerNetworking player = other.GetComponentInParent<PlayerNetworking>();
        player.photonView.RPC("OnDamageByBottle", RpcTarget.All, player.photonView.ViewID);
    }


    [PunRPC]
    public void OnGrabBegin()
    {
        Debug.Log("GrabBegin");
        _isGrabbed = true;
        if(photonView.IsMine)
        {
            photonView.RPC("OnGrabBegin", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void OnGrabEnd()
    {
        Debug.Log("GrabEnd");
        _isGrabbed = false;
        _objCollider.isTrigger = false;
        _grabberPhotonView = null;
        ObjPosReset();

        if (photonView.IsMine)
        {
            photonView.RPC("OnGrabEnd", RpcTarget.Others);
        }
    }

    public void GrabberSetting(PhotonView photonView, SyncOVRGrabber grabber)
    {
        _grabberPhotonView = photonView;
        _grabber = grabber;
    }

    [PunRPC]
    public void Crack()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _audioSource.Play();
        }
        _grabber.GrabEnd();
        TurnOff();
        Respawn();

        Debug.Log($"OnDamageByBottle : {PlayerControlManager.Instance.IsMoveable}");
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
