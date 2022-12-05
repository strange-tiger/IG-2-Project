using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : FocusableObjects
{
    [SerializeField] private GameObject _bottle;
    [SerializeField] private MeshRenderer _objMeshRenderer;
    [SerializeField] private BoxCollider _objCollider;
    private Rigidbody _rigidbody;

    private Vector3 _objSpawnPos;
    private AudioSource _audioSource;

    private bool _isGrabbed = false;

    private SyncOVRGrabber _grabber = null;
    private SyncOVRGrabbable _syncGrabbable;
    private PhotonView _grabberPhotonView = null;

    private new void Awake()
    {
        base.Awake();
    }
    private new void OnEnable()
    {
        base.OnEnable();
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _syncGrabbable = GetComponent<SyncOVRGrabbable>();

        _syncGrabbable.CallbackOnGrabBegin.RemoveListener(OnGrabBegin);
        _syncGrabbable.CallbackOnGrabBegin.AddListener(OnGrabBegin);

        _syncGrabbable.CallbackOnGrabEnd.RemoveListener(OnGrabEnd);
        _syncGrabbable.CallbackOnGrabEnd.AddListener(OnGrabEnd);

        _syncGrabbable.CallbackGrabberSetting.RemoveListener(GrabberSetting);
        _syncGrabbable.CallbackGrabberSetting.AddListener(GrabberSetting);

        _objSpawnPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �׷� ������ ��� Enter ����
        if (_isGrabbed == false)
        {
            return;
        }

        // �׷� �� �÷��̾� �±׸� ���� ������Ʈ�� �ν�
        if (other.CompareTag("PlayerBody") == false)
        {
            return;
        }

        //�÷��̾� �±װ� �νĵǸ� ���� ����ִ� ����� photonView�� ��
        if (_grabberPhotonView == other.transform.root.gameObject.GetPhotonView())
        {
            return;
        }

        // ��ġ���� ������ ���� ������ Ÿ���� ����
        this.photonView.RPC("Crack", RpcTarget.All);
        
        FirstMoveAttackPlayer player = other.transform.root.GetComponentInChildren<FirstMoveAttackPlayer>();
        player.photonView.RPC("OnDamageByBottle", RpcTarget.All);
    }


    [PunRPC]
    public void OnGrabBegin()
    {
        IsGrab(true);
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(OnGrabBegin), RpcTarget.Others);
        }
    }

    [PunRPC]
    public void OnGrabEnd()
    {
        IsGrab(false);
        _grabberPhotonView = null;
        _grabber = null;
        ObjPosReset();

        if (photonView.IsMine)
        {
            photonView.RPC(nameof(OnGrabEnd), RpcTarget.Others);
        }
    }

    private void IsGrab(bool value)
    {
        _isGrabbed = value;
        _objCollider.isTrigger = value;
        _rigidbody.useGravity = !value;
        StopCoroutine(CoReviveCooldown());
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
        TurnOnOff(false);
        StartCoroutine(CoReviveCooldown());
    }

    private void TurnOnOff(bool value)
    {
        _objMeshRenderer.enabled = value;
        _objCollider.enabled = value;
    }
    public void Respawn()
    {
        ObjPosReset();
        TurnOnOff(true);
    }
    private void ObjPosReset()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = _objSpawnPos;
    }

    YieldInstruction _respawnCooldown = new WaitForSeconds(2.0f);
    IEnumerator CoReviveCooldown()
    {
        yield return _respawnCooldown;
        Respawn();
    }

    private new void OnDisable()
    {
        base.OnDisable();
    }
}
