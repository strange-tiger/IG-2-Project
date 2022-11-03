using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunShoot : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Vector3 _offsetPosition = new Vector3(0.0478f, 0.0146f, 0.1126f);
    [SerializeField] private Transform[] handPosition;

    [Header("BasicState")]
    [SerializeField] private float _gunRange = 16f;

    [Header("Bullet")]
    [SerializeField] private TextMeshProUGUI _bulletCountText;
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

        _bulletCount = _MAX_BULLET_COUNT;
    }

    private void Update()
    {
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

        PlayShotEffect();
    }

    private void HitTarget()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out hit, _gunRange))
        {
            if(hit.collider.tag != "ShootingObject")
            {
                return;
            }

            // 여기에 스크립트 처리
        }
    }

    private void PlayShotEffect()
    {
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
        if (Vector3.Dot(transform.forward, Vector3.down) >= 0.8f)
        {
            Debug.Log("[Gun] Reload");
            _bulletCount = _MAX_BULLET_COUNT;
        }
    }

    private void DisableBulletTrail()
    {
        _bulletTrail.SetActive(false);
    }
}
