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

    // ����Ʈ
    [Header("Effects")]
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _bulletTrailDisableOffsetTime = 0.05f;
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


    // ��Ÿ �ʿ� ������Ʈ��
    private PlayerInput _input;
    private int _primaryController;
    private bool _isReloading;

    private void Awake()
    {
        _input = transform.root.GetComponentInChildren<PlayerInput>();
        _primaryController = (int)_input.PrimaryController;
        _mainController = _primaryController == 0 ? OVRInput.Controller.LHand : OVRInput.Controller.RHand;

        transform.parent = handPosition[_primaryController];
        transform.localPosition = new Vector3((_primaryController == 0) ? _offsetPosition.x : _offsetPosition.x * -1f, _offsetPosition.y, _offsetPosition.z);

        _shootEffects = GetComponentsInChildren<ParticleSystem>();

        _audioSource = GetComponent<AudioSource>();

        _bulletCount = _MAX_BULLET_COUNT;

        _waitForViBrationTime = new WaitForSeconds(_vibrationTime);

        foreach(CapsuleCollider bulltTrial in GetComponentsInChildren<CapsuleCollider>())
        {
            _bulletTrailPull.Push(bulltTrial.gameObject);
            bulltTrial.gameObject.SetActive(false);
            bulltTrial.GetComponent<BulletTrailMovement>().enabled = true;
            Debug.Log("[Bullet] " + bulltTrial.name);
        }
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
            if(hit.collider.CompareTag("ShootingObject"))
            {
                return;
            }

            // ���⿡ ��ũ��Ʈ ó��
        }
    }

    private void PlayShotEffect()
    {
        // �ӽ÷� �߰��� ��Ʈ�ѷ� ����
        StartCoroutine(CoVibrateController());

        // �Ѿ˽��
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
        // �ٽ� ���� ���ϸ� ���� ����
        if(_isReloading)
        {
            if(Vector3.Dot(transform.forward, Vector3.down) <= 0.5f)
            {
                _isReloading = false;
            }
        }
        // �Ʒ��� ���� �ִٸ� ����
        else if (Vector3.Dot(transform.forward, Vector3.down) >= 0.8f)
        {
            Debug.Log("[Gun] Reload");
            _bulletCount = _MAX_BULLET_COUNT;
            _audioSource.PlayOneShot(_reloadAudioClip);
            _isReloading = true;
        }
    }

    public void ReturnToPull(GameObject bulletTrail)
    {
        _bulletTrailPull.Push(bulletTrail);
    }
}
