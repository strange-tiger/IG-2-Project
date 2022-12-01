using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStart : QuestConducter
{
    [SerializeField] private GameObject _tutorialButtonPanel;

    public override void StartQuest()
    {
        _tutorialButtonPanel.SetActive(true);
        QuestEnded();
    }

    protected override void OnQuestEnded()
    {
        // 퀘스트 끝 효과음 나오면 안됨
    }
}
