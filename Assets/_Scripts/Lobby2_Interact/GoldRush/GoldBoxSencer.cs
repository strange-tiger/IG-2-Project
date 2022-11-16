using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class GoldBoxSencer : MonoBehaviour
{
    [SerializeField] private Vector3 _onPlayerPosition = new Vector3(0f, 2.35f, 0f);
    private Vector3 ZERO_VECTOR = Vector3.zero;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    private GameObject _goldBoxInteractionObject;
    private GoldBoxInetraction _interaction;

    private bool _isTherePlayer = false;
    private Transform _playerTransform;
    private PlayerGoldRushInteraction _playerInteraction;

    private void Awake()
    {
        _interaction = GetComponentInChildren<GoldBoxInetraction>();
        _goldBoxInteractionObject = _interaction.gameObject;

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;
        _outline.enabled = false;
    }

    private void OnEnable()
    {
        _goldBoxInteractionObject.SetActive(true);
        _interaction.enabled = false;
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
            other.transform.root.GetComponentInChildren<OVRCameraRig>().transform;

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

    private void Update()
    {
        if(_isTherePlayer && _playerInteraction.HasInteract)
        {
            gameObject.transform.parent = _playerTransform;
            gameObject.transform.localPosition = _onPlayerPosition;
            gameObject.transform.rotation = Quaternion.Euler(ZERO_VECTOR);

            _outline.enabled = false;
            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;

            _interaction.enabled = true;
            this.enabled = false;
        }
    }
}
