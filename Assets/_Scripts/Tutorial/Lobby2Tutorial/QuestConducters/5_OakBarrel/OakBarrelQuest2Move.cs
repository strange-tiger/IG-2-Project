using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrelQuest2Move : QuestConducter
{
    [SerializeField] private float _questEndTime;
    private WaitForSeconds _waitForQuestEnd;

    private void Awake()
    {
        _waitForQuestEnd = new WaitForSeconds(_questEndTime);
    }

    public override void StartQuest()
    {
        StartCoroutine(CoQuestEnd());
    }

    private IEnumerator CoQuestEnd()
    {
        yield return _waitForQuestEnd;
        QuestEnded();
    }
}
