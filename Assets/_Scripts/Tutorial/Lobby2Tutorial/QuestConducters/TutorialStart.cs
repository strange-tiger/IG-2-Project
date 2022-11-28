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
}
