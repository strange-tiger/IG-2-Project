using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGoldQuest : QuestConducter
{
    [SerializeField] private GameObject _tumbleweed;

    private void Awake()
    {
        TumbleweedTutorial tumbleweed = GetComponentInChildren<TumbleweedTutorial>();
        tumbleweed.OnQuestEnd -= OnQuestEnded;
        tumbleweed.OnQuestEnd += OnQuestEnded;
    }

    public override void StartQuest()
    {
        _tumbleweed.SetActive(true);
    }

    private void OnDisable()
    {
        _tumbleweed.SetActive(false);
    }
}
