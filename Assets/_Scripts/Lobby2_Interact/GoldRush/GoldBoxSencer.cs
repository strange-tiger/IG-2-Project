using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class GoldBoxSencer : MonoBehaviour
{
    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    private GameObject _goldBoxInteractionObject;

    private bool _isTherePlayer = false;
    private Transform _playerTransform;

    private void Awake()
    {
        _goldBoxInteractionObject = GetComponentInChildren<GoldBoxInetraction>().gameObject;

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;
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

        _playerTransform = other.transform.root;

        //_playerInteraction = playerInteraction;
        //_playerInteraction.IsNearTumbleweed = true;

        _isTherePlayer = true;
    }
}
