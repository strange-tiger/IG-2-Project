using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGoldQuest : QuestConducter
{
    [SerializeField] private GameObject _tumbleweed;

    private void Awake()
    {
        TumbleweedTutorial tumbleweed = GetComponentInChildren<TumbleweedTutorial>();
        tumbleweed.OnQuestEnd -= QuestEnded;
        tumbleweed.OnQuestEnd += QuestEnded;
    }

    public override void StartQuest()
    {
        _tumbleweed.SetActive(true);

        base.StartQuest();
    }

    private void OnDisable()
    {
        _tumbleweed.SetActive(false);
    }
}
