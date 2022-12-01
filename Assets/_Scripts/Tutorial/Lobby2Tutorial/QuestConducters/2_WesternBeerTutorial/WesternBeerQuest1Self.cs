using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesternBeerQuest1Self : QuestConducter
{
    [SerializeField] private GameObject _props;
    [SerializeField] private GameObject _playerInteraction;
    private BeerInteractionForTutorial _beerInteraction;

    private void Awake()
    {
        _beerInteraction = _playerInteraction.GetComponent<BeerInteractionForTutorial>();
        _beerInteraction.OnQuestEnd -= QuestEnded;
        _beerInteraction.OnQuestEnd += QuestEnded;

        _playerInteraction.SetActive(false);
    }

    public override void StartQuest()
    {
        _props.SetActive(true);
        _playerInteraction.SetActive(true);

        base.StartQuest();
    }

    protected override void OnQuestEnded()
    {
        base.OnQuestEnded();
        _playerInteraction.SetActive(false);
    }

    private void OnDisable()
    {
        _beerInteraction.ResetPlayer();
    }
}
