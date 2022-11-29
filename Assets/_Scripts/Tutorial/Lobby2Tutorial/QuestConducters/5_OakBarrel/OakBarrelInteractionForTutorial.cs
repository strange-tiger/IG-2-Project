using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestEnd = QuestConducter.QuestEnd;

public class OakBarrelInteractionForTutorial : MonoBehaviour
{
    public event QuestEnd OnQuestEnd;

    [SerializeField] private GameObject _playerOakBarrel;
    [SerializeField] private GameObject[] _playerModels;

    [SerializeField] private float _reducedMoveScale = 0.2f;
    private float _originalMoveScale;

    private PlayerControllerMove _playerControllerMove;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _playerControllerMove = transform.root.GetComponentInChildren<PlayerControllerMove>();
        _originalMoveScale = _playerControllerMove.MoveScale;
        this.enabled = false;
    }

    public void GetInToOakBarrel()
    {
        PlayerControlManager.Instance.IsRayable = false;
        _playerControllerMove.MoveScale *= (1f - _reducedMoveScale);

        _playerOakBarrel.SetActive(true);
        SetActivePlayerModels(false);

        _audioSource.Play();

        OnQuestEnd.Invoke();
    }

    private void SetActivePlayerModels(bool value)
    {
        foreach(GameObject playerModel in _playerModels)
        {
            playerModel.SetActive(value);
        }
    }

    private void OnDisable()
    {
        PlayerControlManager.Instance.IsRayable = true;
        _playerControllerMove.MoveScale = _originalMoveScale;

        SetActivePlayerModels(true);
        _playerOakBarrel.SetActive(false);
    }
}
