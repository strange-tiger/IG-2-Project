using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRushQuest2Check : QuestConducter
{
    [SerializeField] private float _checkTime = 3f;
    private WaitForSeconds _waitForCheck;

    private void Awake()
    {
        _waitForCheck = new WaitForSeconds(_checkTime);
    }

    public override void StartQuest()
    {
        StopAllCoroutines();
        StartCoroutine(CoQuestEnd());
        base.StartQuest();
    }

    private IEnumerator CoQuestEnd()
    {
        yield return _waitForCheck;
        QuestEnded();
    }
}
