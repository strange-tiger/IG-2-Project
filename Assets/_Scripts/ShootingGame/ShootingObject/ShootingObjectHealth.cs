using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerNumber = ShootingGameManager.EShootingPlayerNumber;

public class ShootingObjectHealth : MonoBehaviourPun
{
    [Serializable]
    public class ShotEffect
    {
        [SerializeField] private int _point = 1;
        [SerializeField] private bool _isThereEffect;
        [SerializeField] private GameObject _effect;
        [SerializeField] private GameObject _model;
        public GameObject Model { get => _model; set => _model = value; }
        [SerializeField] private bool _isLast;
        public bool IsLast { get => _isLast; set => _isLast = value; }

        public int ShowEffect()
        {
            if (_isThereEffect)
            {
                _effect.SetActive(true);
            }
            _model.SetActive(true);

            return _point;
        }

        public void ExitEffect()
        {
            _effect.SetActive(false);
            _model.SetActive(false);
        }
    }

    [SerializeField] private GameObject _initialModel;
    public GameObject InitialModel { get => _initialModel; set => _initialModel = value; }

    [Header("Shot Effects")]
    [SerializeField] private AudioClip[] _shotSoundEffects;
    [SerializeField] private ShotEffect[] _shotEffects;
    public ShotEffect[] ShotEffects { get => _shotEffects; private set => _shotEffects = value; }
    private int _shotEffectCount = -1;

    [Header("Hit Effect")]
    [SerializeField] private GameObject _hitUI;

    [Header("Destroy")]
    [SerializeField] private float _destroyOffsetTime = 0.5f;
    private WaitForSeconds _waitForDestroy;

    private readonly Vector3 _ZERO_VECTOR3 = Vector3.zero;
    private LayerMask _unbreakableObjectLayer;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private ShootingObjectMovement _movement;
    private static ShootingGameManager _shootingGameManager;

    private void Awake()
    {
        if (_shootingGameManager)
        {
            _shootingGameManager = FindObjectOfType<ShootingGameManager>();
        }

        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<ShootingObjectMovement>();

        _unbreakableObjectLayer = LayerMask.NameToLayer("UnbreakableShootingObject");
        _waitForDestroy = new WaitForSeconds(_destroyOffsetTime);

        _shotEffectCount = -1;

        transform.parent = _shootingGameManager.LunchObjects.transform;
    }

    public void Hit(PlayerNumber playerNumber, Vector3 playerColor, Vector3 hitPoint)
    {
        photonView.RPC("HitByServer", RpcTarget.AllBufferedViaServer,
            playerNumber, playerColor, hitPoint);
    }

    [PunRPC]
    public void HitByServer(PlayerNumber playerNumber, Vector3 playerColor, Vector3 hitPoint)
    {
        Debug.Log("[Shooting] Hit");
        if (_shotEffectCount < 0)
        {
            _initialModel.SetActive(false);
        }
        else
        {
            _shotEffects[_shotEffectCount].ExitEffect();
        }

        _shotEffectCount = (_shotEffectCount + 1) % _shotEffects.Length;
        if (_shotEffects[_shotEffectCount].IsLast)
        {
            _movement.enabled = false;
            gameObject.layer = _unbreakableObjectLayer;
            _rigidbody.velocity = _ZERO_VECTOR3;
            if(PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(DisableSelf());
            }
        }
        int point = _shotEffects[_shotEffectCount].ShowEffect();
        _shootingGameManager.AddScoreToPlayer(playerNumber, point);
        PlayEffectSound();

        if(PhotonNetwork.IsMasterClient)
        {
            GameObject hitUI = PhotonNetwork.Instantiate(_hitUI.name, hitPoint, Quaternion.identity);
            hitUI.GetComponent<HitUI>().photonView.RPC("SetPointText", RpcTarget.AllViaServer,
                playerColor, point, point != 0);
        }
    }

    private void PlayEffectSound()
    {
        int randonNumber = UnityEngine.Random.Range(0, _shotSoundEffects.Length);
        _audioSource.PlayOneShot(_shotSoundEffects[randonNumber]);
    }

    private IEnumerator DisableSelf()
    {
        yield return _waitForDestroy;
        //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);
    }
}