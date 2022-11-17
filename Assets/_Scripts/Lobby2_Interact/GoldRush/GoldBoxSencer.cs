using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;

public class GoldBoxSencer : GoldBoxState
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

        if(!_isJoinedRoom)
        {
            return;
        }

        Debug.Log("[GoldBox] Sencer OnEnable");
        //_goldBoxInteractionObject.SetActive(true);
        //_interaction.enabled = false;
        _interaction.SetActiveObject(false, _interaction.name);
        _interaction.EnableScript(false, _interaction.name);
        _sencerCollider.enabled = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _interaction.SetActiveObject(true, _interaction.name);
        _interaction.EnableScript(false, _interaction.name);
        _sencerCollider.enabled = true;
    }

    private void FixedUpdate()
    {
        if (_isTherePlayer && _playerInteraction.HasInteract)
        {
            _sencerCollider.enabled = false;

            gameObject.transform.parent = _playerTransform;
            gameObject.transform.localPosition = _onPlayerPosition;
            gameObject.transform.localRotation = Quaternion.Euler(ZERO_VECTOR);

            _outline.enabled = false;
            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;

            //_interaction.enabled = true;
            _interaction.EnableScript(true, _interaction.name);
            EnableScript(false, this.name);
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


    [PunRPC]
    private void EnableScriptByRPC(bool value)
    {
        this.enabled = value;
    }

    [PunRPC]
    private void SetActiveObjectByRPC(bool value)
    {
        gameObject.SetActive(value);
    }
}
