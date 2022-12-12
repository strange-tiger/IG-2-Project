#define _DEV_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class WaitingRoomRevolver : FocusableObjects
{
    private LineRenderer _lineRenderer;
    private Vector3[] _rayPositions = new Vector3[2];

    // 그랩 관련
    private bool _isGrabbed = false;
    private bool IsGrabbed
    {
        get { return _isGrabbed; }
        set
        {
            photonView.RPC(nameof(SetGrabbed), RpcTarget.All, value);
        }
    }
    private SyncOVRGrabbable _syncGrabbable;
    private PlayerInput _input;

    // 오브젝트 관련
    private BoxCollider _boxCollider;
    private Vector3 _objSpawnPos;

    //재장전
    private bool _isReloading = false;
    private bool IsReloading
    {
        get { return _isReloading; }
        set
        {
            photonView.RPC(nameof(SetReloadingValue), RpcTarget.All, value);
        }
    }

    // 총알 관련
    [SerializeField] private float _gunRange = 18f;
    [SerializeField] private TextMeshProUGUI _bulletCountText;
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

    // Effect
    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;
    private AudioSource _audioSource;

    private ParticleSystem[] _shootEffects = new ParticleSystem[2];

    // 진동 효과
    [SerializeField] private float _vibrationTime = 0.1f;
    [SerializeField] private float _vibrationFrequency = 0.3f;
    [SerializeField] private float _vibrationAmplitude = 0.3f;
    private WaitForSeconds _waitForViBrationTime;

    private LayerMask _breakableObjectLayer;

    private new void Awake()
    {
        base.Awake();

        _boxCollider = GetComponent<BoxCollider>();
        _objSpawnPos = transform.position;

        // 그랩 상태 받아오기
        _syncGrabbable = GetComponent<SyncOVRGrabbable>();
        _syncGrabbable.CallbackOnGrabHand.RemoveListener(OnGrabBegin);
        _syncGrabbable.CallbackOnGrabEnd.RemoveListener(OnGrabEnd);
        _syncGrabbable.CallbackOnGrabHand.AddListener(OnGrabBegin);
        _syncGrabbable.CallbackOnGrabEnd.AddListener(OnGrabEnd);

        // 이펙트를 위한 기타 컴포넌트 가져오기
        _shootEffects = GetComponentsInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();

        // 총 쏘기 관련 초기화
        _bulletCount = _MAX_BULLET_COUNT;
        _waitForViBrationTime = new WaitForSeconds(_vibrationTime);

        // 총알 효과 스택에 넣기
        foreach (CapsuleCollider bulltTrial in GetComponentsInChildren<CapsuleCollider>())
        {
            _bulletTrailPull.Push(bulltTrial.gameObject);
            bulltTrial.gameObject.SetActive(false);
            bulltTrial.GetComponent<WaitingRoomBulletTrail>().enabled = true;
        }

        _breakableObjectLayer = 1 << LayerMask.NameToLayer("BreakableShootingObject");

        _lineRenderer = GetComponent<LineRenderer>();
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
#if _DEV_MODE_
        _rayPositions[0] = _bulletSpawnTransform.position;
        _rayPositions[1] = _bulletSpawnTransform.position + _bulletSpawnTransform.forward * 1000f;
        _lineRenderer.SetPositions(_rayPositions);
#endif

        Reload();
        Shot();
    }

    public void OnGrabBegin(SyncOVRGrabber hand)
    {
        _isGrabbed = true;
        _input = hand.transform.parent.GetComponent<PlayerInput>();
#if _DEV_MODE_
        _lineRenderer.enabled = true;
#else
        _lineRenderer.enabled = false;
#endif
    }

    public void OnGrabEnd()
    {
        _isGrabbed = false;
        ObjPosReset();
#if _DEV_MODE_
        _lineRenderer.enabled = false;
#else
        _lineRenderer.enabled = false;
#endif
    }
    private void ObjPosReset()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = _objSpawnPos;
    }

    private void SetGrabbed(bool value)
    {
        _isGrabbed = value;
        _boxCollider.isTrigger = value;
    }

    private void Reload()
    {
        if (IsReloading)
        {
            if (Vector3.Dot(transform.forward, Vector3.down) <= 0.5f)
            {
                IsReloading = false;
            }
        }
        // 아래를 보고 있다면 장전
        else if (Vector3.Dot(transform.forward, Vector3.down) >= 0.8f)
        {
            BulletCount = _MAX_BULLET_COUNT;
            IsReloading = true;
            _audioSource.PlayOneShot(_reloadAudioClip);
        }
    }

    private void Shot()
    {
        if (!OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || BulletCount <= 0)
        {
            return;
        }
        --BulletCount;

        HitTarget();
        PlayShotEffect();
    }

    private void HitTarget()
    {
        RaycastHit hit;
        Ray ray = new Ray(_bulletSpawnTransform.position, _bulletSpawnTransform.forward);
        if (Physics.Raycast(ray, out hit, _gunRange, _breakableObjectLayer))
        {
            Scarecrow scarecrow = hit.collider.GetComponent<Scarecrow>();
            scarecrow?.Hit(hit.point);
        }
    }

    private void PlayShotEffect()
    {
        _audioSource.PlayOneShot(_shotAudioClip);
        StartCoroutine(CoVibrateController());
        photonView.RPC(nameof(ShotEffect), RpcTarget.All);
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

    [PunRPC]
    private void SetBulletCount(int value)
    {
        _bulletCount = value;
        _bulletCountText.text = _bulletCount.ToString();
    }
    [PunRPC]
    private void SetReloadingValue(bool value)
    {
        _isReloading = value;
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
