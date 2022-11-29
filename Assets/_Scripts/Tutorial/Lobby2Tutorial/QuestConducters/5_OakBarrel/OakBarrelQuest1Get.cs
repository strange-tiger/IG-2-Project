using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrelQuest1Get : QuestConducter
{
    [SerializeField] private GameObject _oakBarrel;
    [SerializeField] private OakBarrelInteractionForTutorial _playerInteraction;

    private void Awake()
    {
        _oakBarrel.SetActive(false);

        _playerInteraction.enabled = false;
        _playerInteraction.OnQuestEnd -= QuestEnded;
        _playerInteraction.OnQuestEnd += QuestEnded;
    }

    public override void StartQuest()
    {
        _oakBarrel.SetActive(true);
        _playerInteraction.enabled = true;
    }

    private void OnDisable()
    {
        _oakBarrel.SetActive(false);
        _playerInteraction.enabled = false;
    }
}
