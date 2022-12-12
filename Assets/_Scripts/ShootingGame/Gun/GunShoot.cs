using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using PlayerNumber = ShootingGameManager.EShootingPlayerNumber;

public class GunShoot : MonoBehaviourPun
{
    // 총이 플레이어 손 어디에 있어야 하는지
    [Header("Position")]
    [SerializeField] private Vector3 _offsetPosition = new Vector3(0.0478f, 0.0146f, 0.1126f);
    [SerializeField] private Transform[] _handPositions;

    // 플레이어 모델 관련(게임 시작 후에 보여져야 함)
    [Header("Model")]
    [SerializeField] private GameObject _playerModel;
    private MeshRenderer _renderer;
    private ShootingPlayerLoadingUI _loadingUI;

    // 총 사거리
    [Header("BasicState")]
    [SerializeField] private float _gunRange = 18f;

    // 총 발사 효과, 총알
    [Header("Bullet")]
    [SerializeField] private TextMeshProUGUI _bulletCountText;
    private ParticleSystem[] _shootEffects = new ParticleSystem[2];

    // 총 발사
    [SerializeField] private Transform _bulletSpawnTransform;
    [SerializeField] private Transform _bulletShotPoint;
    private LayerMask _breakableObjectLayer;                    // 총으로 맞출 수 있는 레이어

    // bulletTrail 오브젝트 풀링
    private List<GameObject> _bulletTrailPull = new List<GameObject>();
    private int _nextBullet = 0;

    // 총알 카운트
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

    // 라인렌더러(조준선)
    [Header("Line")]
    [SerializeField] private Color _lineRendererColor = new Color(1f, 1f, 1f, 0.4f);
    private LineRenderer _lineRenderer;
    private Vector3[] _rayPositions = new Vector3[2];

    // 사운드 효과
    [Header("Sounds")]
    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;
    private AudioSource _audioSource;

    // 진동 효과
    [Header("Vibration")]
    [SerializeField] private float _vibrationTime = 0.1f;
    [SerializeField] private float _vibrationFrequency = 0.3f;
    [SerializeField] private float _vibrationAmplitude = 0.3f;
    private WaitForSeconds _waitForViBrationTime;
    private OVRInput.Controller _mainController;                // 진동을 줄 Controller(총을 잡고 있는 손의 Controller만 진동을 줌)

    // 게임 내에서의 플레이어 정보(색, 번호, 닉네임 등)
    private string _myNickname;
    private PlayerNumber _playerNumber;
    private Vector3 _playerColor;

    // 기타 필요 컨포넌트들
    private PlayerInput _input;
    private int _primaryController;
    private bool _isReloading;

    // 현재 총을 쏠 수 있는 상태(플레이어가 게임에 참가한 상태인지)
    private bool _isShootable = false;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // 플레이어 모델 초기화
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

        // 레이어 마스크 세팅
        _breakableObjectLayer = 1 << LayerMask.NameToLayer("BreakableShootingObject");

        // 라인렌더러 세팅
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        SetRayColor();
    }

    /// <summary>
    /// 라인렌더러 색상 설정
    /// </summary>
    private void SetRayColor()
    {
        Gradient RayMaterialGradient = new Gradient();

        RayMaterialGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(_lineRendererColor, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(_lineRendererColor.a, 0.0f) });

        _lineRenderer.colorGradient = RayMaterialGradient;
    }

    /// <summary>
    /// 현재 게임의 ShootingGameManager를 연결하고 기타 초기화 진행
    /// </summary>
    /// <param name="shootingGameManager">현재 게임의  ShootingGameManager</param>
    /// <param name="_shootingPlayerLoadingUI">게임 시작 전 플레이어에게 보이는 LoadingUI(게임 시작 시 꺼줄 용도)</param>
    public void Init(ShootingGameManager shootingGameManager, ShootingPlayerLoadingUI _shootingPlayerLoadingUI)
    {
        // 로딩 화면
        _loadingUI = _shootingPlayerLoadingUI;

        // 닉네임 받아오기
        _myNickname = transform.root.GetComponentInChildren<BasicPlayerNetworking>().MyNickname;
        
        // 리볼버가 들려있는 위치 확인하기 후 위치 이동
        _input = transform.root.GetComponentInChildren<PlayerInput>();
        _primaryController = (int)_input.PrimaryController;

        transform.parent = _handPositions[_primaryController];
        transform.localPosition = new Vector3((_primaryController == 0) ? _offsetPosition.x : _offsetPosition.x * -1f, _offsetPosition.y, _offsetPosition.z);

        // 진동 효과를 줄 컨트롤러 결정
        _mainController = (_primaryController == 0) ? OVRInput.Controller.LHand : OVRInput.Controller.RHand;

        // 메니저에게 플레이어가 참가했음을 안내
        shootingGameManager.AddPlayer(_myNickname, this);
        shootingGameManager.OnGameOver.RemoveListener(StopShooting);
        shootingGameManager.OnGameOver.AddListener(StopShooting);
    }

    /// <summary>
    /// 플레이어 정보를 받아와 저장한 후, 플레이 준비
    /// </summary>
    /// <param name="playerNumber">플레이어의 번호</param>
    /// <param name="playerColor">플레이어의 색상</param>
    public void PlayerInfoSettingToStart(PlayerNumber playerNumber, Color playerColor)
    {
        _playerNumber = playerNumber;
        _playerColor = new Vector3(playerColor.r, playerColor.g, playerColor.b);

        _isShootable = true;
        _renderer.enabled = true;

        _playerModel.SetActive(true);
        
        _loadingUI.DisableLoadingPanel();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            LineRendererPositionSetting();
        }

        if (!_isShootable)
        {
            return;
        }

        Reload();
        Shot();
    }

    /// <summary>
    /// 라인렌더러 위치 세팅
    /// </summary>
    private void LineRendererPositionSetting()
    {
        _rayPositions[0] = _bulletSpawnTransform.position;
        _rayPositions[1] = _bulletSpawnTransform.position + _bulletSpawnTransform.forward * 1000f;
        _lineRenderer.SetPositions(_rayPositions);
    }

    /// <summary>
    /// 총 발사
    /// </summary>
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

    /// <summary>
    /// 총에서 레이를 쏘아 대상에 맞았는 지 판단(히트 스켄 방식)
    /// </summary>
    private void HitTarget()
    {
        RaycastHit hit;
        Ray ray = new Ray(_bulletSpawnTransform.position, _bulletSpawnTransform.forward);

        if (Physics.Raycast(ray, out hit, _gunRange, _breakableObjectLayer))
        {
            ShootingObjectHealth _health = hit.collider.GetComponent<ShootingObjectHealth>();
            Debug.Assert(_health != null);

            _health.Hit(_playerNumber, _playerColor, hit.point);
        }
    }

    /// <summary>
    /// 총 쏜 이펙트 출력
    /// </summary>
    private void PlayShotEffect()
    {
        // 컨트롤러 진동효과
        StartCoroutine(CoVibrateController());

        // 총알쏘기
        ShotBullet();

        // 이펙트 출력
        foreach (ParticleSystem effect in _shootEffects)
        {
            effect.Play();
        }
        _audioSource.PlayOneShot(_shotAudioClip);
    }

    /// <summary>
    /// 총알(BulletTrail) 쏘기
    /// </summary>
    private void ShotBullet()
    {
        GameObject bulletTrail = _bulletTrailPull[_nextBullet];

        bulletTrail.SetActive(false);
        bulletTrail.transform.position = _bulletSpawnTransform.position;
        bulletTrail.transform.LookAt(_bulletShotPoint);
        bulletTrail.SetActive(true);

        _nextBullet = (_nextBullet + 1) % _bulletTrailPull.Count;
    }

    /// <summary>
    /// 진동 효과
    /// </summary>
    private IEnumerator CoVibrateController()
    {
        OVRInput.SetControllerVibration(_vibrationFrequency, _vibrationAmplitude, _mainController);
        yield return _waitForViBrationTime;
        OVRInput.SetControllerVibration(0, 0, _mainController);
    }

    /// <summary>
    /// 재장전
    /// </summary>
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
            _bulletCount = _MAX_BULLET_COUNT;
            _audioSource.PlayOneShot(_reloadAudioClip);
            _isReloading = true;
        }
    }

    /// <summary>
    /// 게임 종료 시 더 이상 총을 쏠 수 없음
    /// </summary>
    private void StopShooting()
    {
        _isShootable = false;
        PlayerControlManager.Instance.IsRayable = true;
    }
}