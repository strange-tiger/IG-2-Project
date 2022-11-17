using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class WaitingRoomRevolver : MonoBehaviourPun
{
    private bool _isGrabbed = false;
    private SyncOVRGrabbable _syncGrabbable;
    private bool _isReloading = false;

    //private SphereCollider _collider;

    // �Ѿ� ����
    [SerializeField] private Transform _bulletSpawnTransform;
    [SerializeField] private Transform _bulletShotPoint;
    private Stack<GameObject> _bulletTrailPull = new Stack<GameObject>();
    private const int _MAX_BULLET_COUNT = 6;
    private int _bulletCount = 0;
    private int BulletCount
    {
        get { return _bulletCount; }
        set
        {
            photonView.RPC(nameof(SetBulletCount), RpcTarget.All, value);
        }
    }

    [SerializeField] private TextMeshProUGUI _bulletCountText;

    // ���� ����
    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;
    private AudioSource _audioSource;

    // ����Ʈ ����
    private ParticleSystem[] _shootEffects = new ParticleSystem[2];

    // ���� ȿ��
    [SerializeField] private float _vibrationTime = 0.1f;
    [SerializeField] private float _vibrationFrequency = 0.3f;
    [SerializeField] private float _vibrationAmplitude = 0.3f;
    private WaitForSeconds _waitForViBrationTime;

    private void Awake()
    {
        //_collider = GetComponent<SphereCollider>();

        // �׷� ���� �޾ƿ���
        _syncGrabbable = GetComponent<SyncOVRGrabbable>();
        _syncGrabbable.CallbackOnGrabBegin = OnGrabBegin;
        _syncGrabbable.CallbackOnGrabEnd = OnGrabEnd;

        // ����Ʈ�� ���� ��Ÿ ������Ʈ ��������
        _shootEffects = GetComponentsInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();

        // �� ��� ���� �ʱ�ȭ
        BulletCount = _MAX_BULLET_COUNT;
        _waitForViBrationTime = new WaitForSeconds(_vibrationTime);

        // �Ѿ� ȿ�� ���ÿ� �ֱ�
        foreach (CapsuleCollider bulltTrial in GetComponentsInChildren<CapsuleCollider>())
        {
            _bulletTrailPull.Push(bulltTrial.gameObject);
            bulltTrial.gameObject.SetActive(false);
            bulltTrial.GetComponent<BulletTrailMovement>().enabled = true;
        }
    }

    void Update()
    {
        if (_isGrabbed == false)
        {
            return;
        }
        if (photonView.IsMine == false)
        {
            return;
        }

        Reload();
        Shot();
    }

    [PunRPC]
    public void OnGrabBegin()
    {
        _isGrabbed = true;
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(OnGrabBegin), RpcTarget.Others);
        }
    }

    [PunRPC]
    public void OnGrabEnd()
    {
        _isGrabbed = false;
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(OnGrabEnd), RpcTarget.Others);
        }
    }

    private void Reload()
    {
        if (_isReloading)
        {
            if (Vector3.Dot(transform.forward, Vector3.down) <= 0.5f)
            {
                _isReloading = false;
            }
        }
        // �Ʒ��� ���� �ִٸ� ����
        else if (Vector3.Dot(transform.forward, Vector3.down) >= 0.8f)
        {
            Debug.Log("[Gun] Reload");
            _bulletCount = _MAX_BULLET_COUNT;
            _isReloading = true;
            _audioSource.PlayOneShot(_reloadAudioClip);
        }
    }

    private void Shot()
    {
        if (!OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || _bulletCount <= 0)
        {
            return;
        }
        --BulletCount;
        PlayShotEffect();
    }

    [PunRPC]
    private void SetBulletCount(int value)
    {
        _bulletCount = value;
        _bulletCountText.text = _bulletCount.ToString();
    }


    private void PlayShotEffect()
    {
        photonView.RPC(nameof(ShotEffect), RpcTarget.All);

        // �ӽ÷� �߰��� ��Ʈ�ѷ� ����
        StartCoroutine(CoVibrateController());

        _audioSource.PlayOneShot(_shotAudioClip);
    }

    [PunRPC]
    private void ShotEffect()
    {
        ShotBullet();

        foreach (ParticleSystem effect in _shootEffects)
        {
            effect.Play();
        }
    }

    private IEnumerator CoVibrateController()
    {
        OVRInput.SetControllerVibration(_vibrationFrequency, _vibrationAmplitude, OVRInput.Controller.RHand);
        yield return _waitForViBrationTime;
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RHand);
    }

    private void ShotBullet()
    {
        GameObject bulletTrail = _bulletTrailPull.Pop();
        bulletTrail.transform.position = _bulletSpawnTransform.position;
        bulletTrail.transform.LookAt(_bulletShotPoint);
        bulletTrail.SetActive(true);
    }

    public void ReturnToBulletPull(GameObject bulletTrail)
    {
        _bulletTrailPull.Push(bulletTrail);
    }
}
