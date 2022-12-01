using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class OakBarrelInteraction : MonoBehaviourPun
{
    [SerializeField] private GameObject _playerOakBarrel;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private AudioClip _inOakBarrelSound;

    private AudioSource _audioSource;
    private PlayerInteraction _playerInteraction;

    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(60f);
    private PlayerControllerMove _playerControllerMove;

    private PlayerDebuffManager _playerDebuffManager;

    private MeshCollider _oakBarrelMeshCollider;
    private MeshRenderer _playerMeshRenderer;
    private MeshRenderer _oakBarrelMeshRenderer;

    private IEnumerator _oakBarrelIsGone;
    private IEnumerator _fadeOutPlayerScreen;

    private static string _player = "Player";
    private static string _oakBarrel = "OakBarrel";

    private Color _color = new Color(0, 0, 0, 0);

    private float _speedSlower = 0.002f;
    private bool _isInOak;
    public bool IsInOak { get { return _isInOak; } private set { _isInOak = value; } }

    private bool _isSelfExit;

    private void Awake()
    {
        _oakBarrelMeshRenderer = _playerOakBarrel.GetComponent<MeshRenderer>();
        _oakBarrelMeshCollider = _playerOakBarrel.GetComponent<MeshCollider>();

        _oakBarrelMeshRenderer.enabled = false;
        _oakBarrelMeshCollider.enabled = false;
    }

    private void OnEnable()
    {
        _playerInteraction = GetComponentInChildren<PlayerInteraction>();
        _playerDebuffManager = GetComponent<PlayerDebuffManager>();

    }

    private void Start()
    {
        _playerControllerMove = GetComponent<PlayerControllerMove>();

        _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
        _playerInteraction.InteractionOakBarrel.AddListener(BecomeOakBarrel);

        _playerMeshRenderer = GameObject.Find("CenterEyeAnchor").GetComponentInChildren<MeshRenderer>();

        _audioSource = GetComponentInChildren<AudioSource>();

        _oakBarrelIsGone = OakBarrelIsGone();
        _fadeOutPlayerScreen = FadeOutPlayerScreen();
        
        _playerDebuffManager.FadeMaterial.color = Color.black;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {

            if (_isInOak == true && OVRInput.GetDown(OVRInput.Button.One))
            {
                StopAllCoroutines();

                PlayerControlManager.Instance.IsRayable = true;
                _isSelfExit = false;
                OutOakBarrel();
            }

            if (_oakBarrelMeshRenderer.enabled == false && _playerModel.activeSelf == false)
            {
                _playerMeshRenderer.material.color = Color.black;
                StartCoroutine(_fadeOutPlayerScreen);
                PlayerControlManager.Instance.IsRayable = true;
                _isSelfExit = true;
                OutOakBarrel();
            }
        }
    }

    private void BecomeOakBarrel()
    {
        if (photonView.IsMine)
        {
            InOakBarrel();
            StartCoroutine(OakBarrelIsGone());
        }
    }

    private IEnumerator OakBarrelIsGone()
    {
        yield return _oakBarrelReturnTime;

        OutOakBarrel();
    }

    private IEnumerator FadeOutPlayerScreen()
    {
        yield return new WaitForSeconds(2f);

        _playerMeshRenderer.material.color = _color;
    }

    /// <summary>
    /// 플레이어 외관의 변화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    private void ActivePlayer(bool value)
    {
        _playerModel.SetActive(value);
        _oakBarrelMeshCollider.enabled = value;
    }

    /// <summary>
    /// 오크통의 변화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    private void ActiveOakBarrel(bool value)
    {
        _oakBarrelMeshRenderer.enabled = value;
        _oakBarrelMeshCollider.enabled = value;

        _isInOak = value;
    }

    /// <summary>
    /// 오크통에 들어갈 때
    /// </summary>
    private void InOakBarrel()
    {
        photonView.RPC(nameof(ActiveOakBarrel), RpcTarget.All, true);
        photonView.RPC(nameof(ActivePlayer), RpcTarget.All, false);
        photonView.RPC(nameof(OakBarrelToPlayer), RpcTarget.All, _oakBarrel);

        _playerControllerMove.MoveScale *= _speedSlower;

        _audioSource.PlayOneShot(_inOakBarrelSound);

        PlayerControlManager.Instance.IsRayable = false;
    }

    /// <summary>
    /// 오크통에서 나올 때
    /// </summary>
    private void OutOakBarrel()
    {
        photonView.RPC(nameof(ActiveOakBarrel), RpcTarget.All, false);
        photonView.RPC(nameof(ActivePlayer), RpcTarget.All, true);
        photonView.RPC(nameof(OakBarrelToPlayer), RpcTarget.All, _player);

        if (_isSelfExit)
        {
            _playerDebuffManager.CallStunDebuff();
        }

        _playerControllerMove.MoveScale /= _speedSlower;
    }

    [PunRPC]
    private void OakBarrelToPlayer(string str)
    {
        tag = str;
    }

}