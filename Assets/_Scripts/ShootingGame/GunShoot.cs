using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunShoot : MonoBehaviour
{
    [Header("Basic State")]
    [SerializeField] private Vector3 _offsetPosition = new Vector3(0.0478f, 0.0146f, 0.1126f);
    [SerializeField] private Transform[] handPosition;

    [Header("Bullet")]
    [SerializeField] private TextMeshProUGUI _bulletCountText;
    private const int _MAX_BULLET_COUNT = 6;
    private int _bulletCount = 0;
    private int BulletCount
    {
        get => _bulletCount;
        set
        {
            _bulletCount = value;
            _bulletCountText.text = _bulletCount.ToString();
        }
    }

    // 이팩트
    [Header("Effects")]
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _bulletTrailDisableOffsetTime = 0.05f;
    private ParticleSystem[] _shootEffects = new ParticleSystem[2];

    // 기타 필요 컨포넌트들
    private PlayerInput _input;
    private int _primaryController;

    private AudioSource _audioSource;

    private void Awake()
    {
        _input = transform.root.GetComponentInChildren<PlayerInput>();
        _primaryController = (int)_input.PrimaryController;

        transform.parent = handPosition[_primaryController];
        transform.localPosition = new Vector3((_primaryController == 0) ? _offsetPosition.x : _offsetPosition.x * -1f, _offsetPosition.y, _offsetPosition.z);

        _shootEffects = GetComponentsInChildren<ParticleSystem>();

        _audioSource = GetComponent<AudioSource>();

        BulletCount = _MAX_BULLET_COUNT;
    }

    private void Update()
    {
        Reload();
        Shoot();
    }

    private void Shoot()
    {
        if (!_input.IsRayDowns[_primaryController] || BulletCount <= 0)
        {
            return;
        }

        --BulletCount;

        _bulletTrail.SetActive(true);
        Invoke("DisableBulletTrail", _bulletTrailDisableOffsetTime);
        foreach (ParticleSystem effect in _shootEffects)
        {
            effect.Play();
        }
    }

    private void Reload()
    {
        // 아래를 보고 있다면 장전
        BulletCount = _MAX_BULLET_COUNT;
    }

    private void DisableBulletTrail()
    {
        _bulletTrail.SetActive(false);
    }
}
