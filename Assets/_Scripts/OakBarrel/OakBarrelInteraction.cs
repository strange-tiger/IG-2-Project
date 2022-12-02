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
    private MeshRenderer _oakBarrelMeshRenderer;

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
        _playerDebuffManager = GetComponent<PlayerDebuffManager>();

        if (photonView.IsMine)
        {
            photonView.RPC("PlayerSetting", RpcTarget.AllBuffered, false);
        }
    }

    private void Start()
    {
        _playerControllerMove = GetComponent<PlayerControllerMove>();
        _playerInteraction = GetComponentInChildren<PlayerInteraction>();

        if (photonView.IsMine)
        {
            _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
            _playerInteraction.InteractionOakBarrel.AddListener(BecomeOakBarrel);
        }

        _audioSource = GetComponentInChildren<AudioSource>();

        _playerDebuffManager.FadeMaterial.color = Color.black;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (_oakBarrelMeshRenderer.enabled == false && _oakBarrelMeshCollider.enabled == false && _playerModel.activeSelf == false)
            {
                _isSelfExit = false;

                OutOakBarrel();
                Debug.Log($"{photonView.IsMine}타의로 탈출");
            }

            if (_isInOak == true && OVRInput.GetDown(OVRInput.Button.One))
            {
                StopAllCoroutines();

                _isSelfExit = true;
                OutOakBarrel();
                Debug.Log($"{photonView.IsMine}스스로 탈출");
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
        _isSelfExit = true;
        OutOakBarrel();
        Debug.Log($"{photonView.IsMine}시간지나서 탈출");
    }

    [PunRPC]
    private void PlayerSetting(bool value)
    {
        _oakBarrelMeshRenderer.enabled = value;
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

        Debug.Log($"{photonView.IsMine}플레이어의 오크통 RPC");
    }

    /// <summary>
    /// 플레이어 외관의 변화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    private void ActivePlayer(bool value)
    {
        _playerModel.SetActive(value);
        _oakBarrelMeshCollider.isTrigger = value;
        Debug.Log($"{photonView.IsMine}플레이어 모델 RPC");
    }

    /// <summary>
    /// 오크통에 들어갈 때
    /// </summary>
    private void InOakBarrel()
    {
        photonView.RPC(nameof(ActiveOakBarrel), RpcTarget.AllBuffered, true);
        photonView.RPC(nameof(ActivePlayer), RpcTarget.AllBuffered, false);
        photonView.RPC(nameof(ChangePlayerTagFor), RpcTarget.AllBuffered, _oakBarrel);

        //_playerControllerMove.MoveScale *= _speedSlower;

        _audioSource.PlayOneShot(_inOakBarrelSound);

        PlayerControlManager.Instance.IsRayable = false;
        Debug.Log($"{photonView.IsMine}오크통 안으로");
    }

    /// <summary>
    /// 오크통에서 나올 때
    /// </summary>
    private void OutOakBarrel()
    {
        photonView.RPC(nameof(ActivePlayer), RpcTarget.AllBuffered, true);
        photonView.RPC(nameof(ActiveOakBarrel), RpcTarget.AllBuffered, false);
        photonView.RPC(nameof(ChangePlayerTagFor), RpcTarget.AllBuffered, _player);

        PlayerControlManager.Instance.IsRayable = true;
        //_playerControllerMove.MoveScale /= _speedSlower;

        if (!_isSelfExit)
        {
            _playerDebuffManager.CallStunDebuff();
        }
        Debug.Log($"{photonView.IsMine}오크통 밖으로");
    }

    [PunRPC]
    private void ChangePlayerTagFor(string str)
    {
        tag = str;
        Debug.Log($"{photonView.IsMine}테크교체 RPC");
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
        }
    }
}

