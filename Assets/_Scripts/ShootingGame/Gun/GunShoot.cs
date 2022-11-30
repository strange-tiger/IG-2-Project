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

    [Header("Model")]
    [SerializeField] private GameObject _playerModel;
    private MeshRenderer _renderer;
    private ShootingPlayerLoadingUI _loadingUI;

    [Header("BasicState")]
    [SerializeField] private float _gunRange = 18f;

    [Header("Bullet")]
    [SerializeField] private TextMeshProUGUI _bulletCountText;
    [SerializeField] private Transform _bulletSpawnTransform;
    [SerializeField] private Transform _bulletShotPoint;
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
    private List<GameObject> _bulletTrailPull = new List<GameObject>();
    private int _nextBullet = 0;

    // 이팩트
    [Header("Effects")]
    [SerializeField] private Color _lineRendererColor = new Color(1f, 1f, 1f, 0.4f);
    private LineRenderer _lineRenderer;
    private Vector3[] _rayPositions = new Vector3[2];

    [SerializeField] private Color _playerColor = new Color();
    private Vector3 _playerColorInVector3;
    private PlayerNumber _playerNumber;
    private string _myNickname;

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

    private bool _isShootable = false;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // 초기화
        _playerModel.SetActive(false);
        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = false;
        PlayerControlManager.Instance.IsRayable = false;

        // 이팩트를 위한 기타 컴포넌트 가져오기
        _shootEffects = GetComponentsInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();

        // 총 쏘기 관련 초기화
        _bulletCount = _MAX_BULLET_COUNT;
        _waitForViBrationTime = new WaitForSeconds(_vibrationTime);

        // 총알 효과 스택에 넣기
        foreach (CapsuleCollider bulltTrial in GetComponentsInChildren<CapsuleCollider>())
        {
            _bulletTrailPull.Add(bulltTrial.gameObject);
            bulltTrial.gameObject.SetActive(false);
            bulltTrial.GetComponent<BulletTrailMovement>().enabled = true;
        }

        _breakableObjectLayer = 1 << LayerMask.NameToLayer("BreakableShootingObject");

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        SetRayColor();
    }

    private void SetRayColor()
    {
        Gradient RayMaterialGradient = new Gradient();

        RayMaterialGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(_lineRendererColor, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(_lineRendererColor.a, 0.0f) }
            );
        _lineRenderer.colorGradient = RayMaterialGradient;
    }

    public void SetManager(ShootingGameManager shootingGameManager, ShootingPlayerLoadingUI _shootingPlayerLoadingUI)
    {
        _myNickname = transform.root.GetComponentInChildren<BasicPlayerNetworking>().MyNickname;
        
        // 리볼버가 들려있는 위치 확인하기
        _input = transform.root.GetComponentInChildren<PlayerInput>();
        _primaryController = (int)_input.PrimaryController;
        _mainController = _primaryController == 0 ? OVRInput.Controller.LHand : OVRInput.Controller.RHand;

        transform.parent = _handPositions[_primaryController];
        transform.localPosition = new Vector3((_primaryController == 0) ? _offsetPosition.x : _offsetPosition.x * -1f, _offsetPosition.y, _offsetPosition.z);

        _loadingUI = _shootingPlayerLoadingUI;

        shootingGameManager.AddPlayer(_myNickname, this);

        shootingGameManager.OnGameOver.RemoveListener(StopShooting);
        shootingGameManager.OnGameOver.AddListener(StopShooting);
    }

    public void PlayerInfoSetting(PlayerNumber playerNumber, Color playerColor)
    {
        _playerNumber = playerNumber;
        _playerColor = playerColor;
        _playerColorInVector3 = new Vector3(playerColor.r, playerColor.g, playerColor.b);

        _isShootable = true;
        _playerModel.SetActive(true);
        _renderer.enabled = true;
        _loadingUI.DisableLoadingPanel();
    }

    private void Update()
    {
#if _DEV_MODE_
        if(photonView.IsMine)
        {
            _rayPositions[0] = _bulletSpawnTransform.position;
            _rayPositions[1] = _bulletSpawnTransform.position + _bulletSpawnTransform.forward * 1000f;
            _lineRenderer.SetPositions(_rayPositions);
        }
#endif
        if(!_isShootable)
        {
            return;
        }

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

    private void HitTarget()
    {
        RaycastHit hit;
        Ray ray = new Ray(_bulletSpawnTransform.position, _bulletSpawnTransform.forward);
        if (Physics.Raycast(ray, out hit, _gunRange, _breakableObjectLayer))
        {
            ShootingObjectHealth _health = hit.collider.GetComponent<ShootingObjectHealth>();
            _health.Hit(_playerNumber, _playerColorInVector3, hit.point);
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
        GameObject bulletTrail = _bulletTrailPull[_nextBullet];

        bulletTrail.SetActive(false);
        bulletTrail.transform.position = _bulletSpawnTransform.position;
        bulletTrail.transform.LookAt(_bulletShotPoint);
        bulletTrail.SetActive(true);

        _nextBullet = (_nextBullet + 1) % _bulletTrailPull.Count;
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

    private void StopShooting()
    {
        _isShootable = false;
        PlayerControlManager.Instance.IsRayable = true;
    }
}