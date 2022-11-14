using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingRoomRevolver : MonoBehaviour
{
    private bool _isGrabbed = false;
    private bool _isReloading = false;

    //bullet
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
            _bulletCount = value;
            _bulletCountText.text = _bulletCount.ToString();
        }
    }

    // UI�� ȿ��
    [SerializeField]
    private TextMeshProUGUI _bulletCountText;

    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;
    private AudioSource _audioSource;

    private ParticleSystem[] _shootEffects = new ParticleSystem[2];

    private void Awake()
    {
        BulletCount = _MAX_BULLET_COUNT;

        _shootEffects = GetComponentsInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();

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

        //Reload();
        //Shot();
    }

    public void OnGrabBegin()
    {
        _isGrabbed = true;
    }

    public void OnGrabEnd()
    {
        _isGrabbed = false;
    }

    private void Reload()
    {
        // �ٽ� ���� ���ϸ� ���� ����
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
            _audioSource.PlayOneShot(_reloadAudioClip);
            _isReloading = true;
        }
    }

    private void Shot()
    {
        --BulletCount;
        PlayShotEffect();
    }

    private void PlayShotEffect()
    {
        // �ӽ÷� �߰��� ��Ʈ�ѷ� ����
        //StartCoroutine(CoVibrateController());

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
}
