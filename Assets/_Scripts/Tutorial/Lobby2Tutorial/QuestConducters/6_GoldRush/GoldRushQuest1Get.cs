using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRushQuest1Get : QuestConducter
{
    [SerializeField] private GameObject _goldBox;
    [SerializeField] private PlayerGoldRushInteraction _goldRushInteraction;

    private void Awake()
    {
        GoldBoxSencerForTutorial goldboxSencer = _goldBox.GetComponent<GoldBoxSencerForTutorial>();
        goldboxSencer.OnQuestEnd -= QuestEnded;
        goldboxSencer.OnQuestEnd += QuestEnded;
    }

    public override void StartQuest()
    {
        _goldBox.SetActive(true);
        _goldRushInteraction.enabled = true;
    }

    private void OnDisable()
    {
        _goldRushInteraction.enabled = false;
        _goldBox.SetActive(false);
    }
}
