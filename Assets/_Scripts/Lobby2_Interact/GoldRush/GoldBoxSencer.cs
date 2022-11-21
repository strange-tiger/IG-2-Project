//#define _DEV_MODE_
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;

public class GoldBoxSencer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Vector3 _onPlayerPosition = new Vector3(0f, 2.35f, 0f);
    private Vector3 ZERO_VECTOR = Vector3.zero;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    private GoldBoxInetraction _interaction;

    private bool _isTherePlayer = false;
    private Transform _playerTransform;
    private PlayerGoldRushInteraction _playerInteraction;

    private Collider _sencerCollider;
    private bool _isJoinedRoom = false;

    [SerializeField] private GameObject _lightLine;
    private readonly Vector3 _ZERO_VECTOR = Vector3.zero;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _interaction = GetComponentInChildren<GoldBoxInetraction>();

        _sencerCollider = GetComponent<Collider>();

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;
        _outline.enabled = false;

        _rigidBody = GetComponent<Rigidbody>();

        base.OnJoinedRoom();
        _isJoinedRoom = true;
        OnEnable();
    }

    public override void OnEnable()
    {
        if(PhotonNetwork.IsMasterClient && !_isJoinedRoom)
        {
            return;
        }

        base.OnEnable();
        Debug.Log("[GoldBox] Sencer OnEnable");
        //_goldBoxInteractionObject.SetActive(true);
        //_interaction.enabled = false;
        _interaction.SetActiveObject(true);
        _interaction.EnableScript(false);

        _outline.enabled = false;

        if (_isTherePlayer)
        {
            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;
        }

        _sencerCollider.enabled = true;
    }

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    _isJoinedRoom = true;
    //    OnEnable();
    //}

    private void FixedUpdate()
    {
#if _DEV_MODE_
        if(_isTherePlayer && Input.GetKeyDown(KeyCode.A))
#else
        if (_isTherePlayer && _playerInteraction.HasInteract)
#endif
        {
            _sencerCollider.enabled = false;
            photonView.RequestOwnership();

            gameObject.transform.parent = _playerTransform;
            gameObject.transform.localPosition = _onPlayerPosition;
            gameObject.transform.localRotation = Quaternion.Euler(ZERO_VECTOR);

            _outline.enabled = false;
            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;

            _interaction.enabled = true;
            EnableScript(false);
        }

        _lightLine.transform.rotation = Quaternion.Euler(_ZERO_VECTOR);
    }

    private void OnTriggerEnter(Collider other)
    {
        GetPlayer(other, "TriggerEnter");
    }

    private void OnTriggerStay(Collider other)
    {
        if(_isTherePlayer)
        {
            return;
        }

        GetPlayer(other, "TriggerEnter");
    }

    private void GetPlayer(Collider other, string debugMessage)
    {
        if (_isTherePlayer)
        {
            return;
        }

        if (!other.CompareTag("PlayerBody"))
        {
            return;
        }

        PlayerGoldRushInteraction playerInteraction =
            other.transform.root.GetComponentInChildren<PlayerGoldRushInteraction>();
        if (!playerInteraction || playerInteraction.IsNearGoldRush)
        {
            return;
        }

        _outline.enabled = true;

        _playerTransform = 
            other.transform.root.transform;

        _playerInteraction = playerInteraction;
        _playerInteraction.IsNearGoldRush = true;

        _isTherePlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }

        if(_isTherePlayer && _playerTransform != other.transform.root)
        {
            return;
        }

        _outline.enabled = false;

        _playerInteraction.IsNearGoldRush = false;

        _isTherePlayer = false;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        _outline.enabled = false;

        if (_isTherePlayer)
        {
            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;
        }

        _sencerCollider.enabled = false;

        _rigidBody.useGravity = false;
        _rigidBody.velocity = _ZERO_VECTOR;
        _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnableScript(bool value)
    {
        photonView.RPC(nameof(EnableScriptByRPC), RpcTarget.AllBuffered, value);
    }
    [PunRPC]
    private void EnableScriptByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Sencer script {value}");
        this.enabled = value;
    }

    public void SetActiveObject(bool value)
    {
        photonView.RPC(nameof(SetActiveObjectByRPC), RpcTarget.AllBuffered, value);
    }
    [PunRPC]
    private void SetActiveObjectByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Sencer Obejct {value}");
        gameObject.SetActive(value);
    }
}
