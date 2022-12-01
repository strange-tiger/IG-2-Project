using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestEnd = QuestConducter.QuestEnd;

public class GoldBoxSencerForTutorial : MonoBehaviour
{
    public QuestEnd OnQuestEnd;

    [SerializeField] private Vector3 _onPlayerPosition = new Vector3(0f, 2.35f, 0f);
    [SerializeField] private Vector3 _onPlayerScale = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _originalScale;

    private Vector3 ZERO_VECTOR = Vector3.zero;

    private bool _isTherePlayer;
    private Transform _playerTransform;
    private PlayerGoldRushInteraction _playerInteraction;

    private Collider _sencerCollider;

    private Transform _originalParent;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _sencerCollider = GetComponent<Collider>();
        _originalParent = transform.parent;

        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.parent = _originalParent;

        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        transform.localScale = _originalScale;
    }

    private void FixedUpdate()
    {
        if(_isTherePlayer && _playerInteraction.HasInteract && !_playerInteraction.IsGrabbing)
        {
            _sencerCollider.enabled = false;

            _audioSource.Play();

            gameObject.transform.parent = _playerTransform;
            gameObject.transform.localPosition = _onPlayerPosition;
            gameObject.transform.localRotation = Quaternion.Euler(ZERO_VECTOR);
            gameObject.transform.localScale = _onPlayerScale;

            _playerInteraction.IsNearGoldRush = false;
            _isTherePlayer = false;

            OnQuestEnd.Invoke();

            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
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

        _playerTransform =
            other.transform.root.transform;

        _playerInteraction = playerInteraction;
        _playerInteraction.IsNearGoldRush = true;

        _isTherePlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerBody") || !_isTherePlayer)
        {
            return;
        }

        if (_isTherePlayer && _playerTransform != other.transform.root)
        {
            return;
        }

        _playerInteraction.IsNearGoldRush = false;

        _isTherePlayer = false;
    }
}