using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesternBeerQuest1Self : QuestConducter
{
    [SerializeField] private GameObject _props;
    [SerializeField] private BeerInteractionForTutorial _beerInteraction;

    private void Awake()
    {
        _beerInteraction.OnQuestEnd -= QuestEnded;
        _beerInteraction.OnQuestEnd += QuestEnded;

        _beerInteraction.enabled = false;
    }

    public override void StartQuest()
    {
        _props.SetActive(true);

        _beerInteraction.enabled = true;
    }

    protected override void OnQuestEnded()
    {
        base.OnQuestEnded();
        _beerInteraction.enabled = false;
    }
}
