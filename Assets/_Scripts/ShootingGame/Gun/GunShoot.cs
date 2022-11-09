#define _DEV_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using PlayerNumber = ShootingGameManager.EShootingPlayerNumber;

public class GunShoot : MonoBehaviourPun
{
    [Header("Position")]
    [SerializeField] private Vector3 _offsetPosition = new Vector3(0.0478f, 0.0146f, 0.1126f);
    [SerializeField] private Transform[] _handPositions;

    [Header("BasicState")]
    [SerializeField] private float _gunRange = 18f;

    [Header("Bullet")]
    [SerializeField] private TextMeshProUGUI _bulletCountText;
    [SerializeField] private Transform _bulletSpawnTransform;
    [SerializeField] private Transform _bulletShotPoint;
    private Stack<GameObject> _bulletTrailPull = new Stack<GameObject>();
    private const int _MAX_BULLET_COUNT = 6;
    private int _bulletCount_ = 0;
    private int _bulletCount
    {
        get => _bulletCount_;
        set
        {
            _bulletCount_ = value;
            _bulletCountText.text = _bulletCount_.ToString();
        }
    }

    // 이팩트
    [Header("Effects")]
    [SerializeField] private Color _playerColor = new Color();
    private PlayerNumber _playerNumber;
    private string _myNickname;
    [SerializeField] private GameObject _hitUI;

    private ParticleSystem[] _shootEffects = new ParticleSystem[2];

    [Header("Sounds")]
    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;
    private AudioSource _audioSource;

    [Header("Vibration")]
    [SerializeField] private float _vibrationTime = 0.1f;
    [SerializeField] private float _vibrationFrequency = 0.3f;
    [SerializeField] private float _vibrationAmplitude = 0.3f;
    private OVRInput.Controller _mainController;
    private WaitForSeconds _waitForViBrationTime;

    private LayerMask _breakableObjectLayer;

    // 기타 필요 컨포넌트들
    private PlayerInput _input;
    private int _primaryController;
    private bool _isReloading;

    private LineRenderer _lineRenderer;
    private Vector3[] rayPositions = new Vector3[2];

    private void Awake()
    {
        // 이팩트를 위한 기타 컴포넌트 가져오기
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
            bulltTrial.GetComponent<BulletTrailMovement>().enabled = true;
        }

        _breakableObjectLayer = 1 << LayerMask.NameToLayer("BreakableShootingObject");

        _lineRenderer = GetComponent<LineRenderer>();
#if _DEV_MODE_
        _lineRenderer.enabled = true;
#else
        _lineRenderer.enabled = false;
#endif
    }

    [PunRPC]
    public void Reset(Transform[] handPositions, ShootingGameManager shootingGameManager)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        _handPositions = handPositions;

        _myNickname = transform.parent.GetComponentInChildren<PlayerNetworking>().MyNickname;

        // 마스터일 경우에만 실행
        shootingGameManager.AddPlayerToGame(out _playerColor, out _playerNumber, in _myNickname);

        // 리볼버가 들려있는 위치 확인하기
        _input = transform.root.GetComponentInChildren<PlayerInput>();
        _primaryController = (int)_input.PrimaryController;
        _mainController = _primaryController == 0 ? OVRInput.Controller.LHand : OVRInput.Controller.RHand;

        transform.parent = _handPositions[_primaryController];
        transform.localPosition = new Vector3((_primaryController == 0) ? _offsetPosition.x : _offsetPosition.x * -1f, _offsetPosition.y, _offsetPosition.z);
    }

    private void Update()
    {
#if _DEV_MODE_
        rayPositions[0] = _bulletSpawnTransform.position;
        rayPositions[1] = _bulletSpawnTransform.position + _bulletSpawnTransform.forward * 1000f;
        _lineRenderer.SetPositions(rayPositions);
#endif

        Reload();
        Shot();
    }

    private void Shot()
    {
        if (!_input.IsRayDowns[_primaryController] || _bulletCount <= 0)
        {
            return;
        }

        --_bulletCount;

        HitTarget();
        PlayShotEffect();
    }

    private int _score = 0;
    private void HitTarget()
    {
        RaycastHit hit;
        Ray ray = new Ray(_bulletSpawnTransform.position, _bulletSpawnTransform.forward);
        if (Physics.Raycast(ray, out hit, _gunRange, _breakableObjectLayer))
        {
            ShootingObjectHealth _health = hit.collider.GetComponent<ShootingObjectHealth>();
            int point = _health.Hit(_playerNumber);

            GameObject hitUI = Instantiate(_hitUI, hit.point, Quaternion.identity);
            hitUI.GetComponent<HitUI>().enabled = true;
            hitUI.GetComponent<HitUI>().SetPointText(_playerColor, point, point != 0);

            _score += point;
            Debug.Log("[Gun] " + _score);
        }
    }

    private void PlayShotEffect()
    {
        // 임시로 추가한 컨트롤러 진동
        StartCoroutine(CoVibrateController());

        // 총알쏘기
        ShotBullet();

        foreach (ParticleSystem effect in _shootEffects)
        {
            effect.Play();
        }
        _audioSource.PlayOneShot(_shotAudioClip);
    }

    private void ShotBullet()
    {
        GameObject bulletTrail = _bulletTrailPull.Pop();
        bulletTrail.transform.position = _bulletSpawnTransform.position;
        bulletTrail.transform.LookAt(_bulletShotPoint);
        bulletTrail.SetActive(true);
    }

    private IEnumerator CoVibrateController()
    {
        OVRInput.SetControllerVibration(_vibrationFrequency, _vibrationAmplitude, _mainController);
        yield return _waitForViBrationTime;
        OVRInput.SetControllerVibration(0, 0, _mainController);
    }

    private void Reload()
    {
        // 다시 위로 향하면 장전 종료
        if (_isReloading)
        {
            if (Vector3.Dot(transform.forward, Vector3.down) <= 0.5f)
            {
                _isReloading = false;
            }
        }
        // 아래를 보고 있다면 장전
        else if (Vector3.Dot(transform.forward, Vector3.down) >= 0.8f)
        {
            Debug.Log("[Gun] Reload");
            _bulletCount = _MAX_BULLET_COUNT;
            _audioSource.PlayOneShot(_reloadAudioClip);
            _isReloading = true;
        }
    }

    public void ReturnToBulletPull(GameObject bulletTrail)
    {
        _bulletTrailPull.Push(bulletTrail);
    }
}