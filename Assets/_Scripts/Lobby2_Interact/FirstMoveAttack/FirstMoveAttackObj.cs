using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstMoveAttackObj : FocusableObjects
{
    [SerializeField] private GameObject _bottle;
    [SerializeField] private MeshRenderer _objMeshRenderer;
    [SerializeField] private BoxCollider _objCollider;

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
        /// <summary>
        /// Grab������ ��� Enter ����
        /// </summary>
        if (_isGrabbed == false)
        {
            return;
        }

        _objCollider.isTrigger = true;

        /// <summary>
        /// �׷� �� �÷��̾� �±׸� ���� ������Ʈ�� �ν�
        /// </summary>
        if (other.CompareTag("PlayerBody") == false)
        {
            return;
        }

        /// <summary>
        /// �÷��̾� �±װ� �νĵǸ� ���� Grab�ϰ��ִ� ����� photonView�� ��
        /// </summary>
        if (_grabberPhotonView == other.transform.root.gameObject.GetPhotonView())
        {
            return;
        }

        /// <summary>
        /// Grab�� �÷��̾� ���� �ٸ� �÷��̾�� �浹�ϸ� ���� ������ Ÿ�÷��̾�� Ÿ���� ����
        /// </summary>
        this.photonView.RPC(nameof(Crack), RpcTarget.All);
        FirstMoveAttackPlayer player = other.transform.root.GetComponentInChildren<FirstMoveAttackPlayer>();
        player.photonView.RPC("OnDamageByBottle", RpcTarget.All);
    }


    [PunRPC]
    public void OnGrabBegin()
    {
        _isGrabbed = true;
        if(photonView.IsMine)
        {
            photonView.RPC(nameof(OnGrabBegin), RpcTarget.Others);
        }
    }

    [PunRPC]
    public void OnGrabEnd()
    {
        _isGrabbed = false;
        _objCollider.isTrigger = false;
        _grabberPhotonView = null;
        _grabber = null;
        ObjPosReset();

        if (photonView.IsMine)
        {
            photonView.RPC(nameof(OnGrabEnd), RpcTarget.Others);
        }
    }

    public void GrabberSetting(PhotonView grabberPhotonView, SyncOVRGrabber grabber)
    {
        _grabberPhotonView = grabberPhotonView;
        _grabber = grabber;
    }

    [PunRPC]
    private void Crack()
    {
        /// <summary>
        /// 3D����� ������ MasterClient�� ȣ��
        /// </summary>
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
    private void Respawn()
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
    private IEnumerator CoReviveCooldown()
    {
        yield return _respawnCooldown;
        Respawn();
    }

    private new void OnDisable()
    {
        base.OnDisable();
    }
}
