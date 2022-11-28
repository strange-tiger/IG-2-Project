using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesternBeerQuest2NPC : QuestConducter
{
    [SerializeField] private GameObject _props;
    [SerializeField] private GameObject _mapNPC;

    private BeerInteractionNPCForTutorial _beerInteraction;

    private void Awake()
    {
        _beerInteraction = _props.GetComponentInChildren<BeerInteractionNPCForTutorial>();
        _beerInteraction.OnQuestEnd -= QuestEnded;
        _beerInteraction.OnQuestEnd += QuestEnded;
    }

    public override void StartQuest()
    {
        _mapNPC.SetActive(false);
        _props.SetActive(true);
    }
}
