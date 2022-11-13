using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingRoomRevolver : MonoBehaviour
{
    private bool _isGrabbed = false;
    private bool _isReloading = false;
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

    // UI및 효과
    [SerializeField]
    private TextMeshProUGUI _bulletCountText;

    [SerializeField] private AudioClip _shotAudioClip;
    [SerializeField] private AudioClip _reloadAudioClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        BulletCount = _MAX_BULLET_COUNT;

        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(_isGrabbed == false)
        {
            return;
        }

        //if (!_isShootable)
        //{
        //    return;
        //}

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
}
