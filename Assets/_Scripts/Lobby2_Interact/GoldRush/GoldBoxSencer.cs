#define _DEV_MODE_
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

    private void Awake()
    {
        _interaction = GetComponentInChildren<GoldBoxInetraction>();

        _sencerCollider = GetComponent<Collider>();

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;
        _outline.enabled = false;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("[GoldBox] Sencer OnEnable");
        //_goldBoxInteractionObject.SetActive(true);
        //_interaction.enabled = false;
        _interaction.SetActiveObject(true);
        _interaction.EnableScript(false);
        _sencerCollider.enabled = true;
    }

    private void FixedUpdate()
    {
#if _DEV_MODE_
        if(_isTherePlayer && Input.GetKeyDown(KeyCode.A))
#else
        if (_isTherePlayer && _playerInteraction.HasInteract)
#endif
        {
            _sencerCollider.enabled = false;

            gameObject.transform.parent = _playerTransform;
            gameObject.transform.localPosition = _onPlayerPosition;
            gameObject.transform.localRotation = Quaternion.Euler(ZERO_VECTOR);

            _outline.enabled = false;
            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;

            _interaction.enabled = true;
            //_interaction.EnableScript(true);
            EnableScript(false);
        }
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

    public void EnableScript(bool value)
    {
        photonView.RPC(nameof(EnableScriptByRPC), RpcTarget.All, value);
    }
    [PunRPC]
    private void EnableScriptByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Sencer script {value}");
        this.enabled = value;
    }

    public void SetActiveObject(bool value)
    {
        photonView.RPC(nameof(SetActiveObjectByRPC), RpcTarget.All, value);
    }
    [PunRPC]
    private void SetActiveObjectByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Sencer Obejct {value}");
        gameObject.SetActive(value);
    }
}
