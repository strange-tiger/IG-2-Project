using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRushQuest1Get : QuestConducter
{
    [SerializeField] private GameObject _goldBox;
    [SerializeField] private PlayerGoldRushInteraction _goldRushInteraction;
    private GoldBoxSencerForTutorial _goldBoxSencer;

    private void Awake()
    {
        _goldBoxSencer = _goldBox.GetComponent<GoldBoxSencerForTutorial>();
        _goldBoxSencer.OnQuestEnd -= QuestEnded;
        _goldBoxSencer.OnQuestEnd += QuestEnded;
    }

    public override void StartQuest()
    {
        _goldBoxSencer.enabled = true;
        _goldBox.SetActive(true);
        _goldRushInteraction.enabled = true;
    }

    private void OnDisable()
    {
        _goldRushInteraction.enabled = false;
        _goldBox.SetActive(false);
    }
}
